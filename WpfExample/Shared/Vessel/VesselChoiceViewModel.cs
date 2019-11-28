using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DBServices.UsualEntity;
using Microsoft.Practices.Prism.Commands;

namespace Shared.Vessels
{
    public class VesselChoiceViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Commands
        public DelegateCommand ShowShipListCommand { get; private set; }
        public DelegateCommand UnSelectShipCommand { get; private set; }
        #endregion

        #region Properties
        public bool CouldChangeSelectingShip
        {
            get { return _CouldChangeSelectingShip; }
            set { _CouldChangeSelectingShip = value; RaisePropertyChanged(() => this.CouldChangeSelectingShip); }
        }
        bool _CouldChangeSelectingShip;

        public bool CouldUnSelectShip
        {
            get { return _CouldUnSelectShip; }
            set { _CouldUnSelectShip = value; RaisePropertyChanged(() => this.CouldUnSelectShip); }
        }
        bool _CouldUnSelectShip;

        public bool BigOneLineName
        {
            get { return _BigOneLineName; }
            set { _BigOneLineName = value; RaisePropertyChanged(() => this.BigOneLineName); }
        }
        bool _BigOneLineName;

        public bool IsShowVesselList
        {
            get { return _IsShowVesselList; }
            set { _IsShowVesselList = value; RaisePropertyChanged(() => this.IsShowVesselList); }
        }
        bool _IsShowVesselList;

        public ListCollectionView ShipList
        {
            get { return _ShipList; }
            private set
            {
                _ShipList = value;
                RaisePropertyChanged(() => this.ShipList);
                this.ShowShipListCommand.RaiseCanExecuteChanged();
                this.UnSelectShipCommand.RaiseCanExecuteChanged();
            }
        }
        ListCollectionView _ShipList;


        public bool CouldHasManyVessels
        {
            get { return BaseLib.AppManager.CommonInfo.Module_IsTypeCompany; }
        }

        public Ship CurrentShip
        {
            get
            {
                if (CouldHasManyVessels)
                    return (ShipList != null) ? ShipList.CurrentItem as Ship : null;
                else return CurrentShipManager.CurrentShip;
            }
        }

        public Action CurrentShip_Change;

        #endregion

        public VesselChoiceViewModel(bool couldChangeSelectingShip = true, bool bigOneLineName = false)
        {
            CouldChangeSelectingShip = couldChangeSelectingShip;
            BigOneLineName = bigOneLineName;

            VM_Initialize();

            // ShipList_Initialize();
        }

        void VM_Initialize()
        {
            ShowShipListCommand = new DelegateCommand(this.ShowShipListCommand_Execute,
                delegate()
                {
                    return ShipList != null;
                }
            );
            UnSelectShipCommand = new DelegateCommand(this.ShowShipListCommand_Execute,
                delegate()
                {
                    return ShipList != null && CurrentShip != null;
                }
            );
        }

        public void Load()
        {
            ShipList_Initialize();
        }

        public void ReloadShipList()
        {
            if (ShipFactory.Load())
                ShipList_Initialize();
            else ShipList = null;
        }

        void ShipList_Initialize()
        {
            var ships = ShipFactory.ShipList;
            if (ships == null)
            {
                ShipList = null;
                return;
            }
            ShipList = new ListCollectionView(ships);
            ShipList.CurrentChanged += new EventHandler(ShipList_CurrentChanged);
            SetCurrentShip();
        }


        public void SetCurrentShip()
        {
            if (ShipList != null)
            {
                ShipList.MoveCurrentTo(CurrentShipManager.CurrentShip);
                RaisePropertyChanged(() => this.CurrentShip);
            }
        }


        void ShipList_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrentShipManager.SetShip(CurrentShip))
            {
                IsShowVesselList = false;
                if (CurrentShip_Change != null) CurrentShip_Change();
            }
            
            this.UnSelectShipCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => this.CurrentShip);
        }

        public void ShowVesselList()
        {
            IsShowVesselList = true;
        }

        #region Execute Commands
        void ShowShipListCommand_Execute()
        {
            ShowVesselList();
        }

        void UnSelectShipCommand_Execute()
        {
            ShipList.MoveCurrentTo(null);
        }
        #endregion
    }

}
