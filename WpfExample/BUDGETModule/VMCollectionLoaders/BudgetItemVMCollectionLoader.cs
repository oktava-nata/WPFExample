using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.BudgetItems;
using Domain.Common.Services.Budget;
using Models.Budget;

namespace BUDGETModule.VMCollectionLoaders
{
    class BudgetItemVMCollectionLoader
    {
        IBudgetItemReadService _Service;

        public ObservableCollection<BudgetItemViewModel> BudgetItemVMList { get; private set; }
        public ObservableCollection<BudgetItemViewModel> BudgetItemTreeVMList { get; private set; }
        public ObservableCollection<BudgetItemViewModel> GroupingBudgetItemTreeVMList { get; private set; }

        static Expression<Func<BudgetItemViewModel, string>> _OrderByDefaultExpression { get { return i => i.CodeAndName; } }


        public BudgetItemVMCollectionLoader()
        {
            _Service = Domain.Services.Factories.BudgetItemServicesFactory.CreateBudgetItemReadService();
        }

        public async Task<ObservableCollection<BudgetItemViewModel>> GetAllBudgetItemVMOutOfShipownerTreeViewAsync()
        {
            await GetAllBudgetItemVMByShipownerAsync(shipOwnerId: null);
            BudgetItemTreeVMList = StructureHierarchyBudgetItemVMList(BudgetItemVMList);
            return BudgetItemTreeVMList;
        }

        public ObservableCollection<BudgetItemViewModel> GetAllBudgetItemVMOutOfShipownerTreeView()
        {
            GetAllBudgetItemVMByShipowner(shipOwnerId: null);
            BudgetItemTreeVMList = StructureHierarchyBudgetItemVMList(BudgetItemVMList);
            return BudgetItemTreeVMList;
        }

        public async Task<ObservableCollection<BudgetItemViewModel>> GetAllBudgetItemVMByShipownerTreeViewAsync(int shipOwnerId)
        {
            await GetAllBudgetItemVMByShipownerAsync(shipOwnerId: shipOwnerId);
            BudgetItemTreeVMList = StructureHierarchyBudgetItemVMList(BudgetItemVMList);
            return BudgetItemTreeVMList;
        }

        public ObservableCollection<BudgetItemViewModel> GetAllBudgetItemVMByShipownerTreeView(int shipOwnerId)
        {
            GetAllBudgetItemVMByShipowner(shipOwnerId: shipOwnerId);
            BudgetItemTreeVMList = StructureHierarchyBudgetItemVMList(BudgetItemVMList);
            return BudgetItemTreeVMList;
        }

        /// <summary>
        /// Получить доступные групповые статьи в иерархическом виде, в которые можно назначить родительскими для указанной статьи 
        /// Список всех статей BudgetItemVMList должен быть заранее загружен и иерархически структурирован в BudgetItemTreeVMList
        /// </summary>
        public ObservableCollection<BudgetItemViewModel> GetGroupingBudgetItemTreeVMAvailableForChildBudgetItem(BudgetItemViewModel childItem)
        {
            if (BudgetItemVMList == null)
                throw new Exception("BudgetItemVMList was not load!");
            if (BudgetItemTreeVMList == null)
                throw new Exception("BudgetItemTreeVMList was not load!");

            GroupingBudgetItemTreeVMList = new ObservableCollection<BudgetItemViewModel>();
            bool needExcludeChildItem = childItem != null && childItem.IsGroupingItem && childItem.Id > 0;
            foreach (var item in BudgetItemVMList.Where(i => i.IsGroupingItem))
            {
                item.GroupItems = new ObservableCollection<BudgetItemViewModel>();
                if (item.Items == null) continue;
                var onlyGroupItems = item.Items.Where(i => i.IsGroupingItem);
                if (needExcludeChildItem) onlyGroupItems = onlyGroupItems.Where(i => i.Id != childItem.Id);
                foreach (var i in onlyGroupItems.OrderBy(_OrderByDefaultExpression.Compile()))
                    item.GroupItems.Add(i);
            }
            var tree = BudgetItemTreeVMList.Where(i => i.IsGroupingItem);
            if (needExcludeChildItem) tree = tree.Where(i => i.Id != childItem.Id);
            GroupingBudgetItemTreeVMList = new ObservableCollection<BudgetItemViewModel>();
            foreach (var item in tree.OrderBy(_OrderByDefaultExpression.Compile()))
                GroupingBudgetItemTreeVMList.Add(item);

            return GroupingBudgetItemTreeVMList;
        }


        public async Task<ObservableCollection<BudgetItemViewModel>> GetAllBudgetItemVMByShipownerAsync(int? shipOwnerId)
        {
            var listModels = (shipOwnerId.HasValue) ? await _Service.GetAllByShipownerAsync(shipOwnerId.Value) : await _Service.GetAllForCompanyAsync();
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<BudgetItemViewModel, BudgetItem>.ConvertToViewModel(listModels);

            BudgetItemVMList = new ObservableCollection<BudgetItemViewModel>(listVM);

            return BudgetItemVMList;
        }

        public ObservableCollection<BudgetItemViewModel> GetAllBudgetItemVMByShipowner(int? shipOwnerId)
        {
            var listModels = (shipOwnerId.HasValue) ? _Service.GetAllByShipowner(shipOwnerId.Value) : _Service.GetAllForCompany();
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<BudgetItemViewModel, BudgetItem>.ConvertToViewModel(listModels);

            BudgetItemVMList = new ObservableCollection<BudgetItemViewModel>(listVM);

            return BudgetItemVMList;

        }


        static ObservableCollection<BudgetItemViewModel> StructureHierarchyBudgetItemVMList(IEnumerable<BudgetItemViewModel> sourcelist)
        {
            if (sourcelist == null) return null;
            var itemCollection = new ObservableCollection<BudgetItemViewModel>();
            var listModelsParents = sourcelist.Where(i => i.IdParent == null).OrderBy(_OrderByDefaultExpression.Compile());
            foreach (var item in listModelsParents)
            {
                itemCollection.Add(formBudgetItemVM(item, sourcelist));
            }
            return itemCollection;
        }

        static BudgetItemViewModel formBudgetItemVM(BudgetItemViewModel item, IEnumerable<BudgetItemViewModel> allItems)
        {
            item.Items = new ObservableCollection<BudgetItemViewModel>();
            foreach (var ch in allItems.Where(i => i.IdParent == item.Id).OrderBy(_OrderByDefaultExpression.Compile()))
            {
                var chVM = formBudgetItemVM(ch, allItems);
                item.Items.Add(chVM);
            }
            return item;
        }

    }
}
