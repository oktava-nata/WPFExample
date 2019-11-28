using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Domain.Services.Specifications.Budget;
using Models.Budget;

namespace Domain.Services.Budget
{

    internal class BudgetItemCopyService : IBudgetItemCopyService
    {
        BudgetContext _BudgetContext;
        BudgetItemReadService _BudgetItemReadService;
        BudgetItemCUDService _BudgetItemCUDService;

        public BudgetItemCopyService()
        {
            _BudgetContext = new BudgetUnitOfWorkFactory().CreateBudgetContext();
            _BudgetItemCUDService = new BudgetItemCUDService(_BudgetContext);
            _BudgetItemReadService = new BudgetItemReadService();
        }

        public BudgetItem CopyBudgetItemWithChildrenItemsToShipowner(int itemId, int shipownerId)
        {
            BudgetItem sourceItem = _BudgetItemReadService.GetById(itemId);
            _BudgetItemCUDService.Context.TurnOffAutoDetectChangesAndValidateOnSave();
            var propertyManager = new BaseLib.Services.TPropertyManager<BudgetItem>();
            propertyManager.DefineAllSimplePropertieForManage();
            BudgetItem copyItem = CreateCopyBudgetItemWithChildrenItemsToShipownerAndAddInSet(propertyManager, sourceItem, shipownerId, null);
            int count = _BudgetItemCUDService.Context.SaveChanges();
            _BudgetItemCUDService.Context.TurnOnAutoDetectChangesAndValidateOnSave();
            return (count > 0)? copyItem: null;
        }

        public async Task<BudgetItem> CopyBudgetItemWithChildrenItemsToShipownerAsync(int itemId, int shipownerId)
        {
            BudgetItem sourceItem = await _BudgetItemReadService.GetByIdAsync(itemId);
            _BudgetItemCUDService.Context.TurnOffAutoDetectChangesAndValidateOnSave();
            var propertyManager = new BaseLib.Services.TPropertyManager<BudgetItem>();
            propertyManager.DefineAllSimplePropertieForManage();
            BudgetItem copyItem = CreateCopyBudgetItemWithChildrenItemsToShipownerAndAddInSet(propertyManager, sourceItem, shipownerId, null);
            int count = await _BudgetItemCUDService.Context.SaveChangesAsync();
            _BudgetItemCUDService.Context.TurnOnAutoDetectChangesAndValidateOnSave();
            return (count > 0) ? copyItem : null;
        }

       
        private BudgetItem CreateCopyBudgetItemWithChildrenItemsToShipownerAndAddInSet(BaseLib.Services.TPropertyManager<BudgetItem> propertyManager, BudgetItem sourceItem, int shipownerId, BudgetItem parentItem)
        {
            BudgetItem itemCopy = new BudgetItem();
            propertyManager.CopyProperties(sourceItem, itemCopy, true);
            itemCopy.SetShipOwner(shipownerId);
            itemCopy.SetParentItem(parentItem);
            System.Threading.Thread.Sleep(2000);
            _BudgetItemCUDService.TEntitySet.Add(itemCopy);

            IEnumerable<BudgetItem> childrenItems = _BudgetItemReadService.GetCollection(new BudgetItemsByParentSpecification(sourceItem.Id));
            foreach (var item in childrenItems)
            {
                BudgetItem copyChild = CreateCopyBudgetItemWithChildrenItemsToShipownerAndAddInSet(propertyManager, item, shipownerId, itemCopy);
                itemCopy.InsertChildrenItem(copyChild);
            }
            return itemCopy;
        }


    }
}
