using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Shared.DateSpan
{
    public interface IDateSpan
    {
        int? Value_inDays { get; set; }
        int? Value_inMonths { get; set; }
        int? Value_inYears { get; set; }

    }

    
}
