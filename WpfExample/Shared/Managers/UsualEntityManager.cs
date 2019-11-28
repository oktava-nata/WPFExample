using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DBServices;
using BaseLib;
using UIMessager.Services.Message;
using Shared.Forms;
using DBServices.UsualEntity;

namespace Shared
{
    public static class UsualEntityManager
    {
        #region Ship

        /// <summary>
        /// Получение списка судов (типа EntityItem)
        /// </summary>
        public static ObservableCollection<EntityItem<Ship>> GetShipItems(bool includeAllItem)
        {
            List<Ship> ships = Shared.ShipManager.GetShipList().ToList();
            if (ships == null) return null;
            return EntityItemManager.GenerateEntityItemList<Ship>(ships, includeAllItem);
        }

        #endregion


    }
}
