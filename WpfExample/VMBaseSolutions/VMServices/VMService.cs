using System;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace VMBaseSolutions.VMServices
{
    public abstract class CUDVMService<TViewModel, TEntity>
        where TViewModel : VMBaseSolutions.VMEntities.VMEntityBase<TEntity>
        where TEntity : Infrastructure.Domain.IAggregateRootEntity
    {
        protected abstract ICUDService<TEntity> CreateCUDService();

        public virtual bool Add(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return serv.Add(vm.GetEntity());
            }
        }

        public virtual async Task<bool> AddAsync(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return await serv.AddAsync(vm.GetEntity());
            }
        }

        public virtual bool Update(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return serv.Update(vm.GetEntity());
            }
        }

        public virtual async Task<bool> UpdateAsync(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return await serv.UpdateAsync(vm.GetEntity());
            }
        }

        public virtual bool Delete(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return serv.Delete(vm.GetEntity());
            }
        }

        public virtual async Task<bool> DeleteAsync(TViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            using (var serv = CreateCUDService())
            {
                return await serv.DeleteAsync(vm.GetEntity());
            }
        }
    }


}
