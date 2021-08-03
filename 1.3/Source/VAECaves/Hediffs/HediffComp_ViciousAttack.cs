using RimWorld;
using Verse;
using Verse.Sound;
using System;

namespace VAECaves
{
    public class HediffComp_ViciousAttack : HediffComp
    {


        public HediffCompProperties_ViciousAttack Props
        {
            get
            {
                return (HediffCompProperties_ViciousAttack)this.props;
            }
        }

        public override void Notify_PawnDied()
        {
            
            Map map = this.parent.pawn.Corpse.Map;
            if (map != null)
            {
                foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse.Map, 10, true))
                {
                    Pawn pawn = thing as Pawn;
                    if (pawn != null && pawn.def.defName == "VAECaves_InsectoidHulk")
                    {
                        pawn.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("VAECaves_Bloodlust", true), null, true, false, null, false);
                        Messages.Message("VAE_HulkBloodlust".Translate(), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }

                

            }

        }


    }
}
