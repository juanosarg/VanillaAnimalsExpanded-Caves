using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace VAECaves
{
    public class CompBuildSpiderLair : ThingComp
    {


        public CompProperties_BuildSpiderLair Props
        {
            get
            {
                return (CompProperties_BuildSpiderLair)this.props;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.Map!=null&&this.parent.IsHashIntervalTick(Props.checkingTime))
            {
                Pawn pawn = this.parent as Pawn;
				if (pawn!=null &&!pawn.Dead && !pawn.Downed) {
					if (this.parent.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>().spiderLairsInMap.Count == 0
						&& pawn.CurJob.def != DefDatabase<JobDef>.GetNamed("VAECaves_BuildSpiderLair"))
					{

						IntVec3 c;
						CellFinder.TryFindRandomReachableCellNear(pawn.Position, pawn.Map, 500, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => IsGoodLairCell(x, pawn) == true, null, out c, 999999);

						if (c.IsValid)
						{
							Job job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("VAECaves_BuildSpiderLair"), c);
							pawn.jobs.TryTakeOrderedJob(job);
						}

					}

				}
                
            }
			if (this.parent.Map != null && this.parent.IsHashIntervalTick(Props.checkingTimeForEggs))
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && !pawn.Dead && !pawn.Downed)
				{
					if (this.parent.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>().spiderLairsInMap.Count > 0
						&& pawn.CurJob.def != DefDatabase<JobDef>.GetNamed("VAECaves_BuildSpiderLair")
						&& pawn.CurJob.def != DefDatabase<JobDef>.GetNamed("VAECaves_CreateEggs")
						&& pawn.CurJob.def != JobDefOf.AttackMelee)
					{

						IntVec3 c = pawn.Map.listerThings.ThingsOfDef(ThingDef.Named("VAECaves_SpiderLair")).RandomElement().Position;

						if (c.IsValid)
						{
							Job job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("VAECaves_CreateEggs"), c);
							pawn.jobs.TryTakeOrderedJob(job);
						}

					}
				}
					
			}
		}

		private bool IsGoodLairCell(IntVec3 c, Pawn pawn)
		{
			RoofDef roofDef = pawn.Map.roofGrid.RoofAt(c);
			if (roofDef == null || !roofDef.isThickRoof)
			{

				return false;
			}

			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (!c2.InBounds(pawn.Map))
				{
					return false;
				}
				if (!c2.Standable(pawn.Map))
				{
					return false;
				}
				if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
				{
					return false;
				}
			}


			return true;
		}

	



	}
}
