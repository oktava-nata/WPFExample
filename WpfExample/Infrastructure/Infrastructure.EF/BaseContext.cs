using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Infrastructure.EF
{
    public class BaseContext : DbContext, IUnitOfWork
    {
        protected BaseContext(IEFInitializer configuration)
            :base(configuration.GetSqlConnectionStr())
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 60 * 3; 
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors.First().ValidationErrors.ToList();
                string msg = string.Format("{0} \nDbEntityValidationException:", ex.Message);
                foreach (var item in errors)
                {
                    msg = string.Format("{0}\nPropertity '{1}': {2}", msg, item.PropertyName, item.ErrorMessage);
                }
                throw new System.Exception(msg);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors.First().ValidationErrors.ToList();
                string msg = string.Format("{0} \nDbEntityValidationException:", ex.Message);
                foreach (var item in errors)
                {
                    msg = string.Format("{0}\nPropertity '{1}': {2}", msg, item.PropertyName, item.ErrorMessage);
                }
                throw new System.Exception(msg);
            }
        }

        

        public void TurnOffAutoDetectChangesAndValidateOnSave()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }
        public void TurnOnAutoDetectChangesAndValidateOnSave()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

    }
}
