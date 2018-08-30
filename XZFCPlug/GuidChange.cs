using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class GuidChange
    {

        public static string Change2Without_(string input)
        {
            Guid newG = Guid.Parse(input);
            return newG.ToString("N").ToUpper();
        }
        public static string Change2With_(string input)
        {
            Guid newG = Guid.Parse(input);
            return newG.ToString("D").ToUpper();
        }
    }
}
