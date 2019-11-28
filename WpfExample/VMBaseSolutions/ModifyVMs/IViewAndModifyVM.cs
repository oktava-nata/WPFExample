using System;
using BaseLib.Services;

namespace VMBaseSolutions.ModifyVMs
{
    public interface IViewAndModifyVM<TViewModel>
    {
        ActionMode Action { get; }
        void Initialize_ActionView(TViewModel targetVMForView);
        void Initialize_ActionAdd();
        void Initialize_ActionEdit(TViewModel sourceVM);

        Action<TViewModel> OnModifyCommandExecute { get; set; }
        Action<bool> OnFinishModify { get; set; }
    }
}
