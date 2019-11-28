using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Data;
using DBServices.PMS;
using System.Windows;
using Shared.UI.Vessel;
using Shared.ViewModels.Services.Vessel;
using UIMessager.Services.Message;

namespace Shared.ViewModels.Vessel
{
    class ShipTypesViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Commands
        public DelegateCommand AddNewCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        #endregion

        #region Properties
        System.Func<bool> refreshMethodAfterSaving;

        public ShipTypeModifyViewModel ShipTypeModifyVM
        {
            get { return _ShipTypeModifyVM; }
            private set { _ShipTypeModifyVM = value; RaisePropertyChanged(() => this.ShipTypeModifyVM); }
        }
        ShipTypeModifyViewModel _ShipTypeModifyVM;

        public ListCollectionView ShipTypesList
        {
            get { return _ShipTypesList; }
            private set { _ShipTypesList = value; RaisePropertyChanged(() => this.ShipTypesList); }
        }
        ListCollectionView _ShipTypesList;

        UI_ShipType CurrentShipType
        {
            get { return (ShipTypesList != null) ? ShipTypesList.CurrentItem as UI_ShipType : null; }
        }

        #endregion

        public ShipTypesViewModel(System.Func<bool> method)
        {
            AddNewCommand = new DelegateCommand(this.OnAddNew);
            EditCommand = new DelegateCommand(this.OnEdit, delegate() { return CurrentShipType != null; });
            DeleteCommand = new DelegateCommand(this.OnDelete, delegate() { return CurrentShipType != null; });
         
            ShipTypeModifyVM = new ShipTypeModifyViewModel(ShipTypeList_Initialize);
            ShipTypeList_Initialize();
            refreshMethodAfterSaving = method;
        }

        void ShipTypeList_Initialize(UI_ShipType type = null)
        {
            UI_ShipType selectGroup = (type == null) ? CurrentShipType : type;
            List<UI_ShipType> groupList = new ShipTypeService().GetAll();

            ShipTypesList = new ListCollectionView(groupList);

            if (selectGroup != null) ShipTypesList.MoveCurrentTo(selectGroup);
            //имитируем изменение выделения, даже если его не было, потому что по умолчанию будет выделена 1-я строка 
            ShipGroupsList_CurrentChanged(ShipTypesList, null);
            ShipTypesList.CurrentChanged += new EventHandler(ShipGroupsList_CurrentChanged);
            if (refreshMethodAfterSaving != null) refreshMethodAfterSaving();
        }

        void ShipGroupsList_CurrentChanged(object sender, EventArgs e)
        {
            this.EditCommand.RaiseCanExecuteChanged();
            this.DeleteCommand.RaiseCanExecuteChanged();
            ShipTypeModifyVM.Group_CurrentChanged(CurrentShipType);
        }

        #region Execute Commands

        private void OnAddNew()
        {
            ShipTypeModifyVM.Initialize_ActionAdd();

        }

        private void OnEdit()
        {
            ShipTypeModifyVM.Initialize_ActionEdit();

        }

        private void OnDelete()
        {
            if (CurrentShipType == null) return;
            if (new ShipTypeService().HasShips(CurrentShipType))
            {
                MsgInformation.Show(global::Resources.Properties.ResourcesReplaced.mNotDelShipTypeBecauseHasShips);
                return;
            }

            if (MsgConfirm.Show(Properties.Resources.mConfirmDel) == MessageBoxResult.No) return;
            
            if (new ShipTypeService().Delete(CurrentShipType))
                ShipTypeList_Initialize();
        }

        #endregion
    }

}
