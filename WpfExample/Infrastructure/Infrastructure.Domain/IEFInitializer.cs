﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain
{
    public interface IEFInitializer
    {
        string GetSqlConnectionStr();

    }
}
