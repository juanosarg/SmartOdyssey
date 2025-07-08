using HarmonyLib;
using Mono.Cecil.Cil;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld.Planet;
using Verse.AI;



namespace SmartOdyssey
{
    [HarmonyPatch(typeof(WorldGenStep_Mutators), "AddMutatorsFromTile")]
    public static class SmartOdyssey_WorldGenStep_Mutators_AddMutatorsFromTile_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var check = AccessTools.Method(typeof(SmartOdyssey_WorldGenStep_Mutators_AddMutatorsFromTile_Patch), "GetNumber");
            var fieldLoaded = AccessTools.Field(typeof(TileMutatorDef), "chanceOnNonLandmarkTile");


            for (var i = 0; i < codes.Count; i++)
            {

                if (codes[i].opcode == OpCodes.Ldfld && codes[i].OperandIs(fieldLoaded))
                {

                    yield return new CodeInstruction(OpCodes.Call, check);

                }
                else yield return codes[i];
            }
        }


        public static float GetNumber(TileMutatorDef item)
        {

            float chance = SmartOdyssey_Mod.settings.mutatorCommonalities.ContainsKey(item.defName) ? SmartOdyssey_Mod.settings.mutatorCommonalities[item.defName] : item.chanceOnNonLandmarkTile;
            return chance * SmartOdyssey_Settings.mutatorMultiplier;
        }





    }



}