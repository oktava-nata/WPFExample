using System;

namespace VMBaseSolutions.VMEntities
{
    public abstract class VMEntityBase<TEntity> : VMBase, IVMHasId
     where TEntity : Infrastructure.Domain.IAggregateRootEntity
    {
        protected TEntity _entity { get; private set; }

        public virtual int Id { get { return _entity.Id; } }

        protected VMEntityBase(TEntity entity)
        {
            SetEntity(entity);
        }

        public virtual void SetEntity(TEntity entity)
        {
            if (entity == null)
                throw new Exception("Attempting to initialize ViewModel by empty entity");
            _entity = entity;
        }

        public override bool Equals(object obj)
        {
            if (obj is VMEntityBase<TEntity>)
                return this.Id.Equals((obj as VMEntityBase<TEntity>).Id);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual TEntity GetEntity()
        {
            return this._entity;
        }
    }


}

