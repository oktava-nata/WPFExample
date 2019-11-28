using System;
using Telerik.Windows.Controls;

namespace VMBaseSolutions.AdditionalVMs
{

    public class MenuCommandCRUDVM<T> : Telerik.Windows.Controls.ViewModelBase
    {
        #region Properties
        #region Commands
        public DelegateCommand AddNewCommand { get; private set; }
        public DelegateCommand ViewCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        #endregion

        protected Func<T> GetCurrentEntityViewModel;

        Func<bool> CanAddEntityCommand;
        Func<bool> CanEditEntityCommand;
        Func<bool> CanViewEntityCommand;
        Func<bool> CanDeleteEntityCommand;


        #endregion

        #region Constructor
        public MenuCommandCRUDVM(Func<T> getCurrentEntityViewModel)
        {
            if (getCurrentEntityViewModel == null) throw new ArgumentNullException("Argument GetCurrentEntity is null!");
            GetCurrentEntityViewModel = getCurrentEntityViewModel;
        }
        #endregion

        public virtual void InitializeAllCommands
            (Action onAddCommandExecute
            , Action onEditCommandExecute
            , Action onDeleteCommandExecute
            , Action onViewCommandExecute = null
            , Func<bool> canAddCommand = null
            , Func<bool> canEditCommand = null
            , Func<bool> canViewCommand = null
            , Func<bool> canDeleteCommand = null)
        {
            if (onAddCommandExecute == null) throw new ArgumentNullException("Arg onAddCommandExecute is null!");
            if (onEditCommandExecute == null) throw new ArgumentNullException("Arg onEditCommandExecute is null!");
            if (onDeleteCommandExecute == null) throw new ArgumentNullException("Arg onDeleteCommandExecute is null!");
            InitializeAddCommand(onAddCommandExecute, canAddCommand);
            InitializeEditCommand(onEditCommandExecute, canEditCommand);
            InitializeDeleteCommand(onDeleteCommandExecute, canDeleteCommand);
            if (onViewCommandExecute != null) InitializeViewCommand(onViewCommandExecute, canViewCommand);
        }


        public virtual void InitializeAddCommand(Action onAddCommandExecute, Func<bool> canAddCommand = null)
        {
            CanAddEntityCommand = canAddCommand;
            AddNewCommand = CreateCommand(onAddCommandExecute, CanAdd);
        }

        public virtual void InitializeEditCommand(Action onEditCommandExecute, Func<bool> canEditCommand = null)
        {
            CanEditEntityCommand = canEditCommand;
            EditCommand = CreateCommand(onEditCommandExecute, CanEdit);
        }
        public virtual void InitializeViewCommand(Action onViewCommandExecute, Func<bool> canViewCommand = null)
        {
            CanViewEntityCommand = canViewCommand;
            ViewCommand = CreateCommand(onViewCommandExecute, CanView);
        }
        public virtual void InitializeDeleteCommand(Action onDeleteCommandExecute, Func<bool> canDeleteCommand = null)
        {
            CanDeleteEntityCommand = canDeleteCommand;
            DeleteCommand = CreateCommand(onDeleteCommandExecute, CanDelete);
        }

        protected virtual DelegateCommand CreateCommand(Action onCommandExecute, Func<bool> canCommand)
        {
            return new DelegateCommand(
                delegate (object o) { onCommandExecute(); },
                delegate (object o) { return canCommand(); });
        }

        public virtual void UpdateCanExecuteForEditViewDeleteCommands()
        {
            this.EditCommand?.InvalidateCanExecute();
            this.DeleteCommand?.InvalidateCanExecute();
            this.ViewCommand?.InvalidateCanExecute();
        }

        #region CanExecute
        protected virtual bool CanAdd()
        {
            return (CanAddEntityCommand != null) ? CanAddEntityCommand() : true;
        }

        protected virtual bool CanEdit()
        {
            if (GetCurrentEntityViewModel() == null) return false;
            return (CanEditEntityCommand != null) ? CanEditEntityCommand() : true;
        }

        protected virtual bool CanDelete()
        {
            if (GetCurrentEntityViewModel() == null) return false;
            return (CanDeleteEntityCommand != null) ? CanDeleteEntityCommand() : true;
        }

        protected virtual bool CanView()
        {
            if (GetCurrentEntityViewModel() == null) return false;
            return (CanViewEntityCommand != null) ? CanViewEntityCommand() : true;
        }
        #endregion


    }
}
