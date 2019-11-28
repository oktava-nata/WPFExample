using System.ComponentModel;

namespace VMBaseSolutions.VMEntities
{
    public abstract class VMBase : Telerik.Windows.Controls.ViewModelBase, IDataErrorInfo
    {
        string IDataErrorInfo.Error
        {
            get { return null; } //throw new NotImplementedException();
        }

        public abstract string this[string columnName] { get; }

        public virtual bool IsValid()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if ((this as System.ComponentModel.IDataErrorInfo)[property.Name] != null)
                {
                    return false;
                }
            }
            return true;
        }
    }

}

