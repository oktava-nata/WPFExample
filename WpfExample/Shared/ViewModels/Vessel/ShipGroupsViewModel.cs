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
    class ShipGroupsViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Commands
        public DelegateCommand AddNewCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        #endregion

        #region Properties
        System.Func<bool> refreshMethodAfterSaving;

        public ShipGroupModifyViewModel ShipGroupModifyVM
        {
            get { return _ShipGroupModifyVM; }
            private set { _ShipGroupModifyVM = value; RaisePropertyChanged(() => this.ShipGroupModifyVM); }
        }
        ShipGroupModifyViewModel _ShipGroupModifyVM;

        public ListCollectionView ShipGroupsList
        {
            get { return _ShipGroupsList; }
            private set { _ShipGroupsList = value; RaisePropertyChanged(() => this.ShipGroupsList); }
        }
        ListCollectionView _ShipGroupsList;

        UI_ShipGroup CurrentShipGroup
        {
            get { return (ShipGroupsList != null) ? ShipGroupsList.CurrentItem as UI_ShipGroup : null; }
        }

        public bool ShowChangeInfoPopup
        {
            get { return _ShowChangeInfoPopup; }
            set { _ShowChangeInfoPopup = value; RaisePropertyChanged(() => this.ShowChangeInfoPopup); }
        }
        bool _ShowChangeInfoPopup;

        #endregion

        public ShipGroupsViewModel(System.Func<bool> method)
        {
            AddNewCommand = new DelegateCommand(this.OnAddNew);
            EditCommand = new DelegateCommand(this.OnEdit, delegate() { return CurrentShipGroup != null; });
            DeleteCommand = new DelegateCommand(this.OnDelete, delegate() { return CurrentShipGroup != null; });
         
            ShipGroupModifyVM = new ShipGroupModifyViewModel(ShipGroupsList_Initialize);
            ShipGroupsList_Initialize();
            refreshMethodAfterSaving = method;
        }

        void ShipGroupsList_Initialize(UI_ShipGroup group = null)
        {
            UI_ShipGroup selectGroup = (group == null) ? CurrentShipGroup : group;
            List<UI_ShipGroup> groupList = new ShipGroupService().GetAll();

            ShipGroupsList = new ListCollectionView(groupList);

            if (selectGroup != null) ShipGroupsList.MoveCurrentTo(selectGroup);
            //имитируем изменение выделения, даже если его не было, потому что по умолчанию будет выделена 1-я строка 
            ShipGroupsList_CurrentChanged(ShipGroupsList, null);
            ShipGroupsList.CurrentChanged += new EventHandler(ShipGroupsList_CurrentChanged);
            if (refreshMethodAfterSaving != null) refreshMethodAfterSaving();
        }

        void ShipGroupsList_CurrentChanged(object sender, EventArgs e)
        {
            this.EditCommand.RaiseCanExecuteChanged();
            this.DeleteCommand.RaiseCanExecuteChanged();
            ShipGroupModifyVM.Group_CurrentChanged(CurrentShipGroup);
        }

        #region Execute Commands

        private void OnAddNew()
        {
            ShipGroupModifyVM.Initialize_ActionAdd();

        }

        private void OnEdit()
        {
            ShipGroupModifyVM.Initialize_ActionEdit();

        }

        private void OnDelete()
        {
            if (new ShipGroupService().HasShips(CurrentShipGroup))
            {
                MsgInformation.Show(global::Resources.Properties.ResourcesReplaced.mNotDelShipTypeBecauseHasShips);
                return;
            }
            if (CurrentShipGroup == null) return;
            if (MsgConfirm.Show(Properties.Resources.mConfirmDel) == MessageBoxResult.No) return;
            
            if (new ShipGroupService().Delete(CurrentShipGroup))
                ShipGroupsList_Initialize();
        }

        #endregion
    }

}
