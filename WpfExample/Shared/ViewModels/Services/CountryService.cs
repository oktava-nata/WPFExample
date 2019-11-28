using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelBaseSolutions.Services;
using ViewModelBaseSolutions.UIEntityHelper;
using System.Windows;
using UIMessager.Services.Message;
using Shared.UI.Countries;
using System.Globalization;
using UI;

namespace Shared.Services
{
    public class CountryService : TService<UI_Country>
    {
        public List<UI_Country> GetAll()
        {
            try
            {
                List<DBServices.PERSONAL.Country> objList = DBServices.PERSONAL.Country.GetAll();
                if (objList == null) return null;
                else
                {
                    var c = UIEntityListConvertor.Convert<DBServices.PERSONAL.Country, UI_Country>(objList);
                    return (System.Threading.Thread.CurrentThread.CurrentUICulture.CompareInfo == new CultureInfo("en-US").CompareInfo) ?
                    c.OrderBy(i => i.Name_EN).ToList():
                    c.OrderBy(i => i.Name_RU).ToList();
                }
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        /// <summary>
        /// Получение списка стран формата UIItem + строка "Не выбрано" 
        /// </summary>
        public List<UI_Item<UI_Country>> GetAll_Items_WithNonSelectItem()
        {
            List<UI_Country> list = GetAll();
            if (list == null) list = new List<UI_Country>();

            return new UI_ItemCollection<UI_Country>(list, false, true);
        }

        /// <summary>
        /// Получение страны по id для чтения
        /// </summary>
        public UI_Country GetByIdForRead(int id)
        {
            try
            {
                var obj = DBServices.PERSONAL.Country.GetById(id);
                UI_Country result = new UI_Country(obj);
                return result;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public UI_Country CreateNew()
        {
            UI_Country country = new UI_Country();
            return country;
        }

        public UI_Country GetById(int id)
        {
            try
            {
                return new UI_Country(id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public bool LoadChangeInfo(UI_Country obj)
        {
            try
            {
                obj.LoadChangeInfo();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }      

        /// <summary>
        /// Проверка, есть ли у страны связи?
        /// </summary>    
        public bool CountryUsedInSettingsAndPersonCards(UI_Country country)
        {
            try
            {
                return country.GetEntity().CountryUsedInSettingsAndPersonCards();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        // можно удалять экспортированные на судно, так как на судне нельзя редактировать ЛК и настройки судна, связанные со страной!
        public bool DeleteIfNotUsed(UI_Country country)
        {         
            if (CountryUsedInSettingsAndPersonCards(country))
            {
                // нельзя удалять
                MsgInformation.Show(Properties.Resources.mCantDelUsedCountry);
                return false;
            }
            if (MsgConfirm.Show(Properties.Resources.mConfirmDelCountry) == System.Windows.MessageBoxResult.No) return false;
            return this.Delete(country);
        }


    }
}
