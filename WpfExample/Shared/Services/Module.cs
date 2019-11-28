using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Services;
using BaseLib;
using System.Collections.ObjectModel;
using DBServices.PMS;
using Shared.User;
using System.Globalization;
using Models.Account;
using Domain.Common.Services.Account;
using Domain.Services.Account;

namespace Shared
{
    internal class UserMsg : UIMessager.Services.UserMsgManager
    {
        protected override System.Resources.ResourceManager ResCurrentModule
        {
            get { return ResManager.CurrentManager; }
        }
    }

    internal static class ResManager
    {
        public static System.Resources.ResourceManager CurrentManager
        {
            get { return new System.Resources.ResourceManager("Shared.Properties.Resources", typeof(MainWindow).Assembly); }
        }

        public static string GetResource(System.Linq.Expressions.Expression<Func<string>> resName, CultureInfo culture)
        {
            string name = ((System.Linq.Expressions.MemberExpression)resName.Body).Member.Name;
            return CurrentManager.GetString(name, culture);
        }


    }

    public class Module
    {
        //public static UI_User CurrentUser { get; private set; }
        public static User.Access UserAccess { get; private set; }

        // Ksanti
        // новый доступ - по ролям
        public static List<Role> GivenRoles { get; private set; }
        // новый пользователь
        public static  Models.Account.User CurrentUserWithAccess { get; private set; }

        public static DBServices.UsualEntity.Ship CurrentShip { get { return CurrentShipManager.CurrentShip; } }
        public static int? CurrentShipId { get { return (CurrentShipManager.CurrentShip != null) ? CurrentShipManager.CurrentShip.ID : null; } }

        public delegate void LogOutMethod();
        static event LogOutMethod _LogOut;

        public delegate void ShowElementsByAccessMethod();
        static event ShowElementsByAccessMethod _ShowElementsByAccess;
        
        public static void SetUserAndAccess(UI_User user)
        {
            AppManager.UserLogin(user);
            UserAccess = new User.Access(user);

            /* новый доступ */

            IUserWithAccessService userWithAccessReadService = Domain.Services.Factories.UserServicesFactory.CreateUserWithAccessService();  
            CurrentUserWithAccess = userWithAccessReadService.GetUserWithAccess(user.Id);
            FormatGivenRoles(CurrentUserWithAccess.Accounts);           

            if (user.IsActive)
            {
                //сохранение логина пользователя
                var settings = CurrentUser_Settings.GetCurrentSettings();
                if (settings != null)
                {
                    settings.LastLogin = user.Login;
                    settings.Save();
                }
            }
        }


        private static void FormatGivenRoles(IEnumerable<UserAccount> userAccounts)
        {
            GivenRoles = new List<Role>();
            foreach (UserAccount account in userAccounts)
            {
                GivenRoles.Add(account.Role);
            }
        }

        public static void DestroyUserAndAccess()
        {
            AppManager.UserLogout();
            UserAccess = null;
            CurrentUserWithAccess = null;
            GivenRoles = null;
        }

        public static void InitializeDelegatesPMS(LogOutMethod logOutMethod, ShowElementsByAccessMethod showElementsByAccessMethod)
        {
            _LogOut = logOutMethod;
            _ShowElementsByAccess = showElementsByAccessMethod;
        }

        /// <summary>
        /// Происходит вызов метода Выхода из системы текущего пользователя
        /// </summary>
        /// <returns></returns>
        public static void LogOut()
        {
            if (_LogOut != null) _LogOut();
        }

        /// <summary>
        /// Происходит вызов метода Предоставления доступа текущего пользователя к различным функциям и элементам управления
        /// </summary>
        public static void ShowElementsByAccess()
        {
            if (_ShowElementsByAccess != null) _ShowElementsByAccess();
        }        
    }
}
