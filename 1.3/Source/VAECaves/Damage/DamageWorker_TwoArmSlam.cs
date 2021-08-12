using RimWorld;
using System;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAECaves
{
    public class DamageWorker_TwoArmSlam : DamageWorker_AddInjury
    {
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            System.Random rand = new System.Random();
            Pawn pawn = victim as Pawn;
            if (pawn != null)
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("VAE_StunnedByViciousAttack"));
                if (hediff != null)
                {
                    if (rand.NextDouble() < 0.15)
                    {
                        int legOrArm = rand.Next(4);
                        string customLabel = "";
                        switch (legOrArm)
                        {
                            case 0:
                                customLabel = "left arm";
                                break;
                            case 1:
                                customLabel = "right arm";
                                break;
                            case 2:
                                customLabel = "right leg";
                                break;
                            case 3:
                                customLabel = "left leg";
                                break;
                            default:
                                customLabel = "right leg";
                                break;

                        }
                        BodyPartRecord bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).
                                   FirstOrDefault((BodyPartRecord x) => x.customLabel == customLabel);
                        
                        if (bodyPartRecord != null)
                        {
                            int num = (int)pawn.health.hediffSet.GetPartHealth(bodyPartRecord) + 1000;
                            DamageInfo damageInfo = new DamageInfo(DamageDefOf.Cut, (float)num, 999f, -1f, dinfo.Instigator, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
                            damageInfo.SetAllowDamagePropagation(false);
                            victim.TakeDamage(damageInfo);
                            if (pawn.Faction != null && pawn.Faction.IsPlayer) {
                                Messages.Message("VAE_HulkRips".Translate(pawn.LabelCap, bodyPartRecord.customLabel), pawn, MessageTypeDefOf.NegativeEvent);

                            }
                        }
                    }
                }
            }

            DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);


            return damageResult;
        }


    }
}

