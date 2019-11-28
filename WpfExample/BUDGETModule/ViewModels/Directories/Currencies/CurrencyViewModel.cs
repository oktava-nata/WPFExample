using Models.Budget;
using VMBaseSolutions.VMEntities;

namespace BUDGETModule.ViewModels.Directories.Currencies
{
    internal class CurrencyViewModel : VMEntityBase<Currency>
    {
        #region Properties
        public override int Id => _entity.Id;

        public string Code
        {
            get { return _entity.Code; }
            set { _entity.Code = value; OnPropertyChanged(() => this.Code); }
        }

        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; OnPropertyChanged(() => this.Name); }
        }

        public bool VAT
        {
            get { return _entity.VAT; }
            set { _entity.VAT = value; OnPropertyChanged(() => this.VAT); }
        }     
        #endregion

        #region Constructors and PrivateMethods
        public CurrencyViewModel() : base(new Currency()) { }
        public CurrencyViewModel(Currency entity) : base(entity) { }     
        #endregion



        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Code": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Code);
                    case "Name": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name);                      
                }
                return null;
            }
        }

        public override string ToString()
        {
            return Code;
        }
    }
}