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
using Resources;
using Resources.Services;
using BaseLib.Services;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для ButtonsPanel.xaml
    /// </summary>
    public partial class ButtonsPanel : UserControl
    {
        public delegate void OnClick();
        public event OnClick OnSave;
        public event OnClick OnApply;
        public event OnClick OnAdd;
        public event OnClick OnCancel;
        public event OnClick OnClose;
        public event OnClick OnSelect;

        public delegate bool CanClick();
        public event CanClick CanSave;        
        public event CanClick CanAdd;        
        public event CanClick CanSelect;   

        public static DependencyProperty ActionProperty;
        public ActionMode Action
        {
            get { return (ActionMode)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static DependencyProperty ShowStandartBackgroundProperty;
        public bool ShowStandartBackground
        {
            get { return (bool)GetValue(ShowStandartBackgroundProperty); }
            set { SetValue(ShowStandartBackgroundProperty, value); }
        }

        public void CanelButtonSetFocus()
        {
            CancelBtn.Focus();
        }

        #region Constructors And Initializations
        public ButtonsPanel()
        {
            InitializeComponent();
        }

        static ButtonsPanel()
        {
            FrameworkPropertyMetadata metadataAction = new FrameworkPropertyMetadata(ActionMode.None);
            ActionProperty = DependencyProperty.Register("Action", typeof(ActionMode), typeof(ButtonsPanel), metadataAction);

            FrameworkPropertyMetadata metadataShowStandartBackground = new FrameworkPropertyMetadata(true);
            ShowStandartBackgroundProperty = DependencyProperty.Register("ShowStandartBackground", typeof(bool), typeof(ButtonsPanel), metadataShowStandartBackground);
        }

        private void buttonPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (OnAdd == null) gAddBtn.Visibility = System.Windows.Visibility.Collapsed;
            if (OnSave == null) gSaveBtn.Visibility = System.Windows.Visibility.Collapsed;
            if (OnApply == null) gApplyBtn.Visibility = System.Windows.Visibility.Collapsed;
            if (OnCancel == null) gCancelBtn.Visibility = System.Windows.Visibility.Collapsed;
            if (OnClose == null) gCloseBtn.Visibility = System.Windows.Visibility.Collapsed;
            if (OnSelect == null) gSelectBtn.Visibility = System.Windows.Visibility.Collapsed;
            InitializeCommands();
        }

        private void buttonPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearCommands();
        }

        void InitializeCommands()
        {
            CommandBinding cmdSelect = new CommandBinding(MenuCommands.Select);
            cmdSelect.Executed += new ExecutedRoutedEventHandler(cmdSelect_Executed);
            cmdSelect.CanExecute += new CanExecuteRoutedEventHandler(cmdSelect_CanExecute);
            this.stackPanelForButtons.CommandBindings.Add(cmdSelect);

            CommandBinding cmdAdd = new CommandBinding(MenuCommands.Add);
            cmdAdd.Executed += new ExecutedRoutedEventHandler(cmdAdd_Executed);
            cmdAdd.CanExecute += new CanExecuteRoutedEventHandler(cmdAdd_CanExecute);
            this.stackPanelForButtons.CommandBindings.Add(cmdAdd);

            CommandBinding cmdSave = new CommandBinding(MenuCommands.Save);
            cmdSave.Executed += new ExecutedRoutedEventHandler(cmdSave_Executed);
            cmdSave.CanExecute += new CanExecuteRoutedEventHandler(cmdSave_CanExecute);
            this.stackPanelForButtons.CommandBindings.Add(cmdSave);           
        }

        void ClearCommands()
        {
            this.stackPanelForButtons.CommandBindings.Clear();           
        }  
        #endregion

        #region BtnClicks
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnSave != null)
                OnSave();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnAdd != null)
                OnAdd();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnApply != null)
                OnApply();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancel != null)
                OnCancel();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnClose != null)
                OnClose();
        }
        #endregion

        #region Handling Methods
        void cmdSelect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (OnSelect != null)
                OnSelect();
        }

        void cmdAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (OnAdd != null)
                OnAdd();
        }

        void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (OnSave != null)
                OnSave();
        }
        #endregion

        #region Commands Availability
        void cmdSelect_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (CanSelect == null) || CanSelect();
        }

        void cmdAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (CanAdd == null) || CanAdd();
        }

        void cmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (CanSave == null) || CanSave();
        }
        #endregion
    }
}
