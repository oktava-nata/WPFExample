using System.Collections.Generic;
using System.Threading.Tasks;
using VMBaseSolutions.ModifyVMs;
using VMBaseSolutions.VMEntities;

namespace VMBaseSolutions.CollectionVMs
{

    public abstract class CollectionWithCRUDCommandsAndViewModifyVM<TModify, TViewModel> : CollectionWithCRUDCommandsVM<TViewModel>
        where TModify : IViewAndModifyVM<TViewModel>, new()
        where TViewModel : VMBase
    {
        #region Properties  
        public virtual TModify ModifyAndViewVM
        {
            get { return _ModifyVM; }
            protected set { _ModifyVM = value; OnPropertyChanged(() => this.ModifyAndViewVM); }
        }
        TModify _ModifyVM;

        #endregion

        #region Constructor

        public CollectionWithCRUDCommandsAndViewModifyVM()
        {
            ModifyAndViewVM = CreateModifyVM();
            ModifyAndViewVM.OnModifyCommandExecute += ModifyAndViewVM_OnModifyCommandExecute;
            ModifyAndViewVM.OnFinishModify += ModifyAndViewVM_OnFinishModify;
        }
        #endregion

        void ModifyAndViewVM_OnModifyCommandExecute(TViewModel vm)
        {
            CollectionInitialize(vm);
        }

        void ModifyAndViewVM_OnFinishModify(bool changesHaveBeenMade)
        {
            ModifyAndViewVM.Initialize_ActionView(CurrentElement);
        }

        protected virtual TModify CreateModifyVM()
        {
            return new TModify();
        }

        protected override void CurrentElementChanged(bool isSelectedAfterCollectionInitialize)
        {
            base.CurrentElementChanged(isSelectedAfterCollectionInitialize);
            if (!isSelectedAfterCollectionInitialize && ModifyAndViewVM.Action != BaseLib.Services.ActionMode.Adding && ModifyAndViewVM.Action != BaseLib.Services.ActionMode.Editing && ModifyAndViewVM.Action != BaseLib.Services.ActionMode.Modification )
                ModifyAndViewVM.Initialize_ActionView(CurrentElement);
        }


        protected virtual void CollectionInitialize(TViewModel select = null)
        {
            TViewModel selectCurrency = select ?? CurrentElement;
            var result = UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingAction(LoadCollection);
            VMCollection = result.ReturnValue;

            if (VMCollection != null && selectCurrency != null) SelectElementInCollection(selectCurrency);
        }

        abstract protected ICollection<TViewModel> LoadCollection();


        protected override void InitializeCommands()
        {
            MenuCommandCRUDVM.InitializeAllCommands(OnAddCommandExecute, OnEditCommandExecute, OnDeleteCommandExecute);
        }

        abstract protected void OnDeleteCommandExecute();

        protected virtual void OnAddCommandExecute()
        {
            ModifyAndViewVM.Initialize_ActionAdd();
        }

        protected virtual void OnEditCommandExecute()
        {
            ModifyAndViewVM.Initialize_ActionEdit(CurrentElement);
        }
    }
}
