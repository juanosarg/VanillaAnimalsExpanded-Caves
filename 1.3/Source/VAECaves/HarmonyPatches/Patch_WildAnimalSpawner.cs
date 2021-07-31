using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace VAECaves
{

    public static class Patch_WildAnimalSpawner
    {

        public static class manual_SpawnWildAnimalAt_predicate
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var seasonAcceptableFor = AccessTools.Method(typeof(MapTemperature), nameof(MapTemperature.SeasonAcceptableFor));

                var mapInfo = AccessTools.Field(typeof(WildAnimalSpawner), "map");

                var eligibleToSpawnInfo = AccessTools.Method(typeof(manual_SpawnWildAnimalAt_predicate), nameof(EligibleToSpawn));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand == seasonAcceptableFor)
                    {
                        yield return instruction; // this.map.mapTemperature.SeasonAcceptableFor(a.race)
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // a
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldfld, mapInfo); // this.map
                        instruction = new CodeInstruction(OpCodes.Call, eligibleToSpawnInfo); // EligibleToSpawn(this.map.mapTemperature.SeasonAcceptableFor(a.race), a, this.map)
                    }

                    yield return instruction;
                }
            }



            private static bool EligibleToSpawn(bool original, PawnKindDef pawnkind, Map map)
            {
                
                if (original)
                {
                    
                    if (pawnkind!=null && pawnkind.defName.Contains("VAECaves_"))
                    {
                        if (!Find.World.HasCaves(map.Tile))
                        {
                            return false;
                        }
                       
                    }
                }
                return original;
            }

        }

    }

}
