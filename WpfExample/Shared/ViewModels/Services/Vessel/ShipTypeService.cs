using System;
using System.Collections.Generic;
using System.Linq;
using ViewModelBaseSolutions.UIEntityHelper;
using ViewModelBaseSolutions.Services;
using Shared.UI.Vessel;
using UIMessager.Services.Message;
using System.Collections.ObjectModel;
using DBServices.UsualEntity;
using UI;
using System.Globalization;

namespace Shared.ViewModels.Services.Vessel
{
    public class ShipTypeService : TService<UI_ShipType>
    {
        public List<UI_ShipType> GetAll()
        {
            try
            {
                var list = DBServices.UsualEntity.ShipType.GetAll();

                if (list == null) return null;
                else
                {
                    // сортировка с учетом локализации
                    var c = UIEntityListConvertor.Convert<DBServices.UsualEntity.ShipType, UI_ShipType>(list);
                    return (System.Threading.Thread.CurrentThread.CurrentUICulture.CompareInfo == new CultureInfo("en-US").CompareInfo) ?
                    c.OrderBy(i => i.Name_EN).ToList() :
                    c.OrderBy(i => i.Name_RU).ToList();
                }
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        /// <summary>
        /// Получение списка типов судов формата UIItem + строка "Все" 
        /// </summary>
        public List<UI_Item<UI_ShipType>> GetAll_Items_WithAllItem()
        {
            List<UI_ShipType> list = GetAll();
            if (list == null) list = new List<UI_ShipType>();

            return new UI_ItemCollection<UI_ShipType>(list, true, false);
        }

        /// <summary>
        /// Получение списка типов судов формата UIItem + строка "Не выбрано" 
        /// </summary>
        public List<UI_Item<UI_ShipType>> GetAll_Items_WithNonSelectItem()
        {
            List<UI_ShipType> list = GetAll();
            if (list == null) list = new List<UI_ShipType>();

            return new UI_ItemCollection<UI_ShipType>(list, false, true);
        }

        public UI_ShipType GetByIdForEdit(int id)
        {
            try
            {
                return new UI_ShipType(id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public UI_ShipType GetById(int id)
        {
            try
            {
                var obj = DBServices.UsualEntity.ShipType.GetById(id);
                UI_ShipType result = new UI_ShipType(obj);
                return result;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public bool HasShips(UI_ShipType type)
        {
            try
            {
                return type.HasShips();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }



        // Фабрика получения типов судов
        public static class UI_ShipTypeFactory
        {
            //Список судов (если надо происходит загрузка из БД)
            public static List<UI_ShipType> ShipTypeList
            {
                get
                {
                    if (_ShipTypeList == null) Load();
                    return _ShipTypeList;
                }
                private set { _ShipTypeList = value; }
            }
            static List<UI_ShipType> _ShipTypeList;

            public static bool Load()
            {            
                var shipTypeList = DBServices.UsualEntity.ShipType.GetAll();
                if (shipTypeList == null) shipTypeList = null;
                else
                {
                    ShipTypeList = UIEntityListConvertor.Convert<ShipType, UI_ShipType>(shipTypeList);
                }
                return ShipTypeList != null;
            }
        }
    }
}
