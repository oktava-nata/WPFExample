using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DBServices;
using BaseLib;
using UIMessager.Services.Message;
using Shared.Forms;
using DBServices.UsualEntity;
using System.Globalization;

namespace Shared
{
    public static class ShipManager
    {
        /// <summary>
        ///  Загрузка всех судов 
        /// </summary>
        /// <param name="loadFullInfo"> загружать с типом судна, свойствами </param>
        /// <returns></returns>
        public static ObservableCollection<Ship> GetShipList(bool loadFullInfo = false, bool loadSettings = false, bool showHidden = false, int? excludeShip = null)
        {
            try
            {
                return Ship.GetAll(loadFullInfo, loadSettings: loadSettings, showHidden: showHidden, excludeShip: excludeShip);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static bool LoadPropertyValuesForShip(Ship ship)
        {
            try
            {
                ship.LoadPropertiesValues();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }
        public static Ship ReloadShip(Ship ship)
        {
            try
            {
                return new Ship(ship.Id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static Ship GetShip(int id)
        {
            try
            {
                return new Ship(id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static bool DeleteShip(Ship ship)
        {
            if (MsgConfirm.Show(global::Resources.Properties.ResourcesReplaced.mConfirmDelShip) == System.Windows.MessageBoxResult.No) return false;

            UIMessager.Forms.ProgressForm progressForm = new UIMessager.Forms.ProgressForm();
            try
            {
                progressForm.Show();
                ship.Delete();
                progressForm.Close();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            progressForm.Close();
            return false;
        }

        public static bool HideShip(Ship ship)
        {
            if (MsgConfirm.Show(global::Resources.Properties.ResourcesReplaced.mConfirmDelShip) == System.Windows.MessageBoxResult.No) return false;

            UIMessager.Forms.ProgressForm progressForm = new UIMessager.Forms.ProgressForm();
            try
            {
                progressForm.Show();
                ship.Hide();
                progressForm.Close();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            progressForm.Close();
            return false;
        }

        #region ShipType and ShipProperties and ShipFiles
        internal static ObservableCollection<ShipType> GetShipTypes()
        {
            try
            {
                var list = (System.Threading.Thread.CurrentThread.CurrentUICulture.CompareInfo == new CultureInfo("en-US").CompareInfo) ? 
                    ShipType.GetAll().OrderBy(i => i.Name_EN):
                    ShipType.GetAll().OrderBy(i => i.Name_RU);
                if (list != null)               
                    return new ObservableCollection<ShipType>(list);                
                else return null;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static ObservableCollection<EntityItem<ShipType>> GetShipTypeItems()
        {
            List<ShipType> types = GetShipTypes().ToList();
            if (types == null) return null;

            return EntityItemManager.GenerateEntityItemList<ShipType>(types, false);
        }

        internal static ObservableCollection<ShipProperty> GetShipProperties()
        {
            try
            {
                return ShipProperty.GetAll();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        internal static ObservableCollection<IAttachedFile> GetFilesForShip(Ship ship)
        {
            try
            {
                var list = Ship_File.GetAllForShip(ship.Id);
                return new ObservableCollection<IAttachedFile>(list);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        internal static IAttachedFile CreateNewFileForShip()
        {
            return new AttachedFileInfo();
        }

        internal static bool AddFileForShip(Ship ship, IFile file)
        {
            try
            {
                Ship_File shipFile = new Ship_File
                {
                    IdShip = ship.Id,
                    File = file as AttachedFileInfo
                };
                shipFile.AddComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Adding, exp); }
            return false;
        }

        #endregion

        #region ShipSetting
        public static bool LoadSettingForShip(Ship ship)
        {
            try
            {
                int? shipId = ship.Id == 0 ? (int?)null : ship.Id;

                ship.Setting = ShipSetting.GetForShip(shipId, true);
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        /// <summary>
        ///  Получение префикса для судна 
        /// </summary>        
        /// <returns></returns>
        public static string GetShipPrefix(int? shipId)
        {
            try
            {
                if (BaseLib.AppManager.CommonInfo.Module_IsCompany)
                    return ShipFactory.GetShipPrefix(shipId.Value);
                return CurrentShipManager.CurrentShip.Setting == null ? null : CurrentShipManager.CurrentShip.Setting.Prefix;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        /// <summary>
        ///  Получение единиц груза для судна (MRV модуль) 
        /// </summary>        
        public static UnitOfCargoValue? GetShipUnitOfCargo(int? shipId)
        {
            try
            {
                if (BaseLib.AppManager.CommonInfo.Module_IsCompany)
                {
                    UnitOfCargoValue? unit = ShipFactory.GetShipUnitOfCargo(shipId.Value);
                    return (unit.HasValue) ? unit.Value : default(UnitOfCargoValue);
                }
                return CurrentShipManager.CurrentShip.Setting == null ? default(UnitOfCargoValue) : CurrentShipManager.CurrentShip.Setting.UnitOfCargo;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }
        /// <summary>
        ///  Получение единиц груза для судна в виде локализованной строки (MRV модуль) 
        /// </summary>
        public static string GetShipUnitOfCargoAsString(int? shipId)
        {
            UnitOfCargoValue? unit = GetShipUnitOfCargo(shipId);
            return (unit.HasValue) ? new UIViewRules.UnitOfCargoConverter().Convert(unit, typeof(DBServices.UnitOfCargoValue), null, null).ToString() : null;
        }
       
        #endregion
    }
}
