using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DBServices;

namespace Shared
{
    public class EntityItemManager
    {
        /// <summary>
        /// Формирование списка сущностей (типа EntityItem): All (по указанию), NoneType (noneTypeName), <сущности>
        /// </summary>
        public static ObservableCollection<EntityItem<T>> GenerateEntityItemList<T>(List<T> entityList, bool includeAllItem, string noneTypeName = null) where T : dbEntity
        {
            return GenerateEntityItemList<T>(entityList, includeAllItem, true, noneTypeName) ;
        }
        
        /// <summary>
        /// Формирование списка сущностей (типа EntityItem): All (по указанию), NoneType (по указанию) с именем noneTypeName <сущности>
        /// </summary>
        public static ObservableCollection<EntityItem<T>> GenerateEntityItemList<T>(List<T> entityList, bool includeAllItem, bool includeNotSelectItem, string noneTypeName = null) where T : dbEntity
        {
            ObservableCollection<EntityItem<T>> ItemList = new ObservableCollection<EntityItem<T>>();
            if (includeAllItem)
            {
                EntityItem<T> All = new EntityItem<T>(TypeItem.All, global::Resources.Properties.Resources.txtAll);
                ItemList.Add(All);
            }
            if (includeNotSelectItem)
            {
                if (noneTypeName == null) noneTypeName = global::Resources.Properties.Resources.txtNotSelected;
                EntityItem<T> Empty = new EntityItem<T>(TypeItem.None, noneTypeName);
                ItemList.Add(Empty);
            }

            foreach (var item in entityList) ItemList.Add(new EntityItem<T>(item));
            return ItemList;
        }
    }
}
