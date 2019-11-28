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
using System.Collections.ObjectModel;
using Resources.Services;
using DBServices.UsualEntity;

namespace Shared.Forms.AttachedFiles
{
    /// <summary>
    /// Логика взаимодействия для AttachedFileListForm.xaml
    /// </summary>
    public partial class AttachedFileListForm : Window
    {
        #region Properties
        AttachedFilePerformer Performer;

        public static DependencyProperty FileListProperty;
        public ObservableCollection<IAttachedFile> FileList
        {
            get { return (ObservableCollection<IAttachedFile>)GetValue(FileListProperty); }
            set { SetValue(FileListProperty, value); }
        }

        public static DependencyProperty SelectedFileProperty;
        public IAttachedFile SelectedFile
        {
            get { return (IAttachedFile)GetValue(SelectedFileProperty); }
            set { SetValue(SelectedFileProperty, value); }
        }

        public bool WasLoadedSuccessfully { get; private set; }

        bool HasAccessOnModify { get; set; }
        #endregion

        #region Constructors
        public AttachedFileListForm(AttachedFilePerformer performer, bool hasAccessOnModify)
        {
            InitializeComponent();
            HasAccessOnModify = hasAccessOnModify;
            Performer = performer;
            WasLoadedSuccessfully = LoadFiles();
        }

        static AttachedFileListForm()
        {
            FrameworkPropertyMetadata metadataSelectedFile = new FrameworkPropertyMetadata(null);
            SelectedFileProperty = DependencyProperty.Register("SelectedFile", typeof(IAttachedFile), typeof(AttachedFileListForm), metadataSelectedFile);

            FrameworkPropertyMetadata metadataFileList = new FrameworkPropertyMetadata(null);
            FileListProperty = DependencyProperty.Register("FileList", typeof(ObservableCollection<IAttachedFile>), typeof(AttachedFileListForm), metadataFileList);
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
            cmdDel.CanExecute += new CanExecuteRoutedEventHandler(cmdEdit_CanExecute);
            this.Menu.CommandBindings.Add(cmdDel);
            this.ContextMenu.CommandBindings.Add(cmdDel);

            CommandBinding cmdOpenFile = new CommandBinding(MenuCommands.OpenFile);
            cmdOpenFile.Executed += new ExecutedRoutedEventHandler(cmdOpenFile_Executed);
            cmdOpenFile.CanExecute += new CanExecuteRoutedEventHandler(cmdOpenFile_CanExecute);
            this.Menu.CommandBindings.Add(cmdOpenFile);
            this.ContextMenu.CommandBindings.Add(cmdOpenFile);

            CommandBinding cmdDownloadFile = new CommandBinding(MenuCommands.DownloadFile);
            cmdDownloadFile.Executed += new ExecutedRoutedEventHandler(cmdDownloadFile_Executed);
            cmdDownloadFile.CanExecute += new CanExecuteRoutedEventHandler(cmdOpenFile_CanExecute);
            this.Menu.CommandBindings.Add(cmdDownloadFile);
            this.ContextMenu.CommandBindings.Add(cmdDownloadFile);
        }

        void ClearCommands()
        {
            this.Menu.CommandBindings.Clear();
            this.ContextMenu.CommandBindings.Clear();
        }

        private void AttachedFileListForm_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCommands();
            ComponentsVisability();
        }

        private void AttachedFileListForm_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearCommands();
        }

        void ComponentsVisability()
        {
            BtnM_Add.Visibility =
                BtnM_Edit.Visibility = BtnMC_Edit.Visibility =
                 BtnM_Delete.Visibility = BtnMC_Delete.Visibility =
                 (HasAccessOnModify)
                 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region Loaders
        bool LoadFiles(IAttachedFile file = null)
        {
            if (file == null && SelectedFile != null) file = SelectedFile;
            FileList = Performer.LoadFiles();

            if (FileList == null) return false;
            Select(file);
            return true;
        }

        public void Select(IAttachedFile file)
        {
            if (file != null) Select(file.Id);
        }

        public void Select(int idFile)
        {
            SelectedFile = FileList.Where(i => i.Id == idFile).FirstOrDefault();
            if (SelectedFile != null) GridFiles.ScrollIntoView(SelectedFile);
        }
        #endregion

        #region Handling Event Methods
        void cmdOpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedFile == null) return;
            Shared.FileManager.OpenFile(SelectedFile);
        }

        void cmdDownloadFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedFile == null) return;
            Shared.FileManager.DownloadFile(SelectedFile);
        }

        void cmdAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var f = new EditAttachedFileForm(Performer, new EditAttachedFileForm.RefreshMethodAfteModify(LoadFiles));
            if(f.WasLoadedSuccessfully) f.ShowDialog();
        }

        void cmdEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedFile == null) return;
            var f = new EditAttachedFileForm(Performer, SelectedFile, new EditAttachedFileForm.RefreshMethodAfteModify(LoadFiles));
            if (f.WasLoadedSuccessfully) f.ShowDialog();
        }

        void cmdDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedFile == null) return;
            if (Performer.DeleteFile(SelectedFile))
                LoadFiles();
        }

        private void FilesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedFile == null) return;
            Shared.FileManager.OpenFile(SelectedFile);
        }

        #endregion

        #region Commands Availability

        void cmdOpenFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectedFile != null);
        }

        void cmdEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectedFile != null);
        }
        #endregion

    }
}
