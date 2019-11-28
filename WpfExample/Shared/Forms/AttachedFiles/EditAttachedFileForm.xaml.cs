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
using DBServices;
using Resources;
using Resources.Validators;
using UIMessager.Services.Message;
using DBServices.UsualEntity;
using BaseLib.Services;

namespace Shared.Forms.AttachedFiles
{
    /// <summary>
    /// Логика взаимодействия для EditAttachedFileForm.xaml
    /// </summary>
    public partial class EditAttachedFileForm : Window
    {
        #region Properties
        AttachedFilePerformer Performer;

        public delegate bool RefreshMethodAfteModify(IAttachedFile File = null);
        event RefreshMethodAfteModify OnModify;

        public static DependencyProperty ActionProperty;
        public ActionMode Action
        {
            get { return (ActionMode)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static DependencyProperty CurrentFileProperty;
        public IAttachedFile CurrentFile
        {
            get { return (IAttachedFile)GetValue(CurrentFileProperty); }
            set { SetValue(CurrentFileProperty, value); }
        }

        public bool WasLoadedSuccessfully { get; private set; }

        /// <summary>
        /// Валидаторы для загрузки файла
        /// </summary>
        ManagedValidator validatorForFile;
        #endregion

        #region Constructors
        EditAttachedFileForm(RefreshMethodAfteModify refreshMethodAfteModify)
        {
            InitializeComponent();
            errorFileBorder.DataContext = validatorForFile = new ManagedValidator(HasNotFileUploaded);
            OnModify = refreshMethodAfteModify;
        }

        public EditAttachedFileForm(AttachedFilePerformer performer, RefreshMethodAfteModify refreshMethodAfteModify)
            : this(refreshMethodAfteModify)
        {
            Performer = performer;
            WasLoadedSuccessfully = GetReadyForAddingTo();
            Action = ActionMode.Adding;
        }

        public EditAttachedFileForm(AttachedFilePerformer performer, IAttachedFile file, RefreshMethodAfteModify refreshMethodAfteModify)
            : this(refreshMethodAfteModify)
        {
            Performer = performer;
            WasLoadedSuccessfully = GetReadyForEditing(file);
            Action = ActionMode.Editing;
        }

        static EditAttachedFileForm()
        {
            FrameworkPropertyMetadata metadataAction = new FrameworkPropertyMetadata(ActionMode.Viewing);
            ActionProperty = DependencyProperty.Register("Action", typeof(ActionMode), typeof(EditAttachedFileForm), metadataAction);

            FrameworkPropertyMetadata metadataCurrentFile = new FrameworkPropertyMetadata(null);
            CurrentFileProperty = DependencyProperty.Register("CurrentFile", typeof(IAttachedFile), typeof(EditAttachedFileForm), metadataCurrentFile);
        }
        #endregion

        #region Preparation for actions methods
        bool GetReadyForAddingTo()
        {
            CurrentFile = Performer.CreateNewFile();

            validatorForFile.ClearError();
            txtFilePath.Clear();
            return true;
        }

        bool GetReadyForEditing(IAttachedFile file)
        {
            CurrentFile = Performer.ReloadFile(file);
            return (CurrentFile != null);
        }

        bool HasNotFileUploaded(out string errorText)
        {
            errorText = null;
            if (CurrentFile.IsFileDataUploaded) return false;
            errorText = global::Resources.Properties.Resources.txtSelectFile;
            return true;
        }
        #endregion

        #region Btn_Clicks
        private void BtnBrowser_Click(object sender, RoutedEventArgs e)
        {
            string selectFile = Shared.FileManager.BrowserAndThenUploadSelectFile(CurrentFile);
            if (selectFile != null) txtFilePath.Text = selectFile;
        }

        private void BtnAply_Click(object sender, RoutedEventArgs e)
        {
            if (AddingExecute()) GetReadyForAddingTo();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddingExecute()) Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SavingExecute()) Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Handling Methods
        bool AddingExecute()
        {
            if (Add())
            {
                if (OnModify != null) OnModify(CurrentFile);
                return true;
            }
            return false;
        }

        bool SavingExecute()
        {
            bool WasSaved;
            if (Save(out WasSaved))
            {
                if (WasSaved && OnModify != null) OnModify(CurrentFile);
                return true;
            }
            return false;
        }

        #endregion

        #region Methods
        bool Save(out bool WasSaved)
        {
            WasSaved = false;
            if (ValidateHelper.HasErrors(EditFields, false))
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return false;
            }
            return Performer.SaveFile(CurrentFile, out WasSaved);
        }

        bool Add()
        {
            if (validatorForFile.Validate() | ValidateHelper.HasErrors(EditFields, true))
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return false;
            }
            return Performer.AddFile(CurrentFile); 
        }

        public bool CheckFinishUserWork()
        {
            BtnCancel.Focus();
            if (Action == ActionMode.Adding && CurrentFile.Id <= 0
                || Action == ActionMode.Editing && CurrentFile != null && (CurrentFile.WasPropertiesValuesChanged || ValidateHelper.HasErrors(EditFields, false)))
                return !Shared.Services.FormActionsCompletion.Complate
                            (Action
                            , new Shared.Services.FormActionsCompletion.OnComfirmAction(AddingExecute)
                            , new Shared.Services.FormActionsCompletion.OnComfirmAction(SavingExecute));
            return false;
        }
        #endregion

        private void EditAttFileForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = CheckFinishUserWork();
        }

    }
}
