using RimWorld;

using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;


namespace VAECaves
{
    public class Hediff_WallBreaker : HediffWithComps
    {

        private System.Random random = new System.Random();

        private int tickCounter = 0;

        private int filthCounter = 0;
     
        private int spawnTick = 0;
        private int leftFadeOutTicks = -1;

        private List<Thing> tmpThings = new List<Thing>();

        private float FadeInOutFactor
        {
            get
            {
                float a = Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.spawnTick) / 120f);
                float b = (this.leftFadeOutTicks >= 0) ? Mathf.Min((float)this.leftFadeOutTicks / 120f, 1f) : 1f;
                return Mathf.Min(a, b);
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (pawn.Spawned)
            {
               

                tickCounter++;
                if (tickCounter > 30)
                {
                    if (!pawn.Downed)
                    {
                        this.DamageCloseThings();
                        tickCounter = 0;
                    }

                }

                filthCounter++;
                if (filthCounter > 130)
                {
                    if (!pawn.Downed)
                    {
                        this.RandomFilthGenerator();
                        filthCounter = 0;
                    }

                }
            }

        }



     

        private void RandomFilthGenerator()
        {
            int num = GenRadial.NumCellsInRadius(1f);
            for (int i = 0; i < num; i++)
            {
                if (random.NextDouble() > 0.8)
                {
                    IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
                    if (pawn.Map != null)
                    {
                        if (intVec.InBounds(pawn.Map))
                        {

                            Thing thing = ThingMaker.MakeThing(ThingDef.Named("Filth_RubbleRock"), null);

                            GenSpawn.Spawn(thing, intVec, pawn.Map);
                        }
                    }
                }

            }

        }

        private void DamageCloseThings()
        {
            int num = GenRadial.NumCellsInRadius(2f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
                if (pawn.Map != null)
                {
                    if (intVec.InBounds(pawn.Map) && this.CellCanBeAffected(intVec))
                    {
                       
                            this.DoDamage(intVec, 1);
                       
                    }
                }
            }
        }

        private bool CellCanBeAffected(IntVec3 c)
        {
            if (c.Roofed(pawn.Map) && c.GetRoof(pawn.Map).isThickRoof)
            {
                return false;
            }
            Building edifice = c.GetEdifice(pawn.Map);
            bool flagAffected = false;
            if (edifice != null)
            {
                if (edifice.def.category == ThingCategory.Building && (edifice.def == ThingDefOf.Wall || edifice.def == ThingDefOf.Door))
                {
                    flagAffected = true;
                }
            }

            return flagAffected;
        }

        private void DoDamage(IntVec3 c, float damageFactor)
        {
            this.tmpThings.Clear();
            this.tmpThings.AddRange(c.GetThingList(pawn.Map));
            Vector3 vector = c.ToVector3Shifted();
            Vector2 b = new Vector2(vector.x, vector.z);
          
            for (int i = 0; i < this.tmpThings.Count; i++)
            {
                if (tmpThings[i] != this.pawn)
                {
                    BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
                    switch (this.tmpThings[i].def.category)
                    {
                       
                        case ThingCategory.Building:
                            damageFactor *= 1f;
                            break;
                        default:
                            damageFactor *= 0f;
                            break;

                    }
                    int num2 = Mathf.Max(GenMath.RoundRandom(30f * damageFactor), 1);
                    Thing thingtoHurt = this.tmpThings[i];
                    DamageDef boulderScratch = DamageDefOf.Blunt; 
                    float amount = (float)num2;
                    //float angle = num;

                    thingtoHurt.TakeDamage(new DamageInfo(boulderScratch, amount, 0f, 0, pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);


                }
            }
            this.tmpThings.Clear();
        }

    }
}
