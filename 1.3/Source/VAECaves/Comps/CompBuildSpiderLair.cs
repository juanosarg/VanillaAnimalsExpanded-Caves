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
            if (this.parent.IsHashIntervalTick(Props.checkingTime))
            {
                Pawn pawn = this.parent as Pawn;
                if (this.parent.Map.GetComponent<CoccoonsAndSpiderLairs_MapComponent>().spiderLairsInMap.Count == 0
                    && pawn.CurJob.def != DefDatabase<JobDef>.GetNamed("VAECaves_BuildSpiderLair"))
                {
					
					IntVec3 c;
					CellFinder.TryFindRandomReachableCellNear(pawn.Position, pawn.Map, 500, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => IsGoodLairCell(x,pawn)==true, null, out c, 999999);
					
					if (c.IsValid)
					{
						Job job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("VAECaves_BuildSpiderLair"),c);
						pawn.jobs.TryTakeOrderedJob(job);
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

		/*private static IntVec3 TryFindLairBuildCell(Pawn pawn)
		{
			Region rootReg;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), (Region r) => !r.IsForbiddenEntirely(pawn), 100, out rootReg, RegionType.Set_Passable))
			{				
				return IntVec3.Invalid;
			}
			IntVec3 result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.District == rootReg.District, delegate (Region r)
			{
				for (int i = 0; i < r.CellCount; i++)
				{
					IntVec3 randomCell = r.RandomCell;
					if (IsGoodLairCell(randomCell, pawn))
					{
						result = randomCell;
						return true;
					}
				}
				return false;
			}, 30, RegionType.Set_Passable);
			return result;
		}*/




		/*private static bool IsGoodLairCell(IntVec3 c, Pawn pawn)
		{
			RoofDef roofDef = pawn.Map.roofGrid.RoofAt(c);
			if (roofDef == null || !roofDef.isThickRoof)
			{
				
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (c.GetEdifice(pawn.Map) != null)
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
		}*/





	}
}
