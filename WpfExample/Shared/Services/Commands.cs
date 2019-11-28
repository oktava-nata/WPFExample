using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Shared.Services
{
    public class MenuCommands
    {
        public static RoutedUICommand Yes { get; private set; }
        

        static MenuCommands()
        {
            Initialize();
        }

        public static void Initialize()
        {
            Yes = new RoutedUICommand(global::Resources.Properties.Resources.txtYes, "cmd_yes", typeof(MenuCommands));           
        }
    }

}
