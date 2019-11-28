using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMBaseSolutions.AdditionalVMs;
using VMBaseSolutions.VMEntities;

namespace VMBaseSolutions.CollectionVMs
{

    public abstract class CollectionWithCRUDCommandsVM<T> : Telerik.Windows.Controls.ViewModelBase
        where T : VMBase
    {
        #region Properties  
        public MenuCommandCRUDVM<T> MenuCommandCRUDVM { get; set; }

        public ICollection<T> VMCollection
        {
            get { return _VMCollection; }
            protected set { _VMCollection = value; OnPropertyChanged(() => this.VMCollection); }
        }
        ICollection<T> _VMCollection;

        public virtual T CurrentElement
        {
            get { return _CurrentElement; }
            set
            {
                SetCurrentElement(value, isSelectedAfterCollectionInitialize: false);
            }
        }
        private T _CurrentElement;

        #endregion

        #region Constructor
        public CollectionWithCRUDCommandsVM()
        {
            MenuCommandCRUDVM = new MenuCommandCRUDVM<T>(delegate () { return CurrentElement; });
        }
        #endregion

        public virtual async Task InitAsync()
        {
            InitializeCommands();
            await VMDataInitializeAsync();
        }

        abstract protected void InitializeCommands();

        protected virtual async Task VMDataInitializeAsync()
        {
            await CollectionInitializeAsync();
        }

        protected virtual async Task CollectionInitializeAsync(T select = null)
        {
            T selectCurrency = select ?? CurrentElement;
            var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingActionAsync(LoadCollectionAsync);
            VMCollection = result.ReturnValue;

            if (VMCollection != null && selectCurrency != null) SelectElementInCollection(selectCurrency);
        }

        abstract protected Task<ICollection<T>> LoadCollectionAsync();

        protected void SetCurrentElement(T element, bool isSelectedAfterCollectionInitialize)
        {
            if (this._CurrentElement != element)
            {
                this._CurrentElement = element;
                this.OnPropertyChanged(() => this.CurrentElement);
                CurrentElementChanged(isSelectedAfterCollectionInitialize);
            }
        }

        protected virtual void CurrentElementChanged(bool isSelectedAfterCollectionInitialize)
        {
            MenuCommandCRUDVM.UpdateCanExecuteForEditViewDeleteCommands();
        }

        protected virtual bool SelectElementInCollection(T selectElement)
        {
            if (VMCollection == null) throw new Exception("Collection is not define!");
            if (selectElement == null)
            {
                SetCurrentElement(null, isSelectedAfterCollectionInitialize: true);
                return true;
            }
            else
            {
                var elementFromCollection = VMCollection.Where(i => selectElement.Equals(i)).FirstOrDefault();
                SetCurrentElement(elementFromCollection, isSelectedAfterCollectionInitialize: true);
                return elementFromCollection != null;
            }
        }
    }
}
