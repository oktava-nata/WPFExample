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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DBServices;
using Resources.Services;
using DBServices.UsualEntity;
using Shared.UI.Vessel;

namespace Shared.Vessel
{
    /// <summary>
    /// Логика взаимодействия для VesselsForm.xaml
    /// </summary>
    public partial class VesselsForm : Window
    {
        #region Properties
        public delegate void OpenEditVesselFormMethod(Ship ship, Action<UI_Ship> method);
        event OpenEditVesselFormMethod OpenEditVesselForm;

        public delegate void OpenViewVesselFormMethod(Ship ship);
        event OpenViewVesselFormMethod OpenViewVesselForm;

        public delegate void OpenAddVesselFormMethod(Action<UI_Ship> method);
        event OpenAddVesselFormMethod OpenAddVesselForm;

        public static DependencyProperty ShipListProperty;
        public ObservableCollection<UI_Ship> ShipList
        {
            get { return (ObservableCollection<UI_Ship>)GetValue(ShipListProperty); }
            set { SetValue(ShipListProperty, value); }
        }

        public static DependencyProperty SelectShipProperty;
        public UI_Ship SelectShip
        {
            get { return (UI_Ship)GetValue(SelectShipProperty); }
            set { SetValue(SelectShipProperty, value); }
        }

        public bool WasLoadedSuccessfully { get; private set; }

        public bool HasAccessOnModify { get { return Shared.Module.UserAccess.IsInformationBooks_modifiable; } }

        public bool IsShowMRVInfo { get { return BaseLib.AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany && !BaseLib.AppManager.CommonInfo.DisallowMRV; } }

        public bool IsIndependent { get { return BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany; } }

        Shared.ViewModels.Services.Vessel.ShipService _ShipService;  
        #endregion

        #region Constructors
        public VesselsForm(OpenAddVesselFormMethod openAddVesselFormMethod, OpenEditVesselFormMethod openEditVesselFormMethod, OpenViewVesselFormMethod openViewVesselFormMethod)
        {
            InitializeComponent();
            _ShipService = new Shared.ViewModels.Services.Vessel.ShipService();
            WasLoadedSuccessfully = Load();
            OpenEditVesselForm = openEditVesselFormMethod;
            OpenAddVesselForm = openAddVesselFormMethod;
            OpenViewVesselForm = openViewVesselFormMethod;        
        }

        static VesselsForm()
        {
            FrameworkPropertyMetadata metadataShipList = new FrameworkPropertyMetadata(null);
            ShipListProperty = DependencyProperty.Register("ShipList", typeof(ObservableCollection<UI_Ship>), typeof(VesselsForm), metadataShipList);

            FrameworkPropertyMetadata metadataSelectShip = new FrameworkPropertyMetadata(null);
            SelectShipProperty = DependencyProperty.Register("SelectShip", typeof(UI_Ship), typeof(VesselsForm), metadataSelectShip);
        }

        void InitializeCommands()
        {
            CommandBinding cmdAdd = new CommandBinding(MenuCommands.Add);
            cmdAdd.Executed += new ExecutedRoutedEventHandler(cmdAdd_Executed);
            this.Menu.CommandBindings.Add(cmdAdd);
            this.ContextMenu.CommandBindings.Add(cmdAdd);

            CommandBinding cmdEdit = new CommandBinding(MenuCommands.Edit);
            cmdEdit.Executed += new ExecutedRoutedEventHandler(cmdEdit_Executed);
            cmdEdit.CanExecute += new CanExecuteRoutedEventHandler(cmdEdit_CanExecute);
            this.Menu.CommandBindings.Add(cmdEdit);
            this.ContextMenu.CommandBindings.Add(cmdEdit);

            CommandBinding cmdView = new CommandBinding(MenuCommands.View);
            cmdView.Executed += new ExecutedRoutedEventHandler(cmdView_Executed);
            cmdView.CanExecute += new CanExecuteRoutedEventHandler(cmdView_CanExecute);
            this.Menu.CommandBindings.Add(cmdView);
            this.ContextMenu.CommandBindings.Add(cmdView);

            CommandBinding cmdDel = new CommandBinding(MenuCommands.Delete);
            cmdDel.Executed += new ExecutedRoutedEventHandler(cmdDel_Executed);
            cmdDel.CanExecute += new CanExecuteRoutedEventHandler(cmdEdit_CanExecute);
            this.Menu.CommandBindings.Add(cmdDel);
            this.ContextMenu.CommandBindings.Add(cmdDel);

            CommandBinding cmdFiles = new CommandBinding(MenuCommands.Files);
            cmdFiles.Executed += new ExecutedRoutedEventHandler(cmdFiles_Executed);
            cmdFiles.CanExecute += new CanExecuteRoutedEventHandler(cmdView_CanExecute);
            this.Menu.CommandBindings.Add(cmdFiles);
            this.ContextMenu.CommandBindings.Add(cmdFiles);
        }

        void ClearCommands()
        {
            this.Menu.CommandBindings.Clear();
            this.ContextMenu.CommandBindings.Clear();
        }

        private void VesselsForm_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCommands();
            ComponentsVisability();
        }

        private void VesselsForm_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearCommands();
        }

        void ComponentsVisability()
        {
            MBtn_Add.Visibility = (HasAccessOnModify && BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany)
                 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            MBtn_Del.Visibility = MCBtn_Del.Visibility =
                 (HasAccessOnModify && BaseLib.AppManager.CommonInfo.Module_IsTypeCompany)
                 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            MBtn_Edit.Visibility = MCBtn_Edit.Visibility = (HasAccessOnModify)
                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            PrefixReq.Visibility = (BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company)
                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        #endregion

        bool Load()
        {
            return Load(null);
        }

        bool Load(UI_Ship selectShip = null)
        {
            if (selectShip == null && SelectShip != null) selectShip = SelectShip;

            ShipList = new ObservableCollection<UI_Ship>(_ShipService.GetAll(true, loadSettings: true));
            if (ShipList != null) Select(selectShip);

            return ShipList != null;
        }

        void Select(UI_Ship ship)
        {
            SelectShip = (ship != null) ? ShipList.Where(p => p.Id == ship.Id).FirstOrDefault() : null;
            if (SelectShip != null) GridOfVessels.ScrollIntoView(SelectShip);
        }

        ObservableCollection<IAttachedFile> GetShipFiles()
        {
            if (SelectShip == null) return null;
            return ShipManager.GetFilesForShip(SelectShip.GetEntity());
        }

        bool AddFileForShip(IAttachedFile file)
        {
            if (SelectShip == null) return false;
            return ShipManager.AddFileForShip(SelectShip.GetEntity(), file);
        }

        #region Handling Methods
        void cmdAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Ksanti         
            if (OpenEditVesselForm != null)
                OpenAddVesselForm(delegate (UI_Ship selectShip) { Load(selectShip); });
        }

        void cmdEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectShip == null) return;

            if (OpenEditVesselForm != null)
                OpenEditVesselForm(SelectShip.GetEntity(), delegate (UI_Ship selectShip) { Load(selectShip); } );      

        }

        void cmdView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectShip == null) return;
            if (OpenViewVesselForm != null) OpenViewVesselForm(SelectShip.GetEntity());         
        }

        void cmdDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectShip == null) return;
            if (BaseLib.AppManager.CommonInfo.Module_IsCompany ||
                BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany)
            {
                //если модуль компании, то решили скрывать судно, т.к. не хотим терять историю заявок
                //если модуль автономной компании, то решили тоже скрывать судно, чтобы было одинаково
                if (ShipManager.HideShip(SelectShip.GetEntity())) Load();
            }
            //if (BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany)
            //{
            //    //если модуль автономной компании, то решили оставить удаление судна
            //    if (ShipManager.DeleteShip(SelectShip)) Load();
            //}
        }

        void cmdFiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var perfomer = new Shared.Forms.AttachedFiles.AttachedFilePerformer(
                  new Forms.AttachedFiles.AttachedFilePerformer.FileLoaderMethod(GetShipFiles)
                , new Forms.AttachedFiles.AttachedFilePerformer.FileCreatorMethod(ShipManager.CreateNewFileForShip)
                , new Forms.AttachedFiles.AttachedFilePerformer.FileAddingMethod(AddFileForShip));

            var f = new Shared.Forms.AttachedFiles.AttachedFileListForm(perfomer, HasAccessOnModify);
            if (f.WasLoadedSuccessfully) f.ShowDialog();
        }

        #endregion

        #region Commands Availability
        void cmdEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectShip != null);
        }

        void cmdView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectShip != null);
        }
        #endregion

        private void BtnVesselTypes_Click(object sender, RoutedEventArgs e)
        {      
            var f = new Shared.Views.Vessel.VesselTypesView();
            f.DataContext = new Shared.ViewModels.Vessel.ShipTypesViewModel(Load);
            f.ShowDialog();
        }

        private void BtnAdditionalProperties_Click(object sender, RoutedEventArgs e)
        {
            var f = new Shared.Vessel.AdditionalPropertiesForm();
            f.ShowDialog();
        }

        private void BtnShipGroup_Click(object sender, RoutedEventArgs e)
        {
            var f = new Shared.Vessel.ShipGroup();
            f.DataContext = new Shared.ViewModels.Vessel.ShipGroupsViewModel(Load);
            f.ShowDialog();
        }
    }
}
