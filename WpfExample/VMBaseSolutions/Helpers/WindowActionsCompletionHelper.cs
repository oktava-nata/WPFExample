using System;
using System.Threading.Tasks;
using System.Windows;
using BaseLib.Services;
using UIMessager.Services.Message;

namespace VMBaseSolutions.Helpers
{
    public static class WindowActionsCompletionHelper
    {
        public static bool Complate(ActionMode action, Func<bool> DoAddingMethod, Func<bool> DoEditingMethod)
        {
            if (action == ActionMode.Adding)
                switch (MsgWarning.Show(MsgWarning.BaseType.DoAdding))
                {
                    case MessageBoxResult.Cancel: return false;
                    case MessageBoxResult.Yes: return DoAddingMethod();
                }
            else if (action == ActionMode.Editing)
                switch (MsgWarning.Show(MsgWarning.BaseType.DoSaving))
                {
                    case MessageBoxResult.Cancel: return false;
                    case MessageBoxResult.Yes: return DoEditingMethod();
                }
            return true;
        }

        public static bool Complate(Func<bool> doActionMethod, string msgWarningText)
        {
            switch (MsgWarning.Show(msgWarningText))
            {
                case MessageBoxResult.Cancel: return false;
                case MessageBoxResult.Yes: return doActionMethod();
            }
            return true;
        }
    }
}
