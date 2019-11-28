using System;
using System.Collections.Generic;
using ViewModelBaseSolutions.Services;
using UIMessager.Services.Message;
using DBServices.UsualEntity;
using Shared.UI.Vessel;
using ViewModelBaseSolutions.UIEntityHelper;
using UI;

namespace Shared.ViewModels.Services.Vessel
{
    public class ShipService : TService<UI_Ship>
    {
        /// <summary>
        /// Загрузка для редактирования
        /// </summary>
        public static UI_Ship GetById(int id, bool isShip, bool needCreateNewSettingIfItNull)
        {
            try
            {
                Ship ship = new Ship(id);                
                ship.LoadShipGroup();           
                ShipManager.LoadSettingForShip(ship);
                if (needCreateNewSettingIfItNull && ship.Setting == null)
                    ship.Setting = new ShipSetting();
                return new UI_Ship(ship);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }



        /// <summary>
        /// Загрузка судна, которое может быть скрыто
        /// </summary>
        public static UI_Ship GetShipByIdForRead(int shipId)
        {
            try
            {
                var ship = Ship.GetShipById(shipId, true);
            if (ship != null)
                return new UI_Ship(ship);
            else return null;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }


        /// <summary>
        /// Загрузка всего списка судов (с параметрами)
        /// </summary>
        public List<UI_Ship> GetAll(bool loadFullInfo = false, bool loadSettings = false, bool showHidden = false, int? excludeShip = null)
        {
            try
            {
                var list = new List<Ship>(Ship.GetAll(loadFullInfo, loadSettings: loadSettings, showHidden: showHidden, excludeShip: excludeShip));
                if (list == null) return null;
                else
                {
                    var resultList = UIEntityListConvertor.Convert<Ship, UI_Ship>(list);              
                    return resultList;
                }
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        /// <summary>
        /// Получение списка судов (с параметрами) формата UIItem с пунктом "Не выбрано"
        /// </summary>
        public List<UI_Item<UI_Ship>> GetAll_Items_WithNonSelectItem(bool loadFullInfo = false, bool loadSettings = false, bool showHidden = false, int? excludeShip = null)
        {
            List<UI_Ship> list = GetAll(loadFullInfo, loadSettings, showHidden, excludeShip);
            if (list == null) list = new List<UI_Ship>();

            return new UI_ItemCollection<UI_Ship>(list, false, true);
        }      

        // Фабрика получения списка судов (активных и скрытых) без загрузки Settings!
        public static class UI_ShipFactory
        {
            //Список судов - активных и скрытых (если надо происходит загрузка из БД)
            // используем в модуле Персонал - там, где надо оставить сведения о контрактах старых судов
            public static List<UI_Ship> ShipList
            {
                get
                {
                    if (_ShipList == null) Load();
                    return _ShipList;
                }
                private set { _ShipList = value; }
            }
            static List<UI_Ship> _ShipList;

            public static bool Load()
            {
                if (!BaseLib.AppManager.CommonInfo.Module_IsTypeCompany)
                {
                    CurrentShipManager.UpdateSettings();
                    return false;
                }

                var shipList = new List<Ship>(ShipManager.GetShipList(false, loadSettings: false, showHidden: true));
                if (shipList == null) ShipList = null;
                else
                {
                    ShipList = UIEntityListConvertor.Convert<Ship, UI_Ship>(shipList);                    
                }
                return ShipList != null;
            }          
        }
    }
}
