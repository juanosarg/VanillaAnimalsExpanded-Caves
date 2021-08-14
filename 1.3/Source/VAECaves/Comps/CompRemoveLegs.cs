using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace VAECaves
{
	public class CompRemoveLegs : ThingComp
	{


		public CompProperties_RemoveLegs Props
		{
			get
			{
				return (CompProperties_RemoveLegs)this.props;
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Map != null && this.parent.IsHashIntervalTick(Props.checkInterval))
			{
				ChangeTheGraphic();
			}
				
		}

		public void ChangeTheGraphic()
        {
			Pawn pawn = this.parent as Pawn;

			if (VAECaves_Mod.settings.arachnophobiaMode)
            {
				Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize;
				Graphic_Multi nakedGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(pawn.ageTracker.CurKindLifeStage.bodyGraphicData.texPath + "_Legless", ShaderDatabase.Cutout, vector, Color.white);
				pawn.Drawer.renderer.graphics.nakedGraphic = nakedGraphic;

			}else
			{
				Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize;
				Graphic_Multi nakedGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(pawn.ageTracker.CurKindLifeStage.bodyGraphicData.texPath, ShaderDatabase.Cutout, vector, Color.white);
				pawn.Drawer.renderer.graphics.nakedGraphic = nakedGraphic;

			}


		}





	}
}
