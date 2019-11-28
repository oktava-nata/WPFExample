using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DBServices.UsualEntity;

namespace Shared.UIViewRules
{
    /// <summary>
    /// Сортировщик судов. Сортировка по порядковому номеру судна, в случае одинаковых номеров - по названию судна
    /// </summary>
    public class ShipComparer : MVVMHelper.Extensions.ICustomComparer
    {
        public ListSortDirection SortDirection { get; set; }

        public int Compare(object x, object y)
        {
            Ship ship1 = x as Ship;
            Ship ship2 = y as Ship;
            int result = ship1.SortNumber.CompareTo(ship2.SortNumber);
            if (result == 0)
                result =  ship1.Name.CompareTo(ship2.Name);
            return (SortDirection == ListSortDirection.Ascending) ? result : result * -1;
        }
    }
}
