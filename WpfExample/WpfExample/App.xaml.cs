using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfExample
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {




        //protected override BaseLib.CommonProgrammInfo CreateCommonProgrammInfo()
        //{
        //    return CommonProgInfo = new BaseLib.CommonProgrammInfo
        //    {
        //        CompanyName = "TEST",
        //        VesselName = "TEST",
        //        VesselNumber = "001",
        //        VesselFlag = "RUS",

        //        Version = new BaseLib.ProgramVersion(0, 0, 0),
        //        ProgramName = "TEST",
        //        //Projects = new List<CommonProgrammInfo.ProjectName> { CommonProgrammInfo.ProjectName.PMS },
        //        Module = BaseLib.CommonProgrammInfo.TypeModule.Company,
        //        //Craft = CommonProgrammInfo.TypeCraft.Vessel,
        //        DeveloperCompanyName = "SOFT MARINE Ltd",
        //        Year = "2016",

        //    };

        //}
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new BUDGETModule.ViewModels.BUDGETMenuButtonViewModel();
            mainWindow.ShowDialog();
        }



    }
}
