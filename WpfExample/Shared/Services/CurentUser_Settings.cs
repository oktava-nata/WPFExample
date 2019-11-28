using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Настройки для хранения последнего логина и пароля пользователя (для каждого приложения свои значения)
    /// </summary>
    [Serializable]
    public class CurrentUser_Settings : BaseLib.ICommonUserSettings
    {
        [System.Xml.Serialization.XmlIgnore]
        public string PostfixFileName { get { return "CU"; } }

        //public int UserParam { get; set; }        
        public string LastLogin { get; set; }
        public int? Curu { get; set; }
        public string Pcuru { get; set; }

        public CurrentUser_Settings()
        {
            LastLogin = "user";
        }

        public static CurrentUser_Settings GetCurrentSettings()
        {
            //Только для программы MARINOS(бывший PMS) ищем старые настройки CurrentUser_Settings
            if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                string oldPathOfSettings = System.IO.Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), string.Format(@"SOFTMARINE\Planned Maintenance System\{0}", BaseLib.AppManager.CommonInfo.GetTypeModuleToString()));
                return new BaseLib.CommonUserSettingsManager<CurrentUser_Settings>().GetSettings(oldPathOfSettings);
            }
            return new BaseLib.CommonUserSettingsManager<CurrentUser_Settings>().GetSettings();
        }

        public bool Save()
        {
            return new BaseLib.CommonUserSettingsManager<CurrentUser_Settings>().Save(this);
        }
    }
}
