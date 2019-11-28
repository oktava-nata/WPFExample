using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelBaseSolutions.ModifyVM;
using Shared.UI.Vessel;

namespace Shared.ViewModels.Vessel
{
    class ShipGroupModifyViewModel : ViewAndModifyTViewModel<UI_ShipGroup>
    {
        #region Properties
        UI_ShipGroup _SourceTGroup;

        #endregion

        public ShipGroupModifyViewModel(AfterSavingChanges onGroupChanged) :
            base(onModifyViewModel: onGroupChanged) { }

        public void Group_CurrentChanged(UI_ShipGroup tGroup)
        {
            _SourceTGroup = tGroup;
            base.Initialize_ActionView();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }

        #region Ready Methods

        protected override UI_ShipGroup GetTModelForAdding()
        {
            return new UI_ShipGroup();
        }

        protected override UI_ShipGroup GetTModelForEditing()
        {
            if (_SourceTGroup == null) return null;
            return new Shared.ViewModels.Services.Vessel.ShipGroupService().GetByIdForEdit(_SourceTGroup.Id);
        }

        protected override UI_ShipGroup GetTModelForViewing()
        {
            return _SourceTGroup;
        }
        #endregion


        #region Method
        protected override bool Add(UI_ShipGroup obj)
        {
            return new Shared.ViewModels.Services.Vessel.ShipGroupService().Add(obj);
        }

        protected override bool Save(UI_ShipGroup obj, out bool changesHaveBeenMade)
        {
            return new Shared.ViewModels.Services.Vessel.ShipGroupService().Save(obj, out changesHaveBeenMade);
        }

        #endregion


    }
}
