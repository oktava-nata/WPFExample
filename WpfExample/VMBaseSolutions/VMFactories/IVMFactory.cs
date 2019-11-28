using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMBaseSolutions.VMFactories
{
    public interface IVMFactory<T> where T : VMBaseSolutions.VMEntities.VMBase
    {
        Task<T> GetByIdAsync(int itemId);
        T GetById(int itemId);
        T Create();
    }
}
