using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shared.Services;
using UIMessager.Services.Message;
using System.Collections.ObjectModel;
using BaseLib;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для UserListPanel.xaml
    /// </summary>
    public partial class UserListPanel : UserControl
    {
        ObservableCollection<UI_User> UserList
        {
            get { return (ObservableCollection<UI_User>)UserGrid.ItemsSource; }
            set { UserGrid.ItemsSource = value; }
        }

        public static DependencyProperty SelectUserProperty;
        public UI_User SelectUser
        {
            get { return (UI_User)GetValue(SelectUserProperty); }
            set { SetValue(SelectUserProperty, value); }
        }

        #region  Initialize And Loading
        public UserListPanel()
        {
            InitializeComponent();
        }

        static UserListPanel()
        {
            FrameworkPropertyMetadata metadataSelectUser = new FrameworkPropertyMetadata(null);
            SelectUserProperty = DependencyProperty.Register("SelectUser", typeof(UI_User), typeof(UserListPanel), metadataSelectUser);
        }

        public bool Load()
        {
            try
            {
                UserList = UserManager.GetAllActive();
                return true;
            }
            catch (UserMsg.UserMsgException  uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp);}
            return false;
        }

        void userF_RefreshUserList(UI_User SelUser)
        {
            if (Load())
            {
                if (SelUser != null)
                {
                    SelectUser = UserList.Where(item => item.Id == SelUser.Id).FirstOrDefault();
                    if (SelectUser != null) UserGrid.ScrollIntoView(SelectUser);
                }
            }
        }
        #endregion

        #region BtnClick

        private void AddMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            var userF = new Forms.UserForm();
            userF.RefreshUserList += new UserForm.RefreshUserListMethod(userF_RefreshUserList);
            if (userF.FormIsLoaded) userF.ShowDialog();
        }

        
        private void EditMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectUser != null)
            {
                var userF = new Forms.UserForm(SelectUser);
                if (userF.FormIsLoaded)
                {
                    userF.RefreshUserList += new UserForm.RefreshUserListMethod(userF_RefreshUserList);
                    //userF.UserAccessIAAPanel.RefreshUserList += new UserForm.RefreshUserListMethod(userF_RefreshUserList);/////////////
                    userF.ShowDialog();
                }
            }
        }

        private void DelMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectUser == null) return;
            if (UserManager.Delete(SelectUser)) Load();
         
        }
        #endregion

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectUser != null)
            {
                DelMenuBtn.IsEnabled = DelContextMenuBtn.IsEnabled = Module.UserAccess.IsUser_deleted(SelectUser);
                EditMenuBtn.IsEnabled = EditContextMenuBtn.IsEnabled = Module.UserAccess.IsUser_edited(SelectUser);
            }
            else
            {
                DelMenuBtn.IsEnabled = false;
                EditMenuBtn.IsEnabled = false;
            }
        }

        private void UserGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(EditMenuBtn.IsEnabled) EditMenuBtn_Click(null, null);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA))
            {
                colIAA_CompInsManager.Visibility = System.Windows.Visibility.Collapsed;
                colIAA_FlotInsManager.Visibility = System.Windows.Visibility.Collapsed;
                colIAA_Ins.Visibility = System.Windows.Visibility.Collapsed;
            }
            // В Экипаже не показываем
            if (!BaseLib.AppManager.CommonInfo.DisallowEQUIPAGE)
            {
                colExecuteEI.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                colPMS_Allpartitions.Visibility = System.Windows.Visibility.Collapsed;
                colPMS_CatManager.Visibility = System.Windows.Visibility.Collapsed;
                colPMS_Person.Visibility = System.Windows.Visibility.Collapsed;
                colPMS_TemplManager.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS) || AppManager.CommonInfo.Module_IsShip)
            {
                colPMS_TemplManager.Visibility = System.Windows.Visibility.Collapsed;             
            }
            // если прав на модуль Персонал нет, то тоже не показываем
            if (!AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS) || AppManager.CommonInfo.Module_IsShip || AppManager.CommonInfo.Module_IsIndependent
                || BaseLib.AppManager.CommonInfo.DisallowPERSONAL)
            {
                colPMS_PersonalManager.Visibility = System.Windows.Visibility.Collapsed;
            }
            // если строка не выделена, то Edit-Delete некликабельны
            DelMenuBtn.IsEnabled = false;
            EditMenuBtn.IsEnabled = false;
        }
    }
}
