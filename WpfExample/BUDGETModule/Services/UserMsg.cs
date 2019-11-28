using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BUDGETModule.Services
{
    internal class UserMsg : UIMessager.Services.UserMsgManager
    {
        protected override System.Resources.ResourceManager ResCurrentModule
        {
            get { return new System.Resources.ResourceManager("BUDGETModule.Properties.Resources", typeof(MainWindow).Assembly); }
        }
    }
}
