using System;
using System.Windows;
using Resources.Validators;
using UIMessager.Services.Message;
using BaseLib;
using BaseLib.Services;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для UserForm.xaml
    /// </summary>
    public partial class UserForm : Window
    {
        #region Properties
        public static DependencyProperty ActionProperty;
        public ActionMode Action
        {
            get { return (ActionMode)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static DependencyProperty DissalowEquipageProperty;
        public bool DissalowEquipage
        {
            get { return (bool)GetValue(DissalowEquipageProperty); }
            set { SetValue(DissalowEquipageProperty, value); }
        }

        public static DependencyProperty CurrentUserHasAccessOnModifyThisAccessProperty;
        public bool CurrentUserHasAccessOnModifyThisAccess
        {
            get { return (bool)GetValue(CurrentUserHasAccessOnModifyThisAccessProperty); }
            set { SetValue(CurrentUserHasAccessOnModifyThisAccessProperty, value); }
        }

        UI_User EditUser
        {
            get { return (UI_User)DataContext; }
            set { DataContext = value; }
        }

        bool IsCurrentUser { get { return (EditUser.Id == AppManager.CurrentUser.Id); } }
       
        public bool WasChanged = false;

        public bool FormIsLoaded = false;

        public delegate void RefreshUserListMethod(UI_User SelUser);
        public event RefreshUserListMethod RefreshUserList;
        #endregion

        #region Constructors
        public UserForm(UI_User user)
        {
            InitializeComponent();
            FormIsLoaded = GetReadyForEditUser(user.Id);
        }

        public UserForm()
        {
            InitializeComponent();
            FormIsLoaded = GetReadyForAddUser();
        }

        static UserForm()
        {
            FrameworkPropertyMetadata metadataAction = new FrameworkPropertyMetadata(ActionMode.Viewing);
            ActionProperty = DependencyProperty.Register("Action", typeof(ActionMode),  typeof(UserForm), metadataAction);

            FrameworkPropertyMetadata metadataDissalowEquipage = new FrameworkPropertyMetadata(false);
            DissalowEquipageProperty = DependencyProperty.Register("DissalowEquipage", typeof(bool), typeof(UserForm), metadataDissalowEquipage);

            FrameworkPropertyMetadata metadataHasAccessOnModify = new FrameworkPropertyMetadata(false);
            CurrentUserHasAccessOnModifyThisAccessProperty = DependencyProperty.Register("CurrentUserHasAccessOnModifyThisAccess", typeof(bool), typeof(UserForm), metadataHasAccessOnModify);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //NATA_2018-05: remove IAA
            //if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA))
            //{
            //    UserAccessIAAPanel.Visibility = System.Windows.Visibility.Collapsed;
            //}
            DissalowEquipage = BaseLib.AppManager.CommonInfo.DisallowEQUIPAGE;
           
            if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                UserAccessPmsPan.Visibility = System.Windows.Visibility.Collapsed;
            }

            CurrentUserHasAccessOnModifyThisAccess = Module.UserAccess.IsUser_managedaccess(EditUser);
            chkBoxIsAdmin.IsEnabled = Module.UserAccess.IsAdmins_managed;
            chkBoxCanExecuteEI.IsEnabled = Module.UserAccess.IsAdmins_managed; 
        }
        #endregion

        #region Methods

        bool GetReadyForEditUser(int userId)
        {
            Action = ActionMode.Editing;
            EditUser = UserManager.GetUserWithAccess(userId);
            TxtLogin.Focus();
            return (EditUser != null);
        }

        bool GetReadyForAddUser()
        {
            Action = ActionMode.Adding;
            EditUser = UI_User.CreateNewUserWithAccess();
            TxtLogin.Focus();
            return true;
        }

        bool Save()
        {
            if (FormHasErrors()) return false;
            if (TxtPass1.Password.Length != 0) EditUser.Password = TxtPass1.Password;

            if (UserManager.SaveUser(EditUser, out WasChanged))
            {
                if (WasChanged && RefreshUserList != null) RefreshUserList(EditUser);
                TxtPass1.Password = TxtPass2.Password = string.Empty;
                return true;
            }
            else return false;
        }

        bool Add()
        {
            if (FormHasErrors()) return false;
            if (UserManager.AreUserWithLogin(EditUser.Login))
            {
                MsgError.Show(Properties.Resources.mErNotUniqueLogin);
                return false;
            }

            EditUser.Password = TxtPass1.Password;
            if (UserManager.AddUserWithAccess(EditUser))
            {
                TxtPass1.Password = TxtPass2.Password = string.Empty;
                if (RefreshUserList != null) RefreshUserList(EditUser);
                return true;
            }
            else return false;
        }
        #endregion

        #region BtnClick
        private void BtnAply_Click(object sender, RoutedEventArgs e)
        {
            if (AddingExecute()) GetReadyForAddUser();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SavingExecute())
            {
                Action = ActionMode.None;
                Close();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddingExecute())
            {
                Action = ActionMode.None;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void TxtPass1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Action == ActionMode.Adding && TxtPass1.Password.Length == 0)
                TxtPass1.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
            else
                TxtPass1.Style = (Style)(App.Current.Resources["PasswordBoxOk"]);
        }
        private void TxtPass2_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((TxtPass1.Password.Length > 0 || Action == ActionMode.Adding) && TxtPass2.Password.Length == 0)
                TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
            else
                TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxOk"]);
        }

        private bool FormHasErrors()
        {
            bool erValidPsw = false;
            if (Action == ActionMode.Editing)
            {
                if (TxtPass1.Password.Length > 0 && TxtPass2.Password.Length < 1)
                {
                    TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
                    erValidPsw = true;
                }
                if (TxtPass1.Password.Length > 0 && TxtPass2.Password.Length > 0 && TxtPass1.Password.CompareTo(TxtPass2.Password) != 0)
                {
                    TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxErrorRepeat"]);
                    erValidPsw = true;
                }
            }
            else if (Action == ActionMode.Adding)
            {
                if (TxtPass1.Password.Length == 0)
                {
                    TxtPass1.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
                    erValidPsw = true;
                }
                if (TxtPass2.Password.Length == 0)
                {
                    TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
                    erValidPsw = true;
                }

                if (TxtPass1.Password.Length > 0 && TxtPass2.Password.Length > 0 && TxtPass1.Password.CompareTo(TxtPass2.Password) != 0)
                {
                    TxtPass2.Style = (Style)(App.Current.Resources["PasswordBoxErrorRepeat"]);
                    erValidPsw = true;
                }
            }

            bool UpdateSource = (Action == ActionMode.Adding);

            bool erValidUserData = ValidateHelper.HasErrors(EditFields, UpdateSource);

            if (erValidPsw || erValidUserData)
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return true;
            }
            return false;
        }

        #region Handling Methods
        bool AddingExecute()
        {
            return Add();
        }

        bool SavingExecute()
        {
            return Save();
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = CheckFinishUserWork();
        }

        public bool CheckFinishUserWork()
        {
            BtnCancel.Focus();

            if (Action == ActionMode.Adding && EditUser.Id <= 0
                || Action == ActionMode.Editing && (HasChanges()))
                return !Shared.Services.FormActionsCompletion.Complate
                            (Action
                            , new Shared.Services.FormActionsCompletion.OnComfirmAction(AddingExecute)
                            , new Shared.Services.FormActionsCompletion.OnComfirmAction(SavingExecute));
            return false;
        }

        private bool HasChanges()
        {
            if (EditUser.WasPropertiesValuesChanged) return true;
            if (AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS) && UserAccessPmsPan.HasChanges()) return true;

            if (TxtPass1.Password.Length > 0) return true;
            return false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (IsCurrentUser && WasChanged)
            {
                UserManager.RefreshCurrentUserWithAccess();
                Module.ShowElementsByAccess();
            }
            //NATA_2018-05: remove IAA
            //if (AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA) && UserAccessIAAPanel.IsInspectorsDataMayBeChanged && !WasChanged)
            //{
            //    Module.ReloadInspectionsDataOnActiveTab();
            //    // сделано на всякий случай или на будущее, сейчас не должно выполняться условие (WasChangedInspectorData && RefreshUserList != null)  
            //    // - т.к. напрямую  (т.е. на этой форме) редактировать свои данные как инспектора может только текущий пользователь, который не имеет 
            //    // доступа к форме со списком пользователей и поэтому в этом случае всегда RefreshUserList = null
            //    if (RefreshUserList != null)
            //        RefreshUserList(EditUser);
            //}

        }

    }
}
