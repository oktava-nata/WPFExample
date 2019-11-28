using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;

namespace Domain.EFContexts
{
    internal static class InfrastructureEFInitializer
    {
        internal class Initializer : IEFInitializer
        {
            public string GetSqlConnectionStr()
            {
                return @""; 
            }
        }

        internal static Initializer Configuration { get; private set; }

        static InfrastructureEFInitializer()
        {
            Configuration = new Initializer();
        }
    }
}
