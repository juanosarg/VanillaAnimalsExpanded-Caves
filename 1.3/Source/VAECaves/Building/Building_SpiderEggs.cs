﻿using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

using Verse.Sound;
using UnityEngine;

namespace VAECaves
{
    public class Building_SpiderEggs : Building
    {
        public int tickCounter = 0;
        public const int totalTicks = 60000;
       
        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);
            if(dinfo.Instigator.Faction!=null && dinfo.Instigator.Faction.IsPlayer)
            {
                List<Pawn> pawnsaffected = (from x in this.Map.mapPawns.AllPawnsSpawned
                                            where x.kindDef == PawnKindDef.Named("VAECaves_AncientGiantSpider")
                                            select x).ToList();
                foreach (Pawn pawnaffected in pawnsaffected)
                {
                    pawnaffected.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            tickCounter++;
            if (tickCounter > totalTicks)
            {
                System.Random rand = new System.Random();
                int numberOfHatchlings = rand.Next(4);
               
                for (int i = 0; i <= numberOfHatchlings; i++)
                {
                    PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named("VAECaves_GiantSpiderHatchling"), null, PawnGenerationContext.NonPlayer);
                    Pawn spider = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(spider, this.Position, this.Map, Rot4.South, WipeMode.Vanish, false);
                  
                }
                this.DeSpawn();
            }
        }
        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();
         
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);


        }

        public override string GetInspectString()
        {
            
            return "VAE_EggsHatchIn".Translate((totalTicks-tickCounter).ToStringTicksToPeriod(true, false, true, true));
            


        }

    }
}
