using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class ModuleSettings
    {        
        public static int ReportPeriodMounth { get { return 1; } }

        public enum RecordsCountOnPageVariants { _10 = 10, _20 = 20, _50 = 50, _100 = 100, _500 = 500 };
        public static RecordsCountOnPageVariants RecordsCountOnPageByDefault = RecordsCountOnPageVariants._20;


        public static readonly long MaxLengthFileToRead = 100 * 1024 * 1024;  //100 Mb
    }   
}
