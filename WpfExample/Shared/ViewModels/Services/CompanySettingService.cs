using System;
using System.Collections.Generic;
using ViewModelBaseSolutions.Services;
using UIMessager.Services.Message;
using DBServices.UsualEntity;
using Shared.UI.Vessel;
using ViewModelBaseSolutions.UIEntityHelper;
using UI;
using Shared.UI.CompanySettings;
using DBServices.PMS;
using ViewModelBaseSolutions.UIEntityHelper.Files;

namespace Shared.ViewModels.Services
{
    public class CompanySettingService : TService<UI_CompanySetting>
    {
        /// <summary>
        /// Получение записи, существующей в БД или только что сформированной, ещё не добавленной в БД
        /// для последующего редактирования или добавления в БД
        /// </summary>
        public UI_CompanySetting Get(bool readOnly = false)
        {
            try
            {
                var entity = PMS_CompanySetting.Get(readOnly);
                if (entity != null)
                {
                    UI_CompanySetting settings = new UI_CompanySetting(entity);
                    //для редактирования загружаем файл, для заполнения свойства FileDataBytes и отображения его в пользовательском интерфейсе                     
                    if (!readOnly && settings.Logo != null) settings.Logo.LoadFileData();
                    return settings;
                }
                else return new UI_CompanySetting();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }
        /// <summary>
        /// Сохранение настроек компании
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="Logo">новый файл-логотип для добавления</param>
        /// <param name="LogoForDel">файл-логотип для удаления</param>
        /// <param name="wasChanged"></param>
        /// <returns></returns>
        public bool Save(UI_CompanySetting setting, UI_File<PMS_CompanySettingLogoFileInfo> Logo, UI_File<PMS_CompanySettingLogoFileInfo> LogoForDel, out bool wasChanged)
        {
            wasChanged = false;
            try
            {
                wasChanged = setting.Save(Logo, LogoForDel);
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        public bool Add(UI_CompanySetting setting, UI_File<PMS_CompanySettingLogoFileInfo> Logo)
        {
            try
            {
                setting.Add(Logo);
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        /// <summary>
        /// Подгрузка логотипа
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static bool LoadFileData(UI_CompanySetting setting)
        {
            try
            {
                setting.Logo.LoadFileData();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        /*/// <summary>
        /// Получение записи настройки расчета статистики, существующей в БД, либо фиктивно-созданную, если запись ещё не существует в БД 
        /// </summary>
        public UI_JobStatisticSetting GetForReadOnly()
        {
            try
            {
                var entity = PMS_JobStatisticSetting.Get(true);
                if (entity != null) return new UI_JobStatisticSetting(entity);
                else return new UI_JobStatisticSetting();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }*/

    }
}
