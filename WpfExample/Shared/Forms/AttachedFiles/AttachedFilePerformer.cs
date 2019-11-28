using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DBServices;
using System.Windows;
using DBServices.UsualEntity;
using UIMessager.Services.Message;

namespace Shared.Forms.AttachedFiles
{
    public class AttachedFilePerformer
    {
        public delegate ObservableCollection<IAttachedFile> FileLoaderMethod();
        public delegate IAttachedFile FileCreatorMethod();
        public delegate bool FileAddingMethod(IAttachedFile file);

        event FileLoaderMethod FileLoading;
        event FileCreatorMethod FileCreating;
        event FileAddingMethod FileAdding;

        public AttachedFilePerformer(FileLoaderMethod fileLoadingMethod, FileCreatorMethod fileCreatingMethod, FileAddingMethod fileAdding)
        {
            FileLoading = fileLoadingMethod;
            FileCreating = fileCreatingMethod;
            FileAdding = fileAdding;
        }

        internal ObservableCollection<IAttachedFile> LoadFiles()
        {
            if (FileLoading != null) return FileLoading();
            return null;
        }

        internal IAttachedFile ReloadFile(IAttachedFile file)
        {
            try
            {
                return file.ReloadFile();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        internal IAttachedFile CreateNewFile()
        {
            if (FileCreating != null) return FileCreating();
            return null;
        }


        internal bool AddFile(IAttachedFile file)
        {
            if (FileAdding != null) return FileAdding(file);
            return false;
        }

        internal bool SaveFile(IAttachedFile file, out bool WasSaved)
        {
            WasSaved = false;
            try
            {
                WasSaved = file.Save();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        internal bool DeleteFile(IAttachedFile file)
        {
            if (MsgConfirm.Show(Properties.Resources.mConfirmDelFile) == MessageBoxResult.No) return false;
            try
            {
                file.Delete();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            return false;
        }
    }
}
