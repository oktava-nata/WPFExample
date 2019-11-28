using BaseLib;

// TODO: В Модуле Компании надо перестать использовать эти методы проверки прав доступа!!!
namespace Shared.User
{
    public partial class Access
    {
        UI_User _user { get; set; }

        public CH_Access CH { get; set; }

        public Access(UI_User user)
        {
            _user = user;
            CH = new CH_Access(user);
        }

        /// <summary>
        /// Право управления общими для судна и компании справочниками в системе: 
        /// Например, справочник должностей; для системы PMS это справочники разделов, типов счётчиков, типов работ и т.п.
        /// </summary>
        public bool Can_ManageInformationBooks
        {
            get { return AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.Ship 
                && _user.IsAdmin; }
        }

        #region Common Access
        public bool IsSysAdmin
        {
            get { return _user.IsSysAdmin; }
        }

        public bool IsAdmin
        {
            get { return _user.IsAdmin; }
        }

        public bool IsSuperUser
        {
            get { return _user.IsSuperUserOnShip && AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship; }
        }

        public bool IsInformationBooks_viewed
        {
            get { return !_user.IsSysAdmin; }
        }

        public bool IsInformationBooks_modifiable
        {
            get { return _user.IsAdmin; }
        }


        public bool IsExportImport_available
        {
            get { return _user.CanExecuteExportImport; }
        }

        public bool IsExportImportJournal_viewed
        {
            get { return !_user.IsSysAdmin; }
        }

        public bool IsUsers_managed
        {
            get { return (_user.IsAdmin || _user.IsSysAdmin); }
        }

        public bool IsUsersMenu_available
        {
            get { return (_user.IsAdmin); }
        }

        public bool IsAdmins_managed
        {
            get { return _user.IsSysAdmin; }
        }

        public bool IsApplicationSettings_managed
        {
            get { return _user.IsAdmin; }
        }
        #endregion

       

        /// <summary>
        /// Определение может ли текущий пользователь редактировать данные конкретного пользователя 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUser_edited(UI_User user)
        {
            if (user.Id == _user.Id) return true;
            if (user.IsSysAdmin) return false;
            if (user.IsAdmin && IsAdmins_managed) return true;
            if (!user.IsAdmin && IsUsers_managed) return true;
            return false;
        }

        /// <summary>
        /// Определение может ли текущий пользователь удалять конкретного пользователя 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUser_deleted(UI_User user)
        {            
            if (user.IsSysAdmin) return false;
            if (user.IsAdmin && IsAdmins_managed) return true;
            if (!user.IsAdmin && IsUsers_managed) return true;
            return false;
        }

        /// <summary>
        /// Определение может ли текущий пользователь удалять конкретного пользователя 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUser_managedaccess(UI_User user)
        {
            if (user.Id == _user.Id) return IsOwnUserAccess_modifiable;
            if (user.IsSysAdmin) return false;
            if (user.IsAdmin && IsAdmins_managed) return true;
            if (!user.IsAdmin && IsUsers_managed) return true;
            return false;
        }
    }
}
