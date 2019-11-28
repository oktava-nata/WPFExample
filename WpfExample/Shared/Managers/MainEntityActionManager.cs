using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIMessager.Services.Message;
using System.Windows;
using Shared.Forms;

namespace Shared.Managers
{
    public static class MainEntityActionManager
    {
        public static bool AddEntity<TEntity>(TEntity entity) where TEntity : DBServices.IEntity
        {
            try
            {
                entity.AddComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Adding, exp); }
            return false;
        }

        public static bool SaveEntity<TEntity>(TEntity entity, out bool wasSave) where TEntity : DBServices.IEntity
        {
            wasSave = false;
            try
            {
                wasSave = entity.Save();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        public static bool SaveComplexEntity<TEntity>(TEntity entity, out bool wasSave) where TEntity : DBServices.IEntity
        {
            wasSave = false;
            try
            {
                wasSave = entity.SaveComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        public static bool DeleteEntity<TEntity>(TEntity entity, string confirmMsgText, bool showWaitingProgressForm = false)
            where TEntity : DBServices.IEntity
        {
            if (MsgConfirm.Show(confirmMsgText) == MessageBoxResult.No) return false;
            UIMessager.Forms.ProgressForm progressForm = (showWaitingProgressForm)
                ? new UIMessager.Forms.ProgressForm() : null;
            try
            {
                if (progressForm!=null) progressForm.Show();
                entity.DeleteWithImitator();
                if (progressForm != null) progressForm.Close();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            if (progressForm != null) progressForm.Close();
            return false;
        }
    }
}
