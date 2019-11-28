using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMBaseSolutions.CheckedItems
{
    public class ListCheckedItemViewModel<T> : List<CheckedItemViewModel<T>>
    {
        public delegate bool SetChecked(T item);
        public Action OnCheckedChanged { set { ForEach(i => i.OnCheckedChanged += value); } }

        public int Selected { get { return this.Where(i => i.IsChecked).Count(); } }
        public int All { get { return this.Count(); } }

        public ListCheckedItemViewModel(List<T> list, bool allisChecked, Action onCheckedChanged = null)
        {
            if (list != null)
                foreach (var item in list)
                {
                    CheckedItemViewModel<T> itemVM = new CheckedItemViewModel<T>(item, allisChecked, onCheckedChanged);
                    this.Add(itemVM);
                }
        }

        public ListCheckedItemViewModel(List<T> list, SetChecked setChecked, Action onCheckedChanged = null)
        {
            if (list != null)
                foreach (var item in list)
                {
                    CheckedItemViewModel<T> itemVM = new CheckedItemViewModel<T>(item, setChecked(item), onCheckedChanged);
                    this.Add(itemVM);
                }
        }

        public ListCheckedItemViewModel(List<CheckedItemViewModel<T>> list, bool allisChecked, Action onCheckedChanged = null)
        {
            if (list != null)
                foreach (var item in list)
                {
                    item.IsChecked = allisChecked;
                    item.OnCheckedChanged = onCheckedChanged;
                    this.Add(item);
                }
        }

        public ListCheckedItemViewModel(List<CheckedItemViewModel<T>> list, SetChecked setChecked, Action onCheckedChanged = null)
        {
            if (list != null)
                foreach (var item in list)
                {
                    item.IsChecked = setChecked(item.ItemObject);
                    item.OnCheckedChanged = onCheckedChanged;
                    this.Add(item);
                }
        }

        public void CheckedAll()
        {
            foreach (var item in this)
                item.IsChecked = true;
        }

        public void UncheckedAll()
        {
            foreach (var item in this)
                item.IsChecked = false;
        }

        /// <summary>
        /// Возвращает список выделенных элементорв типа T
        /// </summary>
        /// <returns></returns>
        public List<T> GetSelectedTList()
        {
            return this.Where(i => i.IsChecked).Select(i => i.ItemObject).ToList();
        }

        /// <summary>
        /// Возвращает список не выделенных элементорв типа T
        /// </summary>
        /// <returns></returns>
        public List<T> GetUnSelectedTList()
        {
            return this.Where(i => !i.IsChecked).Select(i => i.ItemObject).ToList();
        }

        /// <summary>
        /// Возвращает список всех элементорв типа T
        /// </summary>
        /// <returns></returns>
        public List<T> GetTList()
        {
            return this.Select(i => i.ItemObject).ToList();
        }
    }
}
