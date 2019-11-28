using Shared.UI.Countries;
using System.ComponentModel;
using ViewModelBaseSolutions.ModifyVM;

namespace Shared.ViewModels.Countries
{
    public class CountryModifyViewModel : ViewAndModifyTViewModel<UI_Country>       
    {
        #region Properties      
        UI_Country _SourceCountry;

        #endregion

        public void Country_CurrentChanged(UI_Country changedCountry)
        {
            _SourceCountry = changedCountry;
            base.Initialize_ActionView();
        }
       
        #region Ready Methods

        protected override UI_Country GetTModelForAdding()
        {
            return new Shared.Services.CountryService().CreateNew();
        }

        protected override UI_Country GetTModelForEditing()
        {
            if (_SourceCountry == null) return null;           
            return new Shared.Services.CountryService().GetById(_SourceCountry.Id);
        }

        protected override UI_Country GetTModelForViewing()
        {
            return _SourceCountry;
        }
        #endregion

        #region Method
        protected override bool Add(UI_Country obj)
        {
            return new Shared.Services.CountryService().Add(obj);
        }

        protected override bool Save(UI_Country obj, out bool changesHaveBeenMade)
        {
            return new Shared.Services.CountryService().Save(obj, out changesHaveBeenMade);
        }

        #endregion
       

        public CountryModifyViewModel(AfterSavingChanges onTypeChanged) :
            base(onModifyViewModel: onTypeChanged)
        { }
        
    }
}
