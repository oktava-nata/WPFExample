using System;
using System.Threading.Tasks;
using VMBaseSolutions.VMEntities;
using VMBaseSolutions.VMFactories;
using VMBaseSolutions.VMServices;

namespace VMBaseSolutions.ModifyVMs
{
    public class ModifyWithFactoryAndCUDServiceVM<TFactory, TCUDService, TViewModel, TEntity> : ModifyVM<TViewModel>
     where TFactory : IVMFactory<TViewModel>, new()
     where TCUDService : CUDVMService<TViewModel, TEntity>, new()
     where TViewModel : VMEntityBase<TEntity>
     where TEntity : Infrastructure.Domain.IAggregateRootEntity
    {
        protected TFactory VMFactory { get; private set; }

        protected TCUDService CUDService { get; private set; }

        public ModifyWithFactoryAndCUDServiceVM(Action<TViewModel> onModifyViewModel = null, Action<bool> onFinishModify = null)
            : base(onModifyViewModel, onFinishModify)
        {
            VMFactory = new TFactory();
            CUDService = new TCUDService();
        }

        protected override TViewModel CreateTViewModelForAdding()
        {
            return VMFactory.Create();
        }

        protected async override Task<TViewModel> GetTargetVMForAddingAsync()
        {
            return await Task.Run(()=> VMFactory.Create());
        }

        protected override TViewModel GetTViewModelById(int id)
        {
            return VMFactory.GetById(id);
        }

        protected async override Task<TViewModel> GetTViewModelByIdAsync(int id)
        {
            return await VMFactory.GetByIdAsync(id);
        }


        protected override bool AddTViewModel(TViewModel targetVM)
        {
            return CUDService.Add(ModifyTargetVM);
        }

        protected override bool UpdateTViewModel(TViewModel targetVM)
        {
            return CUDService.Update(ModifyTargetVM);
        }

       
        
    }
}
