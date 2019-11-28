using System;
using System.Threading.Tasks;
using BaseLib.Services;
using VMBaseSolutions.VMEntities;

namespace VMBaseSolutions.ModifyVMs
{
    public abstract class ModifyVM<TViewModel> : ModifyBaseVM<TViewModel>, IModifyVM<TViewModel>
     where TViewModel : VMBase, IVMHasId
    {
        protected TViewModel SourceVM { get; private set; }

        protected TPropertyManager<TViewModel> _VMPropertyManager { get; private set; }

        public ModifyVM(Action<TViewModel> onModifyViewModel = null, Action<bool> onFinishModify = null)
            : base(onModifyViewModel, onFinishModify)
        {
            _VMPropertyManager = TPropertyManagerDefine();
        }

        public virtual void Initialize_ActionEdit(TViewModel source)
        {
            SourceVM = source ?? throw new ArgumentNullException("Arg source is null!");
            base.Initialize_ActionEdit();
        }

        public async virtual Task Initialize_ActionEditAsync(TViewModel source)
        {
            SourceVM = source ?? throw new ArgumentNullException("Arg source is null!");
            await base.Initialize_ActionEditAsync();
        }

        protected override TViewModel GetTargetVMForAdding()
        {
            return CreateTViewModelForAdding();
        }

        protected override TViewModel GetTargetVMForEditing()
        {
            var result = UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingAction(LoadTViewModelForEditing);
            return result.ReturnValue;
        }

        protected async override Task<TViewModel> GetTargetVMForEditingAsync()
        {
            var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingActionAsync(LoadTViewModelForEditingAsync);
            return result.ReturnValue;
        }

        protected TViewModel LoadTViewModelForEditing()
        {
            if (SourceVM == null) throw new ArgumentNullException("SourceViewModel was no difined!");
            return GetTViewModelById(SourceVM.Id);
        }

        protected async Task<TViewModel> LoadTViewModelForEditingAsync()
        {
            if (SourceVM == null) throw new ArgumentNullException("SourceViewModel was no difined!");
            return await GetTViewModelByIdAsync(SourceVM.Id);
        }

        protected abstract TViewModel GetTViewModelById(int id);
        protected abstract Task<TViewModel> GetTViewModelByIdAsync(int id);

        protected abstract TViewModel CreateTViewModelForAdding();


        #region TModelChanged

        protected virtual TPropertyManager<TViewModel> TPropertyManagerDefine()
        {
            var manager = new TPropertyManager<TViewModel>();
            manager.DefineAllSimplePropertieForManage();
            return manager;
        }

        public virtual bool WasAddingTModelChanged()
        {
            TViewModel emptyVM = CreateTViewModelForAdding();
            if (ModifyTargetVM == null) return false;
            return !_VMPropertyManager.IsEqualByProperties(ModifyTargetVM, emptyVM);
        }

        public virtual bool WasEdittingTModelChanged()
        {
            if (ModifyTargetVM == null) return false;
            return !_VMPropertyManager.IsEqualByProperties(ModifyTargetVM, SourceVM);
        }
        #endregion

        #region Actions

        protected abstract bool AddTViewModel(TViewModel targetVM);
        protected abstract bool UpdateTViewModel(TViewModel targetVM);

        protected override bool Add()
        {
            var result = UIMessager.Services.Execeptions.ExceptionНandling.HandleAddAction(
                delegate ()
                {
                    return AddTViewModel(ModifyTargetVM);
                });

            return result.ReturnValue;
        }

        protected override bool Save(out bool changesHaveBeenMade)
        {
            var result = UIMessager.Services.Execeptions.ExceptionНandling.HandleSaveAction(
              delegate ()
              {
                  return UpdateTViewModel(ModifyTargetVM);
              });

            changesHaveBeenMade = result.ReturnValue;
            return result.IsSuccess;
        }
        #endregion
    }
}
