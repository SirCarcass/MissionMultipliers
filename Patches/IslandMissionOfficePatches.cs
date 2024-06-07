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
using BepInEx.Logging;
using BepInEx;

namespace MissionMultipliers.Patches
{    
    public static class IslandMissionOfficePatches
    {        
        [HarmonyPatch(typeof(IslandMissionOffice), "GenerateMissions")]

        public static class GenerateMissionsPatch
        {            
            [HarmonyTranspiler]
#if DEBUG
            [HarmonyDebug]
#endif            
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {                
                var codes = new List<CodeInstruction>(instructions);                

            //Multiplier insertion
            int insertionIndex = -1;

                for (int i = 0; i < codes.Count; i++)
                {
#if DEBUG
                    FileLog.Log("codes[" + i + "].opcode: " + codes[i].opcode + ", codes[" + i + "].operand: " + codes[i].operand);
#endif                                                            

                    //if (codes[i].opcode == OpCodes.Stloc_S && codes[i].operand.ToString() == "System.Int32 (21)") //old way
                    if (codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Int32 RoundToInt(Single)")
                    {
                        //insertionIndex = i;  //old way
                        insertionIndex = i+5;
#if DEBUG
                        FileLog.Log("Found insertion index");
#endif                        
                        break;
                    }
                }

                var instructionsToInsert = new List<CodeInstruction>();

#if BUILD_BEPINEX
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, MissionMultipliersMain.instance.missionPayMultiplier.Value));  //Ldc_R4 for float
#endif
#if BUILD_UMM
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, (int)Main.settings.MissionPayMultiplier));  //Ldc_R4 for float
#endif
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Mul));

                if (insertionIndex != -1)
                {
                    codes.InsertRange(insertionIndex, instructionsToInsert);

                }

                return codes;
            }
        }
    }
}
