using System;
using BaseLib.Services;
using VMBaseSolutions.VMEntities;
using VMBaseSolutions.VMFactories;
using VMBaseSolutions.VMServices;

namespace VMBaseSolutions.ModifyVMs
{
    public class ViewAndModifyWithFactoryAndCUDServiceVM<TFactory, TCUDService, TViewModel, TEntity> : ModifyWithFactoryAndCUDServiceVM<TFactory, TCUDService, TViewModel, TEntity>, IViewAndModifyVM<TViewModel>
     where TFactory : IVMFactory<TViewModel>, new()
     where TCUDService : CUDVMService<TViewModel, TEntity>, new()
     where TViewModel : VMEntityBase<TEntity>
     where TEntity : Infrastructure.Domain.IAggregateRootEntity
    {
        #region Properties
        public TViewModel ViewOrModifyTargetVM
        {
            get { return (Action == ActionMode.Viewing) ? ViewTargetVM : base.ModifyTargetVM; }
        }

        public TViewModel ViewTargetVM
        {
            get { return _ViewTModel; }
            private set
            {
                _ViewTModel = value;
                OnPropertyChanged(() => this.ViewTargetVM);
                OnPropertyChanged(() => this.ViewOrModifyTargetVM);
            }
        }
        TViewModel _ViewTModel;
        #endregion

        public ViewAndModifyWithFactoryAndCUDServiceVM(Action<TViewModel> onModifyViewModel = null, Action<bool> onFinishModify = null)
            : base(onModifyViewModel, onFinishModify)
        {
            this.OnModifyTargetVMChanged += ViewAndModifyWithFactoryAndCUDServiceVM_OnModifyTargetVMChanged;
        }

        
        #region Initialize
        public virtual void Initialize_ActionView(TViewModel targetVMForView)
        {
            Deinitialize();
            Action = ActionMode.Viewing;
            ViewTargetVM = targetVMForView;
        }

        #endregion

        void ViewAndModifyWithFactoryAndCUDServiceVM_OnModifyTargetVMChanged()
        {
            OnPropertyChanged(() => this.ViewOrModifyTargetVM);
        }

    }
}
