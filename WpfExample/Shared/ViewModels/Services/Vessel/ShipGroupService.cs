using System;
using System.Collections.Generic;
using System.Linq;
using ViewModelBaseSolutions.UIEntityHelper;
using ViewModelBaseSolutions.Services;
using Shared.UI.Vessel;
using UIMessager.Services.Message;
using System.Collections.ObjectModel;
using DBServices.UsualEntity;
using UI;

namespace Shared.ViewModels.Services.Vessel
{
    public class ShipGroupService : TService<UI_ShipGroup>
    {
       public List<UI_ShipGroup> GetAll()
        {
            try
            {
                var list = DBServices.UsualEntity.ShipGroup.GetAll();
                return UIEntityListConvertor.Convert<DBServices.UsualEntity.ShipGroup, UI_ShipGroup>(list);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        //Получение списка групп судов + строка "Не выбрано"
        public static ObservableCollection<DBServices.EntityItem<DBServices.UsualEntity.ShipGroup>> GetShipGroupItems()
        {
            try
            {
                List<DBServices.UsualEntity.ShipGroup> list = DBServices.UsualEntity.ShipGroup.GetAll();
                if (list == null) return null;

                return EntityItemManager.GenerateEntityItemList<DBServices.UsualEntity.ShipGroup>(list, false);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        /// <summary>
        /// Получение списка групп судов формата UIItem + строка "Все" 
        /// </summary>
        public List<UI_Item<UI_ShipGroup>> GetAll_Items_WithAllItem()
        {
            List<UI_ShipGroup> list = GetAll();
            if (list == null) list = new List<UI_ShipGroup>();

            return new UI_ItemCollection<UI_ShipGroup>(list, true, false);
        }

        /// <summary>
        /// Получение списка групп судов формата UIItem + строка "Не выбрано" 
        /// </summary>
        public List<UI_Item<UI_ShipGroup>> GetAll_Items_WithNonSelectItem()
        {
            List<UI_ShipGroup> list = GetAll();
            if (list == null) list = new List<UI_ShipGroup>();

            return new UI_ItemCollection<UI_ShipGroup>(list, false, true);
        }


        public UI_ShipGroup GetByIdForEdit(int id)
        {
            try
            {
                return new UI_ShipGroup(id);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

        public bool HasShips(UI_ShipGroup group)
        {
            try
            {
                return group.HasShips();
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

    }
}
