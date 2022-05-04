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
        [HarmonyPatch(typeof(Mission), "DeliverGood")]
#if DEBUG
        [HarmonyDebug]
#endif      
         
        public static class DeliverGoodPatch
        {            
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                int insertionIndex = -1;

                for (int i = 0; i < codes.Count - 1; i++)
                {
                    if (codes[i].opcode == OpCodes.Add && codes[i + 1].operand.ToString() == "System.Int32 gold")
                    {
                        insertionIndex = i;
#if DEBUG
                        FileLog.Log("Previous opcode is " + codes[i - 1].opcode);
                        FileLog.Log("Found insertion index of " + insertionIndex);
                        FileLog.Log("Next opcode is " + codes[i + 1].opcode);
#endif
                        break;
                    }                    
                }

                var instructionsToInsert = new List<CodeInstruction>();
#if DEBUG
                FileLog.Log("Creating insertion list.");
                FileLog.Log("Creating first instruction.");
                FileLog.Log("MissionPayMultiplier is " + (int)Main.settings.MissionPayMultiplier);
#endif
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4, (int)Main.settings.MissionPayMultiplier));
#if DEBUG
                FileLog.Log("Creating second instruction.");
#endif
                instructionsToInsert.Add(new CodeInstruction(OpCodes.Mul));
#if DEBUG
                FileLog.Log("Created insertion list.");
#endif
                if (insertionIndex != -1)
                {
#if DEBUG
                    FileLog.Log("Running insertion.");
#endif
                    codes.InsertRange(insertionIndex, instructionsToInsert);
#if DEBUG
                    FileLog.Log("Ran insertion.");
#endif
                }

                return codes;                
            }
        }
    }
}
