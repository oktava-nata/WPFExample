using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Shared.User;
using UIMessager.Services.Message;
using System.Windows;
using BaseLib;

namespace Shared
{
    static class UserManager
    {
        public static ObservableCollection<UI_User> GetAllActive()
        {
            try
            {
                return UI_User.GetAllActive();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static UI_User GetUserWithAccess(int id)
        {
            try
            {
                return UI_User.GetUserWithAccess(id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public static bool RefreshCurrentUserWithAccess()
        {
            try
            {
                UI_User user = AppManager.CurrentUser as UI_User;
                user.Refresh();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        public static bool AreUserWithLogin(string login)
        {
            try
            {
                return UI_User.AreUserWithLogin(login);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        public static bool AddUserWithAccess(UI_User user)
        {
            try
            {
                user.AddComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Adding, exp); }
            return false;
        }

        public static bool SaveUser(UI_User user, out bool wasSave)
        {
            wasSave = false;
            try
            {
                wasSave = user.SaveComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        public static bool Delete(UI_User user)
        {
            if (MsgConfirm.Show(Properties.Resources.mConfirmDelUser) == MessageBoxResult.No) return false;
            try
            {
                user.Delete();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            return false;
        }
    }
}
