using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimWorld.Planet;

namespace VAECaves
{
    public class HuntQuest_MapComponent : MapComponent
    {
        public PawnKindDef monsterKindDef = null;
        public float? threatPoints = 0;
        public bool verifyFirstTime = true;
        public int spawnCounter = 0;

        public HuntQuest_MapComponent(Map map) : base(map)
        {

        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.verifyFirstTime, "verifyFirstTime", true, true);


        }

        public override void FinalizeInit()
        {

            base.FinalizeInit();

            if (verifyFirstTime)
            {
                this.doMapSpawns();


            }

        }

        public void doMapSpawns()
        {


            if (monsterKindDef != null)
            {

                foreach (VAEQuestMapSpawnsDef element in DefDatabase<VAEQuestMapSpawnsDef>.AllDefs.Where(element => element.associatedMonster == this.monsterKindDef))
                {

                    IEnumerable<IntVec3> tmpTerrain = map.AllCells.InRandomOrder();


                    bool canSpawn = true;
                    if (spawnCounter == 0)
                    {
                        spawnCounter = Rand.RangeInclusive(element.numberToSpawn.min, element.numberToSpawn.max);
                        if (threatPoints != 0)
                        {
                            spawnCounter *= (int)MultiplierByThreat(threatPoints);
                        }

                    }
                    foreach (IntVec3 c in tmpTerrain)
                    {

                        TerrainDef terrain = c.GetTerrain(map);



                        bool flagDisallowed = true;
                        foreach (string notAllowed in element.terrainValidationDisallowed)
                        {
                            if (terrain.HasTag(notAllowed))
                            {
                                flagDisallowed = false;
                                break;
                            }
                        }
                        bool flagWater = true;
                        if (!element.allowOnWater && terrain.IsWater)
                        {
                            flagWater = false;

                        }

                        canSpawn = flagDisallowed & flagWater;
                        if (canSpawn)
                        {
                            if (element.isPawn)
                            {
                                PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named(element.thingDef.defName), null, PawnGenerationContext.NonPlayer);
                                Pawn spider = PawnGenerator.GeneratePawn(request);
                                GenSpawn.Spawn(spider, c, map, Rot4.South, WipeMode.Vanish, false);
                                if (element.ifPawnGoManhunter)
                                {
                                    spider.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false);
                                }
                                spawnCounter--;
                            }
                            else
                            {
                                Thing thing = (Thing)ThingMaker.MakeThing(element.thingDef, null);
                                CellRect occupiedRect = GenAdj.OccupiedRect(c, thing.Rotation, thing.def.Size);
                                if (occupiedRect.InBounds(map))
                                {
                                    GenSpawn.Spawn(thing, c, map);
                                    spawnCounter--;
                                }
                            }

                        }
                        if (canSpawn && spawnCounter <= 0)
                        {
                            
                            spawnCounter = 0;
                            break;
                        }
                    }
                }
            }
            this.verifyFirstTime = false;
        }

        public float MultiplierByThreat(float? threat)
        {
            if (threat < 1000)
            {
                return 1f;
            } if(threat < 5000)
            {
                return 1.5f;
            }
            if (threat < 10000)
            {
                return 2f;
            }
            else return 2.5f;

        }
    }
}
