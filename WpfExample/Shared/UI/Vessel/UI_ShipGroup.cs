using ViewModelBaseSolutions.UIEntityHelper;
using DBServices.UsualEntity;

namespace Shared.UI.Vessel
{
    public class UI_ShipGroup : UI_CEntity<ShipGroup>
    {
        #region Properties
        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; RaisePropertyChanged(() => this.Name); }
        }

        #endregion

        #region Constructors and PrivateMethods
        public UI_ShipGroup() { }
        public UI_ShipGroup(int id): this(new ShipGroup(id)) { }
        public UI_ShipGroup(ShipGroup entity) : base(entity) { }

        #endregion

        
        /// <summary>
        /// Проверка есть ли суда данного типа
        /// </summary>
        public bool HasShips()
        {
            return _entity.HasShips();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name);
                }
                return null;
            }
        }

    }



}
