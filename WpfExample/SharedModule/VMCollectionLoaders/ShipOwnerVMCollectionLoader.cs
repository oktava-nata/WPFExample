using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Common.Services.ShipArea;
using Models.ShipArea;
using SharedModule.ViewModels.Directories;

namespace SharedModule.VMCollectionLoaders
{
    public class ShipOwnerVMCollectionLoader
    {
        IShipOwnerReadServices _Service;

        public ObservableCollection<ShipOwnerViewModel> ShipOwnerVMList { get; private set; }
        public ObservableCollection<ShipOwnerViewModel> ShipOwnerWithSameIdVMList { get; private set; }


        public static Expression<Func<ShipOwnerViewModel, string>> _OrderByDefaultExpression { get { return i => i.Name; } }


        public ShipOwnerVMCollectionLoader()
        {
            _Service = Domain.Services.Factories.ShipOwnerServicesFactory.CreateShipOwnerReadService();
        }


        public async Task<ObservableCollection<ShipOwnerViewModel>> GetAllAsync()
        {
            var listModels = await _Service.GetAllAsync();
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToViewModel(listModels);

            ShipOwnerVMList = new ObservableCollection<ShipOwnerViewModel>(listVM);

            return ShipOwnerVMList;
        }

        public async Task<ObservableCollection<ShipOwnerViewModel>> GetAllWithSameIdAsync(List<int> ids)
        {
            ShipOwnerWithSameIdVMList = new ObservableCollection<ShipOwnerViewModel>();
            IEnumerable<ShipOwner> listModels;           
            listModels = await _Service.GetAllWithSameIdAsync(ids);
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToViewModel(listModels);

            foreach (var item in listVM) ShipOwnerWithSameIdVMList.Add(item);

            return ShipOwnerWithSameIdVMList;
        }   


    }
}
