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
	public class JobDriver_BuildSpiderLair : JobDriver
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

			Toil toil2 = Toils_General.Wait(1200, TargetIndex.None);
			toil2.FailOnDespawnedOrNull(TargetIndex.A);
			toil2.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);			
			toil2.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);			
			yield return toil2;

			Toil toil3 = new Toil();
			toil3.initAction = delegate
			{
				Thing lair = ThingMaker.MakeThing(ThingDef.Named("VAECaves_SpiderLair"), null);
				GenSpawn.Spawn(lair, job.targetA.Cell, pawn.Map, WipeMode.FullRefund);
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;


			
		}



	}
}