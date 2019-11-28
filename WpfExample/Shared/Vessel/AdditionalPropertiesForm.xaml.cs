using System.Windows;
using System.Windows.Input;
using Resources.Services;
using DBServices.UsualEntity;
using BaseLib.Services;

namespace Shared.Vessel
{
    /// <summary>
    /// Логика взаимодействия для AdditionalPropertiesForm.xaml
    /// </summary>
    public partial class AdditionalPropertiesForm : Window
    {
       #region Properties
        public static DependencyProperty ActionProperty;
        public ActionMode Action
        {
            get { return (ActionMode)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public bool WasLoadedSuccessfully { get; private set; }
        #endregion

        #region Constructors
        public AdditionalPropertiesForm()
        {
            InitializeComponent();
            WasLoadedSuccessfully = Load();
        }
        static AdditionalPropertiesForm()
        {
            FrameworkPropertyMetadata metadataAction = new FrameworkPropertyMetadata(ActionMode.Empty);
            ActionProperty = DependencyProperty.Register("Action", typeof(ActionMode),
                typeof(AdditionalPropertiesForm), metadataAction);


            FrameworkPropertyMetadata metadataSelectProperty = new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SelectShipPropertyProperty_OnChanged));
            SelectShipPropertyProperty = DependencyProperty.Register("SelectShipProperty", typeof(ShipProperty),
                typeof(AdditionalPropertiesForm), metadataSelectProperty);
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

            CommandBinding cmdDel = new CommandBinding(MenuCommands.Delete);
            cmdDel.Executed += new ExecutedRoutedEventHandler(cmdDel_Executed);
            cmdDel.CanExecute += new CanExecuteRoutedEventHandler(cmdDel_CanExecute);
            this.Menu.CommandBindings.Add(cmdDel);
            this.ContextMenu.CommandBindings.Add(cmdDel);
        }

        void ClearCommands()
        {
            this.Menu.CommandBindings.Clear();
            this.ContextMenu.CommandBindings.Clear();
        }

        private void AdditionalPropertiesForm_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCommands();
        }

        private void AdditionalPropertiesForm_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearCommands();
        }
        #endregion

        #region Handling Methods
        void cmdAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GetReadyForAdding();
        }

        void cmdEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectShipProperty == null) return;
            GetReadyForEditing();
        }

        void cmdDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectShipProperty == null) return;
            if (Delete()) Load();
        }

        bool AddingExecute()
        {
            if (Add())
            {
                Load(EditingShipProperty);
                return true;
            }
            return false;
        }
        bool SavingExecute()
        {
            bool WasSaved;
            if (Save(out WasSaved))
            {
                Load(EditingShipProperty);
                return true;
            }
            return false;
        }
        #endregion

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SavingExecute();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddingExecute();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowSelectShipProperty();
        }

        private void BtnAply_Click(object sender, RoutedEventArgs e)
        {
            if (AddingExecute()) GetReadyForAdding();
        }

        #region Commands Availability
        void cmdEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectShipProperty != null);
        }

        void cmdDel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectShipProperty != null);
        }
        #endregion

        static void SelectShipPropertyProperty_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((AdditionalPropertiesForm)obj).ShowSelectShipProperty();
        }
    }
}
