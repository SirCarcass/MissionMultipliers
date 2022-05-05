using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using UnityModManagerNet;

namespace MissionMultipliers.Patches
{
    public static class PortPatches
    {
        [HarmonyPatch(typeof(Port), "GetTotalPrice")]
#if DEBUG
        [HarmonyDebug]
#endif  
        public static class GetTotalPricePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result)
            {
                if (!Main.enabled) return;

                __result = __result * (int)Main.settings.MissionPayMultiplier;

                return;
            }
        }
    }
}
