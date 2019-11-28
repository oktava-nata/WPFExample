using System;
using UIMessager.Services.Message;
using Microsoft.Win32;
using DBServices.UsualEntity;

namespace Shared
{
    /// <summary>
    /// [2016-10-07 Nata] !!УСТАРЕЛ!! т.к. при работе с формами мы больше не используем напрямую сущности из DBServices.
    /// Рекомендуется использовать методы из библиотеки FileSystemWorker (Utils)
    /// 
    /// Менеджер для загрузки, открытия и скачивания файлов из БД (DBServices.UsualEntity.IFile)
    /// </summary>
    public static class FileManager
    {
        public static string BrowserAndThenUploadSelectFile(IFile file, string defaultFileExtension = null, string fileType = null)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            
            //Olga 8/09/16
            if (defaultFileExtension != null && fileType != null)
            {
                dlg.DefaultExt = defaultFileExtension;
                fileType = string.Format("{0} ", fileType);
                dlg.Filter = string.Format("{0}(*.{1})|*{1}", fileType, dlg.DefaultExt);
            }
            
            if (dlg.ShowDialog() == true)
                if (UploadFile(dlg.FileName, file))
                    return dlg.FileName;
            return null;
        }
        
        public static bool UploadFile(string filePath, IFile file, bool flag1C = false)
        {
            if (flag1C)
            {
                return UploadFileNoMsg(filePath, file);
            }
            else return UploadFileMsg(filePath, file);
        }

        public static bool UploadFileMsg(string filePath, IFile file)
        {
            long FileSize;
            try
            {
                byte[] fileData = BaseLib.FileManager.ReadFileBytes(filePath, out FileSize, ModuleSettings.MaxLengthFileToRead);
                if (fileData != null)
                {
                    file.FileData = fileData;
                    file.FileSize = (int)FileSize;
                    file.FileExtension = System.IO.Path.GetExtension(filePath);
                    if (file.FileName == null) file.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    return true;
                }
                else if (FileSize > ModuleSettings.MaxLengthFileToRead)
                    MsgInformation.Show(string.Format(global::Resources.Properties.Resources.mFileSizeTooLarge, ModuleSettings.MaxLengthFileToRead / 1024 / 1024));
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        public static bool UploadFileNoMsg(string filePath, IFile file)
        {
            long FileSize;
            byte[] fileData = BaseLib.FileManager.ReadFileBytes(filePath, out FileSize, ModuleSettings.MaxLengthFileToRead);
            if (fileData != null)
            {
                file.FileData = fileData;
                file.FileSize = (int)FileSize;
                file.FileExtension = System.IO.Path.GetExtension(filePath);
                // Ksanti 
                // проверить, как это будет работать при загрузке файлов 1С
                if (file.FileName == null) file.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                return true;
            }
            else if (FileSize > ModuleSettings.MaxLengthFileToRead)
                return false;
            return false;
        }

        public static void OpenFile(IFile File)
        {
            try
            {
                if (!File.IsFileDateLoaded) File.LoadFileData();
                if (File.FileData == null)
                {
                    MsgInformation.Show(global::Resources.Properties.Resources.mFileNotLoaded);
                    return;
                }
                string tmpFile = BaseLib.TEMP_MARINER.CreateTmpFile(File.FileFullName, File.FileData, "FilesFromDB");
                BaseLib.FileManager.OpenTMPFile(tmpFile);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.FileOpening, exp); }
        }

        public static void DownloadFile(IFile File)
        {
            string fileName = BaseLib.FileDialogManager.ChooseFileForSaving(File.FileExtension, File.FileName);
            if (string.IsNullOrEmpty(fileName)) return;

            if (createAndsaveCopyOfFile(File, fileName))
                MsgInformation.Show(string.Format(global::Resources.Properties.Resources.mFileDownloadOk, fileName));
        }

        static bool createAndsaveCopyOfFile(IFile File, string path)
        {
            try
            {
                if (!File.IsFileDateLoaded) File.LoadFileData();
                if (File.FileData == null)
                {
                    MsgInformation.Show(global::Resources.Properties.Resources.mFileNotLoaded);
                    return false;
                }
                BaseLib.FileManager.CreateFile(path, File.FileData);
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

    }
}
