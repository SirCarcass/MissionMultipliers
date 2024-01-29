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
    public static class MissionPatches
    {
        [HarmonyPatch(typeof(Mission), "GetDeliveryRep")]
#if DEBUG
        [HarmonyDebug]
#endif  
        public static class GetDeliveryRepPatch
        {
            [HarmonyPostfix]
#if BUILD_UMM
            public static void Postfix(ref int __result)
            {
                if (!Main.enabled) return;

                __result = __result * (int)Main.settings.MissionRepMultiplier;


                return;
            }
#endif

#if BUILD_BEPINEX
            public static void Postfix(ref int __result)
            {                
                __result = __result * MissionMultipliersMain.instance.missionRepMultiplier.Value;

                return;
            }
#endif
        }
    }
}
