using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VMBaseSolutions.VMEntities
{
    public static class VMEntityListConvertor<TViewModel, TEntity>
        where TEntity : Infrastructure.Domain.IAggregateRootEntity
        where TViewModel : VMEntityBase<TEntity>, new()
    {
        public static IEnumerable<TViewModel> ConvertToViewModel(IEnumerable<TEntity> list)
        {
            List<TViewModel> result = new List<TViewModel>();
            if (list == null) return null;
            foreach (var item in list)
            {
                TViewModel e = new TViewModel();
                e.SetEntity(item);
                result.Add(e);
            }
            return result;
        }

        public static IEnumerable<TEntity> ConvertToModel(IEnumerable<TViewModel> list)
        {
            if (list!=null)
                return list.Select(f => f.GetEntity()).ToList();
            return null;
        }
    }
}

