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
#if DEBUG
                FileLog.Log("================================================");
                FileLog.Log("GenerateMissions opcodes");
                FileLog.Log("================================================");
#endif

                for (int i = 0; i < codes.Count; i++)
                {
#if DEBUG
                    FileLog.Log("codes[" + i + "].opcode: " + codes[i].opcode + ", codes[" + i + "].operand: " + codes[i].operand);
#endif                                                            

                    if (codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Int32 RoundToInt(Single)")
                    {                        
                        insertionIndex = i + 5;
#if DEBUG
                        FileLog.Log("Found GenerateMissions insertion index");
#endif                        
                        break;
                    }
                }

                var instructionsToInsert = new List<CodeInstruction>();

#if BUILD_BEPINEX
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, MissionMultipliersMain.instance.cargoMissionPayMultiplier.Value));  //Ldc_R4 for float
#endif
#if BUILD_UMM
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, (int)Main.settings.MissionPayMultiplier));  //Ldc_R4 for float
#endif
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Mul));

                if (insertionIndex != -1)
                {
                    codes.InsertRange(insertionIndex, instructionsToInsert);

                }

#if DEBUG
                FileLog.Log("================================================");
                FileLog.Log("List of CargoMission opcodes after insertion");
                FileLog.Log("================================================");
                for (int i = 0; i < codes.Count; i++)
                    FileLog.Log("codes[" + i + "].opcode: " + codes[i].opcode + ", codes[" + i + "].operand: " + codes[i].operand);
#endif

                return codes;
            }
        }

        [HarmonyPatch(typeof(IslandMissionOffice), "GenerateMailMission")]

        public static class GenerateMailMissionPatch
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
#if DEBUG
                FileLog.Log("================================================");
                FileLog.Log("GenerateMailMission opcodes");
                FileLog.Log("================================================");
#endif

                for (int i = 0; i < codes.Count; i++)
                {
#if DEBUG
                    FileLog.Log("codes[" + i + "].opcode: " + codes[i].opcode + ", codes[" + i + "].operand: " + codes[i].operand);
#endif                                                            

                    if (codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Int32 RoundToInt(Single)")
                    {
                        insertionIndex = i+1;
#if DEBUG
                        FileLog.Log("Found MailMission insertion index");
#endif                        
                        break;
                    }
                }

                var instructionsToInsert = new List<CodeInstruction>();

#if BUILD_BEPINEX
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, MissionMultipliersMain.instance.mailMissionPayMultiplier.Value));  //Ldc_R4 for float
#endif
#if BUILD_UMM
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, (int)Main.settings.MissionPayMultiplier));  //Ldc_R4 for float
#endif
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Mul));

                if (insertionIndex != -1)
                {
                    codes.InsertRange(insertionIndex, instructionsToInsert);

                }

#if DEBUG
                FileLog.Log("================================================");
                FileLog.Log("List of MailMission opcodes after insertion");
                FileLog.Log("================================================");
                for (int i = 0; i < codes.Count; i++)
                    FileLog.Log("codes[" + i + "].opcode: " + codes[i].opcode + ", codes[" + i + "].operand: " + codes[i].operand);                
#endif
                return codes;
            }

        }
    }
}
