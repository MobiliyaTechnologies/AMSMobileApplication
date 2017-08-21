using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Utilities
{
    public class Utility
    {
       
        public static string GetAccessTokenfromQueryString(string url, string param)
        {
            try
            {
                Dictionary<string, string> dicQueryString =
                      url.Split('?')[1].Split('&')
                           .ToDictionary(c => c.Split('=')[0],
                                         c => Uri.UnescapeDataString(c.Split('=')[1]));
                return dicQueryString[param];
            }
            catch (Exception)
            {
                Dictionary<string, string> dicQueryString =
                                      url.Split('#')[1].Split('&')
                                           .ToDictionary(c => c.Split('=')[0],
                                                         c => Uri.UnescapeDataString(c.Split('=')[1]));
                return dicQueryString[param];
            }

        }

        public static double GetTimezoneDifference()
        {
            System.TimeZoneInfo localZone = System.TimeZoneInfo.Local;
            TimeSpan localTimeOffset = localZone.GetUtcOffset(DateTime.Now);
            return localTimeOffset.TotalHours;
        }

    }
}
