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

            IEnumerable<IntVec3> tmpTerrain = map.AllCells.InRandomOrder();

            if (monsterKindDef != null)
            {
               
                foreach (VAEQuestMapSpawnsDef element in DefDatabase<VAEQuestMapSpawnsDef>.AllDefs.Where(element => element.associatedMonster == this.monsterKindDef))
                {
                    bool canSpawn = true;
                    if (spawnCounter == 0)
                    {
                        if (element.numberToSpawnRandom != IntRange.zero)
                        {
                            spawnCounter = element.numberToSpawnRandom.RandomInRange;
                        }
                        else
                        {
                            spawnCounter = element.numberToSpawn;

                        }
                    }
                    foreach (IntVec3 c in tmpTerrain)
                    {

                        TerrainDef terrain = c.GetTerrain(map);


                        foreach (string notAllowed in element.terrainValidationDisallowed)
                        {
                            if (terrain.defName == notAllowed)
                            {
                                canSpawn = false;
                                break;
                            }
                        }

                        if (canSpawn)
                        {
                            if (element.isPawn) {


                                PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named(element.thingDef.defName), null, PawnGenerationContext.NonPlayer);
                                Pawn spider = PawnGenerator.GeneratePawn(request);
                                GenSpawn.Spawn(spider, c,map, Rot4.South, WipeMode.Vanish, false);
                                if (element.ifPawnGoManhunter)
                                {

                                    spider.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false);

                                }
                                spawnCounter--;


                            } else {
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

    }
}
