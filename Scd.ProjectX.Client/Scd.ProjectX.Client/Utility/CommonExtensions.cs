using Scd.ProjectX.Client.Models.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scd.ProjectX.Client.Utility
{
    public static class CommonExtensions
    {
        public static Candle ToCandle(this Candle bar)
        {
            return new Candle();
        } 
    }
}
