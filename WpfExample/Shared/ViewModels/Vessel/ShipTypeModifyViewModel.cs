using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelBaseSolutions.ModifyVM;
using Shared.UI.Vessel;

namespace Shared.ViewModels.Vessel
{
    class ShipTypeModifyViewModel : ViewAndModifyTViewModel<UI_ShipType>
    {
        #region Properties
        UI_ShipType _SourceShipType;

        #endregion

        public ShipTypeModifyViewModel(AfterSavingChanges onGroupChanged) :
            base(onModifyViewModel: onGroupChanged) { }

        public void Group_CurrentChanged(UI_ShipType shipType)
        {
            _SourceShipType = shipType;
            base.Initialize_ActionView();
        }

        #region Ready Methods

        protected override UI_ShipType GetTModelForAdding()
        {
            return new UI_ShipType();
        }

        protected override UI_ShipType GetTModelForEditing()
        {
            if (_SourceShipType == null) return null;
            return new Shared.ViewModels.Services.Vessel.ShipTypeService().GetByIdForEdit(_SourceShipType.Id);
        }

        protected override UI_ShipType GetTModelForViewing()
        {
            return _SourceShipType;
        }
        #endregion


        #region Method 
        protected override bool Add(UI_ShipType obj)
        {
            return new Shared.ViewModels.Services.Vessel.ShipTypeService().Add(obj);
        }

        protected override bool Save(UI_ShipType obj, out bool changesHaveBeenMade)
        {
            return new Shared.ViewModels.Services.Vessel.ShipTypeService().Save(obj, out changesHaveBeenMade);
        }

        #endregion


    }
}
