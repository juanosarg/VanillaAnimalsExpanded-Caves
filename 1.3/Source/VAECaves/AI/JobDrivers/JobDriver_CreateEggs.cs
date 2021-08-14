using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
using RimWorld;
using RimWorld.Planet;
using System.Linq;

namespace VAECaves
{
	public class JobDriver_CreateEggs : JobDriver
	{
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return toil;

			Toil toil2 = Toils_General.Wait(250, TargetIndex.None);
			toil2.FailOnDespawnedOrNull(TargetIndex.A);
			toil2.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil2.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil2;

			Toil toil3 = new Toil();
			toil3.initAction = delegate
			{

				System.Random rand = new System.Random();
				int numberOfEggs = rand.Next(2);

				for (int i = 0; i <= numberOfEggs; i++)
				{
					IntVec3 c;
					CellFinder.TryFindRandomReachableCellNear(job.targetA.Cell, pawn.Map, 3, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => IsGoodEggCell(x, pawn) == true, null, out c, 999999);
					if (c.IsValid)
					{
						Thing eggs = ThingMaker.MakeThing(ThingDef.Named("VAECaves_SpiderEggs"), null);
						GenSpawn.Spawn(eggs, c, pawn.Map, WipeMode.FullRefund);
					}
				}

				
				
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;



		}

		private bool IsGoodEggCell(IntVec3 c, Pawn pawn)
		{
			RoofDef roofDef = pawn.Map.roofGrid.RoofAt(c);
			if (roofDef == null || !roofDef.isThickRoof)
			{

				return false;
			}			
			if (!c.Standable(pawn.Map))
			{
				return false;
			}
			if (c.GetEdifice(pawn.Map) != null) { return false; }
			
			


			return true;
		}



	}
}