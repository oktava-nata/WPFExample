using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    internal abstract class EnumValue<TEnum> where TEnum : struct
    {
        public TEnum Value { get; set; }

        public int IntValue
        {
            get
            {
                int intValue;
                int.TryParse(DisplayValue, out intValue);
                return intValue;
            }
            set { Value = (TEnum)Enum.Parse(typeof(TEnum), value.ToString()); }
        }

        public abstract string DisplayValue { get; }

        public EnumValue(TEnum value) { Value = value; }  
    }

    internal class RecordsCountOnPage : EnumValue<Shared.ModuleSettings.RecordsCountOnPageVariants> 
    {
        public override string DisplayValue
        {    
            get  { return ((int) Value).ToString(); }
        }

        public RecordsCountOnPage(Shared.ModuleSettings.RecordsCountOnPageVariants count) : base(count) { }

        public static List<RecordsCountOnPage> GetValues()
        {
            List<RecordsCountOnPage> countVariants = new List<RecordsCountOnPage>();
            foreach (Shared.ModuleSettings.RecordsCountOnPageVariants item in Enum.GetValues(typeof(Shared.ModuleSettings.RecordsCountOnPageVariants)))
                countVariants.Add(new RecordsCountOnPage(item));
            return countVariants.OrderBy(i=>i.Value).ToList();
        }  
    }

   
}
