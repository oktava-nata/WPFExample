using System.Collections.ObjectModel;
using Models.Budget;
using VMBaseSolutions.VMEntities;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    internal class BudgetItemViewModel : VMEntityBase<BudgetItem>, global::UI.UI_INameENRU
    {
        #region Properties
        public override int Id => _entity.Id;

        public string Code
        {
            get { return _entity.Code; }
            set { _entity.Code = value; OnPropertyChanged(() => this.Code); }
        }

        public string Name_RU
        {
            get { return _entity.Name_RU; }
            set
            {
                _entity.Name_RU = value; OnPropertyChanged(() => this.Name_RU);
                OnPropertyChanged(() => this.Name_EN);
            }
        }

        public string Name_EN
        {
            get { return _entity.Name_EN; }
            set
            {
                _entity.Name_EN = value; OnPropertyChanged(() => this.Name_EN);
                OnPropertyChanged(() => this.Name_RU);
            }
        }

        public int? IdParent
        {
            get { return _entity.IdParent; }
        }

        public int? IdShipOwner
        {
            get { return _entity.IdShipOwner; }
        }

        public string Description
        {
            get { return _entity.Description; }
            set { _entity.Description = value; OnPropertyChanged(() => this.Description); }
        }

        public bool IsGroupingItem
        {
            get { return _entity.IsGroupingItem; }
            set { _entity.IsGroupingItem = value; OnPropertyChanged(() => this.IsGroupingItem); }
        }

        public bool IsInBudgetPlanByDefault
        {
            get { return _entity.IsInBudgetPlanByDefault; }
            set { _entity.IsInBudgetPlanByDefault = value; OnPropertyChanged(() => this.IsInBudgetPlanByDefault); }
        }

        public string CodeAndName => string.Format("{0} {1}", Code, NameForView);

        //сделать зависимой от языка интерфейса
        public string NameForView
        {
            get { return global::UI.UI_NameENRUService.GetDisplayName(this); }
        }

        public BudgetItemViewModel ParentItem
        {
            get { return _ParentItem; }
            set
            {
                _ParentItem = value;
                _entity.SetParentItem((value != null) ? (int?)value.Id : null);
                OnPropertyChanged(() => this.ParentItem);
            }
        }
        BudgetItemViewModel _ParentItem;

        public ObservableCollection<BudgetItemViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; OnPropertyChanged(() => this.Items); }
        }
        ObservableCollection<BudgetItemViewModel> _Items;

        public ObservableCollection<BudgetItemViewModel> GroupItems
        {
            get { return _GroupItems; }
            set { _GroupItems = value; OnPropertyChanged(() => this.GroupItems); }
        }
        ObservableCollection<BudgetItemViewModel> _GroupItems;
        #endregion

        #region Constructors and PrivateMethods
        public BudgetItemViewModel() : base(new BudgetItem()) { }
        public BudgetItemViewModel(BudgetItem entity) : base(entity) { }

        public override void SetEntity(BudgetItem entity)
        {
            base.SetEntity(entity);

            if (_entity.ParentItem != null)
                _ParentItem = new BudgetItemViewModel(_entity.ParentItem);
        }
        #endregion



        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Code": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Code);
                    case "Name_RU":
                        {
                            string name_RU_valid = MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_RU);
                            if (name_RU_valid == null)
                                return null;
                            else
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmptyRUAndEN(Name_RU, Name_EN);
                        }
                    case "Name_EN":
                        {
                            string name_EN_valid = MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_EN);
                            if (name_EN_valid == null)
                                return null;
                            else
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmptyRUAndEN(Name_RU, Name_EN);
                        }
                }
                return null;
            }
        }

        public override string ToString()
        {
            return CodeAndName;
        }
    }
}