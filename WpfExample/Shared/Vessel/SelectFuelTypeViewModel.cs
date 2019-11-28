using System;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using DBServices.PMS;
using DBServices;
using DBServices.UsualEntity;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using ViewModelBaseSolutions.Items;
using System.Collections.ObjectModel;
using DBServices.MRV;

namespace Shared.Vessel
{
    //Olga 
    public class SelectFuelTypeViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Properties
        public string TextForEmptyList
        {
            get { return _TextForEmptyList; }
            private set { _TextForEmptyList = value; RaisePropertyChanged(() => this.TextForEmptyList); }
        }
        string _TextForEmptyList;

        public ListCollectionView FuelTypeList
        {
            get { return _FuelTypeList; }
            set { _FuelTypeList = value; RaisePropertyChanged(() => this.FuelTypeList); }
        }
        ListCollectionView _FuelTypeList;

        MRV_FuelType  SelectedFuelType
        {
            get
            {
                return (FuelTypeList != null && FuelTypeList.CurrentItem != null)
                    ? (FuelTypeList.CurrentItem as CheckedItemWithCommandsViewModel<MRV_FuelType>).ItemObject
                    : null;
            }
        }

        #region BackgroundWorker
        public bool ShowWaiting
        {
            get { return _ShowWaiting; }
            set { _ShowWaiting = value; RaisePropertyChanged(() => this.ShowWaiting); }
        }
        bool _ShowWaiting;

        BackgroundWorker _backgroundWorker = null;
        bool _backgroundWorkerRunAgainAfterCanceled;
        #endregion

        MRV_FuelType _selectFuelType;
        public List<MRV_FuelType> FuelTypeListIsUseOld { get; set; }
        #endregion

        #region Constructor
        public SelectFuelTypeViewModel(List<MRV_FuelType> fuelTypeListIsUseOld)
        {
            FuelTypeListIsUseOld = fuelTypeListIsUseOld;
            TextForEmptyList = Shared.Properties.Resources.txtFuelTypeDictionaryIsEmpty;
            InitializeVM();
        }

        private void InitializeVM()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoLoadingFuelTypeList);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_LoadingFuelTypeListCompleted);
            _backgroundWorker.WorkerSupportsCancellation = true;
        }

        #endregion

        #region Public Loaders
        public void LoadFuelTypeList()
        {
            reloadList();
        }
        #endregion

        #region loadMethods
        void reloadList(MRV_FuelType selectFuelType = null)
        {
            _selectFuelType = (FuelTypeList != null && selectFuelType == null) ? SelectedFuelType : selectFuelType;
            //если процесс уже запущен - останавливаем его, иначе запускаем процесс
            if (!backgroundWorker_CancelIfRunning(true))
                //запускаем процесс
                _backgroundWorker.RunWorkerAsync();
        }

        //срабатывает событие при запуске процесса
        void backgroundWorker_DoLoadingFuelTypeList(object sender, DoWorkEventArgs e)
        {
            ShowWaiting = true;           
            List<MRV_FuelType> fuelTypeList = MRV_FuelType.GetActiveFuelTypeList();
            
            //прерывание процесса
            if (_backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            var gridItemFuelTypeList = new ListCheckedItemViewModel<MRV_FuelType>(fuelTypeList, false);
            if (FuelTypeListIsUseOld != null)
            {
                foreach (var fuelType in gridItemFuelTypeList)
                {
                    if (FuelTypeListIsUseOld.Exists(x => x.Id == fuelType.ItemObject.Id)) { fuelType.IsChecked = true; }
                }
            }

            FuelTypeList = new ListCollectionView(gridItemFuelTypeList);
        }

        //событие срабатывает при завершении метода DoWork
        void backgroundWorker_LoadingFuelTypeListCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //процесс загрузки был отменен
            if (e.Cancelled)
            {
                //если нужно запускаем процесс заново
                if (_backgroundWorkerRunAgainAfterCanceled)
                {
                    _backgroundWorkerRunAgainAfterCanceled = false;
                    reloadList();
                }
                else ShowWaiting = false;
            }
            else
            {
                ShowWaiting = false;
                _selectFuelType = null;
            }
        }

        bool backgroundWorker_CancelIfRunning(bool needRunAgain)
        {
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorkerRunAgainAfterCanceled = needRunAgain;
                _backgroundWorker.CancelAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region Methods
        public void CancelWorking()
        {
            backgroundWorker_CancelIfRunning(false);
        }

        //Получаем выделенные галочками типы топлива
        public List<MRV_FuelType> GetChecked()
        {
            if (FuelTypeList == null) return null;

            var checkedFuelTypeList = new List<MRV_FuelType>();
            foreach (var item in FuelTypeList)
            {
                if (!(item is CheckedItemViewModel<MRV_FuelType>)) continue;
                var chekedItem = item as CheckedItemViewModel<MRV_FuelType>;
                if (chekedItem.IsChecked)
                    checkedFuelTypeList.Add(chekedItem.ItemObject);
            }
            return checkedFuelTypeList;
        }
        #endregion
    }
}

