using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EcoUtils
/// </summary>
/// 

namespace eco.utils
{
    public class EcoUtils
    {
        public EcoUtils()
        {
        }
        public static bool isWet(int aId)
        {
            return aId == 8 || aId == 19 || aId == 20 || aId == 21;
        }
    }


}