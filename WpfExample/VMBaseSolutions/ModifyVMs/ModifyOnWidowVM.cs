using BaseLib.Services;
using VMBaseSolutions.VMEntities;
using System;
using System.Threading.Tasks;

namespace VMBaseSolutions.ModifyVMs
{
    public class ModifyOnWidowVM<TModifyViewModel, TViewModel> : Telerik.Windows.Controls.ViewModelBase
      where TModifyViewModel : IModifyVM<TViewModel>
    {
        #region Properties
        public TModifyViewModel ModifyVM { get; private set; }

        public bool IsCloseView
        {
            get { return _IsCloseView; }
            set { _IsCloseView = value; OnPropertyChanged(() => this.IsCloseView); }
        }
        bool _IsCloseView;

        public MVVMHelper.WindowExtension.ClosingWindowEventHandler OnClosingWindow { get; private set; }

        public bool IsFocusedToFinishEnter
        {
            get { return _IsFocusedToFinishEnter; }
            set { _IsFocusedToFinishEnter = value; OnPropertyChanged(() => this.IsFocusedToFinishEnter); }
        }
        bool _IsFocusedToFinishEnter;

        bool _wasModifyDone;
        #endregion

        #region Constructors
        public ModifyOnWidowVM(TModifyViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg TModifyViewModel is null!");
            ModifyVM = vm;
            ModifyVM.OnFinishModify += ModifyOnWindowTViewModel_OnFinishModify;
            this.OnClosingWindow += new MVVMHelper.WindowExtension.ClosingWindowEventHandler(ModifyOnWindowTViewModel_OnClosingWindow);
        }
        #endregion

        public void Initialize_ActionAdd()
        {
            ModifyVM.Initialize_ActionAdd();
        }

        public async Task Initialize_ActionAddAsync()
        {
            await ModifyVM.Initialize_ActionAddAsync();
        }

        public void Initialize_ActionEdit(TViewModel sourceVM)
        {
            ModifyVM.Initialize_ActionEdit(sourceVM);
        }

        public async Task Initialize_ActionEditAsync(TViewModel sourceVM)
        {
            await ModifyVM.Initialize_ActionEditAsync(sourceVM);
        }

        protected virtual void ModifyOnWindowTViewModel_OnFinishModify(bool changesHaveBeenMadeOrCanceled)
        {
            _wasModifyDone = changesHaveBeenMadeOrCanceled;
            IsCloseView = true;
        }

        #region ClosingWindow

        bool ModifyOnWindowTViewModel_OnClosingWindow()
        {
            if (_wasModifyDone) return true;
            IsFocusedToFinishEnter = true;
            if (ModifyVM.Action == ActionMode.Adding && ModifyVM.WasAddingTModelChanged()
               || ModifyVM.Action == ActionMode.Editing && ModifyVM.WasEdittingTModelChanged())
                return ComplateAction();
            return true;
        }

        protected virtual bool ComplateAction()
        {
            return Helpers.WindowActionsCompletionHelper.Complate(ModifyVM.Action, ModifyVM.AddModifyTModel, ModifyVM.SaveModifyTModel);
        }

        #endregion
    }
}
