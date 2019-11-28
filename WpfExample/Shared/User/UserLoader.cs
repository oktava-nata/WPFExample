using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Services;
using BaseLib;
using UIMessager.Services.Message;

namespace Shared.User
{
    public static class UserLoader
    {
        public static bool Load()
        {
            if (AppManager.CurrentUser != null)
                return true;

            var settings = CurrentUser_Settings.GetCurrentSettings();
            if (settings == null)
                return false;

            //не требуется заново проходить авторизацию
            if (settings.Curu.HasValue)
                if (LoadDefault(settings.Curu.Value)) return true;


            Forms.AuthorizationForm autorizF = new Forms.AuthorizationForm();
            if (!string.IsNullOrEmpty(settings.Pcuru))
            {
                autorizF.IsSavePsw = true;
                autorizF.pU = settings.Pcuru;
            }
            autorizF.ShowDialog();
            return autorizF.LogIn;
        }

        static bool LoadDefault(int id)
        {
            try
            {
                UI_User user = UI_User.GetUserWithAccess(id);

                Module.SetUserAndAccess(user);
                //сбрасываем сохранение текущего пользователя
                var settings = CurrentUser_Settings.GetCurrentSettings();
                if (settings != null)
                {
                    settings.Curu = null;
                    settings.Save();
                }
                return true;
            }
            catch (Exception exp) { AppManager.WriteLog(exp, "Can not find user after reload program! Current user hasn't been saved!"); return false; }
        }

        public static void SaveCurrentUserIfExists()
        {
            if (AppManager.CurrentUser != null)
            {
                var settings = CurrentUser_Settings.GetCurrentSettings();
                if (settings != null)
                {
                    settings.Curu = AppManager.CurrentUser.Id;
                    settings.Save();
                }
            }
        }

        public static void SavePUser(string pcuru)
        {
            if (AppManager.CurrentUser != null)
            {
                var settings = CurrentUser_Settings.GetCurrentSettings();
                if (settings != null)
                {
                    settings.Pcuru = pcuru;
                    settings.Save();
                }
            }
            else AppManager.WriteLog("Saving pcuru failed! Current user hasn't been loaded!");
        }


        public static void DestroyCurrentUser()
        {
            Module.DestroyUserAndAccess();
        }
    }
}
