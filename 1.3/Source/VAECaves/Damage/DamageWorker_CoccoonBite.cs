using RimWorld;
using System;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAECaves
{
    public class DamageWorker_CoccoonBite : DamageWorker_AddInjury
    {
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
           
            Pawn pawn = victim as Pawn;
            if (pawn != null)
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("VAE_SilkCovered"));
                if (pawn.stances.stunner.Stunned || hediff != null)
                {
                    TryCoverPawn(pawn);
                    
                } 
            }

            DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);


            return damageResult;
        }

        public void TryCoverPawn(Pawn pawn)
        {
            Thing coccoon = ThingMaker.MakeThing(ThingDef.Named("VAECaves_Coccoon"), null);
            GenSpawn.Spawn(coccoon, pawn.Position, pawn.Map, WipeMode.FullRefund);
            Building_Coccoon coccoonAsSarcophagus = coccoon as Building_Coccoon;
            coccoonAsSarcophagus.TryAcceptThing(pawn);
        }
    }

    
}

