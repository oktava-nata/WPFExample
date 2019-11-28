using System;
using System.Threading.Tasks;
using BaseLib.Services;

namespace VMBaseSolutions.ModifyVMs
{
    public interface IModifyVM<TViewModel>
    {
        ActionMode Action { get; }
        TViewModel ModifyTargetVM { get; }
        void Initialize_ActionAdd();
        Task Initialize_ActionAddAsync();
        void Initialize_ActionEdit(TViewModel sourceVM);
        Task Initialize_ActionEditAsync(TViewModel sourceVM);
        void Deinitialize();
        bool AddModifyTModel();
        bool SaveModifyTModel();
        bool WasAddingTModelChanged();
        bool WasEdittingTModelChanged();

        Action<bool> OnFinishModify { get; set; }
        Action OnModifyTargetVMChanged { get; set; }
        Action<TViewModel> OnModifyCommandExecute { get; set; }
    }
}
