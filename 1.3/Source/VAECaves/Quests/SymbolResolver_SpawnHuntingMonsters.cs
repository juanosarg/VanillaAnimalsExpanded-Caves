using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.BaseGen;
using Verse.AI.Group;
using Verse;
using RimWorld;

namespace VAECaves
{
    public class SymbolResolver_SpawnHuntingMonsters : SymbolResolver
    {

        public override void Resolve(ResolveParams rp)
        {
            ResolveParams canReachEdgeParams = rp;
            BaseGen.symbolStack.Push("ensureCanReachMapEdge", canReachEdgeParams);

            CellRect rect = rp.rect;
            //Chance to use the right half of the rect
            if (Rand.Chance(0.5f))
            {
                rp.rect = new CellRect(rect.minX + (rect.Width / 2), rect.minZ, rect.Width / 2, rect.Height);
            }
            else
            {
                rp.rect = new CellRect(rect.minX, rect.minZ, rect.Width / 2, rect.Height);
            }

            int spawnCount;
            PawnKindDef huntingTarget = rp.singlePawnKindDef;

            if (huntingTarget.wildGroupSize == null)
            {
                spawnCount = 1;

            }
            else
            {
                spawnCount = Rand.RangeInclusive(huntingTarget.wildGroupSize.min, huntingTarget.wildGroupSize.max);

            }



            for (int i = 0; i < spawnCount; i++)
            {
                ResolveParams resolveParams = rp;
                Faction faction = Faction.OfInsects;
                Pawn pawnToSpawn = PawnGenerator.GeneratePawn(huntingTarget, faction);

                Lord lord = resolveParams.singlePawnLord;
                if (lord == null)
                {
                    Map map = BaseGen.globalSettings.map;
                    if (map.GetComponent<HuntQuest_MapComponent>() == null)
                    {
                        map.components.Add(new HuntQuest_MapComponent(map));
                        map.GetComponent<HuntQuest_MapComponent>().monsterKindDef = huntingTarget;
                    }
                    else { map.GetComponent<HuntQuest_MapComponent>().monsterKindDef = huntingTarget; }
                    map.GetComponent<HuntQuest_MapComponent>().threatPoints = rp.threatPoints;
                    IntVec3 point;
                    LordJob lordJob;
                    if (Rand.Bool && (from x in rp.rect.Cells
                                      where !x.Impassable(map)
                                      select x).TryRandomElement(out point))
                    {
                        lordJob = new LordJob_LongRanged(point);
                    }
                    else
                    {
                        lordJob = new LordJob_AssaultColony(faction, false, false, false, false, false);
                    }
                    lord = LordMaker.MakeNewLord(faction, lordJob, map, null);

                }
               

                resolveParams.singlePawnLord = lord;
                resolveParams.faction = faction;
                resolveParams.singlePawnToSpawn = pawnToSpawn;

                BaseGen.symbolStack.Push("pawn", resolveParams);
            }
        }
    }
}
