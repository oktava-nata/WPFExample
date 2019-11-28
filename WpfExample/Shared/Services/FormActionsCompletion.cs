using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;
using UIMessager.Services.Message;
using System.Windows;
using BaseLib.Services;

namespace Shared.Services
{
    public static class FormActionsCompletion
    {
        public delegate bool OnComfirmAction();

        public static bool Complate(ActionMode action, OnComfirmAction DoAddingMethod, OnComfirmAction DoEditingMethod)
        {
            if (action == ActionMode.Adding)
            {
                switch (MsgWarning.Show(MsgWarning.BaseType.DoAdding))
                {
                    case MessageBoxResult.Cancel: return false;
                    case MessageBoxResult.Yes:  return DoAddingMethod();
                }
                return true;
            }
            if (action == ActionMode.Editing)
                switch (MsgWarning.Show(MsgWarning.BaseType.DoSaving))
                {
                    case MessageBoxResult.Cancel: return false;
                    case MessageBoxResult.Yes: return DoEditingMethod();
                }
            return true;
        }
    }
}
