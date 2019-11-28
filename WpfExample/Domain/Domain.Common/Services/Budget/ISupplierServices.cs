using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Domain;
using Models.Budget;
using Models.ShipArea;

namespace Domain.Common.Services.Budget
{
    public interface ISupplierReadService : IHasGetByIdMethods<Supplier>//, IHasGetAllMethods<Supplier>
    {
        Supplier GetById(int id, params Expression<Func<Supplier, object>>[] includeProperties);
        Task<Supplier> GetByIdAsync(int id, params Expression<Func<Supplier, object>>[] includeProperties);
        Task<ICollection<Supplier>> GetAllAsync(params Expression<Func<Supplier, object>>[] includeProperties);
        ICollection<Supplier> GetAll(params Expression<Func<Supplier, object>>[] includeProperties);
        Task<ICollection<Supplier>> GetAllByShipownerAsync(int shipownerId);
        ICollection<Supplier> GetAllByShipowner(int shipownerId);
        Task<ICollection<Supplier>> GetAllUsedInCompanyBudgetAsync();
        ICollection<Supplier> GetAllUsedInCompanyBudget();
    }

    public interface ISupplierCUDService : ICUDService<Supplier>
    {
        bool Update(Supplier sourceEntity, IEnumerable<ShipOwner> shipOwnerForInclude, IEnumerable<ShipOwner> shipOwnerForExclude);
    }

   
}
