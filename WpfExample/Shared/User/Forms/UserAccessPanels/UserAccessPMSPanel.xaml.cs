using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BaseLib;
using DBServices;
using DBServices.UsualEntity;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для UserAccessPMSPanel.xaml
    /// </summary>
    public partial class UserAccessPMSPanel : UserControl
    {
        #region Properties
        public static DependencyProperty AccessProperty;
        public PMS_Access Access
        {
            get { return (PMS_Access)GetValue(AccessProperty); }
            set { SetValue(AccessProperty, value); }
        }

        public static DependencyProperty PersonListProperty;
        public ObservableCollection<EntityItem<Person>> PersonList
        {
            get { return (ObservableCollection<EntityItem<Person>>)GetValue(PersonListProperty); }
            set { SetValue(PersonListProperty, value); }
        }

        public static DependencyProperty CurrentUserHasAccessOnModifyThisAccessProperty;
        public bool CurrentUserHasAccessOnModifyThisAccess
        {
            get { return (bool)GetValue(CurrentUserHasAccessOnModifyThisAccessProperty); }
            set { SetValue(CurrentUserHasAccessOnModifyThisAccessProperty, value); }
        }
        #endregion

        public UserAccessPMSPanel()
        {
            InitializeComponent();
        }

        static UserAccessPMSPanel()
        {
            FrameworkPropertyMetadata metadataAccess = new FrameworkPropertyMetadata(null, new PropertyChangedCallback(AccessProperty_OnChanged));
            AccessProperty = DependencyProperty.Register("Access", typeof(PMS_Access), typeof(UserAccessPMSPanel), metadataAccess);

            FrameworkPropertyMetadata metadataPersonList = new FrameworkPropertyMetadata(null);
            PersonListProperty = DependencyProperty.Register("PersonList", typeof(ObservableCollection<EntityItem<Person>>), typeof(UserAccessPMSPanel), metadataPersonList);

            FrameworkPropertyMetadata metadataHasAccessOnModify = new FrameworkPropertyMetadata(false);
            CurrentUserHasAccessOnModifyThisAccessProperty = DependencyProperty.Register("CurrentUserHasAccessOnModifyThisAccess", typeof(bool), typeof(UserAccessPMSPanel), metadataHasAccessOnModify);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {       
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)
                && AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                Load();
            }
        }

        void Load()
        {
            if (CurrentUserHasAccessOnModifyThisAccess)
                LoadPersonsList();
            // при отсутствии прав на модуль Персонал не видна тоже
            if (AppManager.CommonInfo.Module_IsShip || AppManager.CommonInfo.Module_IsIndependent || BaseLib.AppManager.CommonInfo.DisallowPERSONAL)
            {
                chkPersonnelManager.Visibility = System.Windows.Visibility.Collapsed;                
            }
            if (AppManager.CommonInfo.Module_IsShip)
            {                     
                chkTempateManager.Visibility = System.Windows.Visibility.Collapsed;
                tempateManagerInfo.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public bool HasChanges()
        {
            if (PartitionListHasChanges) return true;
            return false;
        }

        public void LoadUserPartitions()
        {
            LoadPartitions();
            listBoxPartitions.ItemsSource = Access.PartitionsForSave;
        }

        static void AccessProperty_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((UserAccessPMSPanel)obj).LoadUserPartitions();
        }
    }

    
}
