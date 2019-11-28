using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Data;
using Shared.UI.Countries;
using Shared.Services;
using System.Globalization;
using UIMessager.Services.Message;
using System.Windows;

namespace Shared.ViewModels.Countries
{
    public class CountriesViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Commands
        public DelegateCommand AddNewCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        public DelegateCommand ShowChageInfoPopupCommand { get; set; }

        #endregion

        #region Properties
        public CountryModifyViewModel CountryModifyVM
        {
            get { return _CountryModifyVM; }
            private set { _CountryModifyVM = value; RaisePropertyChanged(() => this.CountryModifyVM); }
        }
        CountryModifyViewModel _CountryModifyVM;

        public ListCollectionView CountryList
        {
            get { return _CountryList; }
            private set { _CountryList = value; RaisePropertyChanged(() => this.CountryList); }
        }
        ListCollectionView _CountryList;

        UI_Country CurrentCountry
        {
            get { return (CountryList != null) ? CountryList.CurrentItem as UI_Country : null; }
        }

        public bool ShowChangeInfoPopup
        {
            get { return _ShowChangeInfoPopup; }
            set { _ShowChangeInfoPopup = value; RaisePropertyChanged(() => this.ShowChangeInfoPopup); }
        }
        bool _ShowChangeInfoPopup;
        CountryService _CountryService;
        #endregion

        #region Visibility
        public bool IsVisibilityEdit
        {
            get
            {
                return (BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company);
            }
        }
        #endregion         

        public CountriesViewModel()
        {
            AddNewCommand = new DelegateCommand(this.OnAddNew);
            EditCommand = new DelegateCommand(this.OnEdit, delegate () { return CurrentCountry != null; });
            DeleteCommand = new DelegateCommand(this.OnDelete, delegate () { return CurrentCountry != null; });

            ShowChageInfoPopupCommand = new DelegateCommand(this.OnShowChangeInfoPopup);

            CountryModifyVM = new CountryModifyViewModel(CountryList_Initialize);
            InitializeViewModel();
            CountryList_Initialize();
        }

        void CountryList_Initialize(UI_Country country = null)
        {
            UI_Country selectCountry = (country == null) ? CurrentCountry : country;
            List<UI_Country> countryList = _CountryService.GetAll();

            if (countryList == null)
                countryList = new List<UI_Country>();
            CountryList = new ListCollectionView(countryList);

            if (selectCountry != null) CountryList.MoveCurrentTo(selectCountry);
            //имитируем изменение выделения, даже если его не было, потому что по умолчанию будет выделена 1-я строка 
            CountryModifyList_CurrentChanged(CountryList, null);
            CountryList.CurrentChanged += new EventHandler(CountryModifyList_CurrentChanged);
        }

        private void InitializeViewModel()
        {
            _CountryService = new CountryService();
        }

        void CountryModifyList_CurrentChanged(object sender, EventArgs e)
        {
            this.EditCommand.RaiseCanExecuteChanged();
            this.DeleteCommand.RaiseCanExecuteChanged();
            CountryModifyVM.Country_CurrentChanged(CurrentCountry);
        }

        #region Execute Commands

        private void OnAddNew()
        {
            CountryModifyVM.Initialize_ActionAdd();

        }

        private void OnEdit()
        {
            CountryModifyVM.Initialize_ActionEdit();

        }

        private void OnDelete()
        {
            if (_CountryService.DeleteIfNotUsed(CurrentCountry))
                CountryList_Initialize();
        }

        private void OnShowChangeInfoPopup()
        {
            if (CurrentCountry != null &&
               _CountryService.LoadChangeInfo(CurrentCountry)
              )
                ShowChangeInfoPopup = true;
        }

        #endregion
    }
}
