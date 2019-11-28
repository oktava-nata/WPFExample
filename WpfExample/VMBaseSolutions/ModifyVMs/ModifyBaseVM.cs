using System;
using System.Threading.Tasks;
using BaseLib.Services;
using Telerik.Windows.Controls;
using UIMessager.Services.Message;
using VMBaseSolutions.VMEntities;

namespace VMBaseSolutions.ModifyVMs
{
    public abstract class ModifyBaseVM<TViewModel> : Telerik.Windows.Controls.ViewModelBase
        where TViewModel : VMBase
    {
        // метод, срабатывающий после каждого сохранения/добавления 
        //(например, после каждого выполнения команды "Применить")
        public Action<TViewModel> OnModifyCommandExecute { get; set; }

        //метод, срабатывающий после окончания редактирования/добавления
        // (например, после выполнения команды "Отмена")
        public Action<bool> OnFinishModify { get; set; }

        public Action OnModifyTargetVMChanged { get; set; }

        #region Commands
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand AplyCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region Properties
        public ActionMode Action
        {
            get { return _Action; }
            protected set { _Action = value; OnPropertyChanged(() => this.Action); }
        }
        ActionMode _Action;

        public virtual TViewModel ModifyTargetVM
        {
            get { return _TargetVM; }
            protected set
            {
                _TargetVM = value;
                OnPropertyChanged(() => this.ModifyTargetVM);
                OnModifyTargetVMChanged?.Invoke();
            }
        }
        TViewModel _TargetVM;

        public bool IsFocusedToStartModify
        {
            get { return _IsFocusedToStartModify; }
            set { _IsFocusedToStartModify = value; OnPropertyChanged(() => this.IsFocusedToStartModify); }
        }
        bool _IsFocusedToStartModify;

        bool ChangesHaveBeenMade { get; set; }

        #endregion

        #region Constructors
        public ModifyBaseVM()
        {
            AplyCommand = new DelegateCommand(OnAplyCommandExecute);
            AddCommand = new DelegateCommand(OnAddCommandExecute);
            SaveCommand = new DelegateCommand(OnSaveCommandExecute);
            CancelCommand = new DelegateCommand(OnCancelCommandExecute);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="onModifyViewModel">метод, срабатывающий после каждого сохранения/добавления 
        /// (например, после каждого выполнения команды "Применить")</param>
        /// <param name="onFinishModify">метод, срабатывающий после окончания редактирования/добавления
        /// (например, после выполнения команды "Отмена")</param>
        public ModifyBaseVM(Action<TViewModel> onModifyViewModel = null, Action<bool> onFinishModify = null)
            : this()
        {
            OnModifyCommandExecute = onModifyViewModel;
            OnFinishModify = onFinishModify;
        }

        #endregion

        #region Initialize
        public virtual void Initialize_ActionEdit()
        {
            Action = ActionMode.Editing;
            GetReadyForModify();
        }

        public async virtual Task Initialize_ActionEditAsync()
        {
            Action = ActionMode.Editing;
            await GetReadyForModifyAsync();
        }

        public virtual void Initialize_ActionAdd()
        {
            Action = ActionMode.Adding;
            GetReadyForModify();
        }

        public async virtual Task Initialize_ActionAddAsync()
        {
            Action = ActionMode.Adding;
            await GetReadyForModifyAsync();
        }

        public virtual void Deinitialize()
        {
            Action = ActionMode.None;
            ModifyTargetVM = null;
            ChangesHaveBeenMade = false;
        }
        #endregion

        protected abstract TViewModel GetTargetVMForAdding();
        protected abstract TViewModel GetTargetVMForEditing();

        protected abstract Task<TViewModel> GetTargetVMForAddingAsync();
        protected abstract Task<TViewModel> GetTargetVMForEditingAsync();

        protected abstract bool Add();
        protected abstract bool Save(out bool changesHaveBeenMade);


        void GetReadyForModify()
        {
            if (Action == ActionMode.Adding) ModifyTargetVM = GetTargetVMForAdding();
            else ModifyTargetVM = GetTargetVMForEditing();
            IsFocusedToStartModify = true;
        }

        async Task GetReadyForModifyAsync()
        {
            if (Action == ActionMode.Adding) ModifyTargetVM = await GetTargetVMForAddingAsync();
            else ModifyTargetVM = await GetTargetVMForEditingAsync();
            IsFocusedToStartModify = true;
        }

        protected virtual bool IsModifyTargetValid(out string errorMsg)
        {
            if (!ModifyTargetVM.IsValid())
            {
                errorMsg = global::UIMessager.Properties.Resources.mErIncorrectInputDate;
                return false;
            }
            else
            {
                errorMsg = null;
                return true;
            }
        }

        #region Methods
        protected bool Validate()
        {
            string errorMsg;
            bool result = IsModifyTargetValid(out errorMsg);
            if (!result && !string.IsNullOrWhiteSpace(errorMsg))
                MsgError.Show(errorMsg);
            return result;
        }

        public bool AddModifyTModel()
        {
            if (!Validate()) return false;
            if (Add())
            {
                ChangesHaveBeenMade = true;
                OnModifyCommandExecute?.Invoke(ModifyTargetVM);
                return true;
            }
            return false;
        }

        public bool SaveModifyTModel()
        {
            if (!Validate()) return false;
            bool changesHaveBeenMade;
            if (Save(out changesHaveBeenMade))
            {
                ChangesHaveBeenMade = changesHaveBeenMade;
                OnModifyCommandExecute?.Invoke(ModifyTargetVM);
                return true;
            }
            return false;
        }

        #endregion

        #region Actions
        private void OnAdd()
        {
            var result = AddModifyTModel();
            if (result) FinishModify(ChangesHaveBeenMade);
        }

        private void OnAply()
        {
            var result = AddModifyTModel();
            if (result)
                Initialize_ActionAdd();
        }

        private void OnSave()
        {
            if (Action == ActionMode.Adding) OnAdd();
            else
            {
                var result = SaveModifyTModel();
                if (result) FinishModify(ChangesHaveBeenMade);
            }
        }

        private void OnCancel()
        {
            FinishModify(true);
            // OnFinishModify?.Invoke(true); //!!!передаем true при Отмене    
            // Deinitialize();
        }
        #endregion

        void FinishModify(bool changesHaveBeenMade)
        {
            Deinitialize();
            OnFinishModify?.Invoke(changesHaveBeenMade);
        }

        #region Execute Commands
        private void OnAddCommandExecute(object obj)
        {
            OnAdd();
        }

        private void OnAplyCommandExecute(object obj)
        {
            OnAply();
        }

        private void OnSaveCommandExecute(object obj)
        {
            OnSave();
        }

        private void OnCancelCommandExecute(object obj)
        {
            OnCancel();
        }
        #endregion

    }
}
