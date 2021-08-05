using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using RimWorld.BaseGen;

namespace VAECaves
{

    public class GenStep_BasicGenVAE : GenStep
    {

        protected Dictionary<string, float> randomRoomEvents = new Dictionary<string, float>();

        protected CellRect adventureRegion;

        protected ResolveParams baseResolveParams;

        public override int SeedPart => 44867541;

        public override void Generate(Map map, GenStepParams parms)
        {
            int num = map.Size.x / 10;
            int num2 = 8 * map.Size.x / 10;
            int num3 = map.Size.z / 10;
            int num4 = 8 * map.Size.z / 10;
            this.adventureRegion = new CellRect(num, num3, num2, num4);
            this.adventureRegion.ClipInsideMap(map);
            BaseGen.globalSettings.map = map;
            this.randomRoomEvents.Clear();
            IntVec3 playerStartSpot;
            CellFinder.TryFindRandomEdgeCellWith((IntVec3 v) => GenGrid.Standable(v, map), map, 0f, out playerStartSpot);
            MapGenerator.PlayerStartSpot = playerStartSpot;
            this.baseResolveParams = default(ResolveParams);
            foreach (string current in this.randomRoomEvents.Keys)
            {
                this.baseResolveParams.SetCustom<float>(current, this.randomRoomEvents[current], false);
            }
        }
    }

    public class GenStep_MonsterEncounter_Spider : GenStep_BasicGenVAE
    {
        public const int edgeSize = 40;

        public override void Generate(Map map, GenStepParams parms)
        {
            PawnKindDef huntingKind = PawnKindDef.Named("VAECaves_AncientGiantSpider");
            base.Generate(map, parms);
            CellRect rect = new CellRect(Rand.RangeInclusive(this.adventureRegion.minX + 10, this.adventureRegion.maxX - (edgeSize + 10)), Rand.RangeInclusive(this.adventureRegion.minZ + 10, this.adventureRegion.maxZ - (edgeSize + 10)), edgeSize, edgeSize);
            rect.ClipInsideMap(map);
            ResolveParams animalResolveParams = this.baseResolveParams;
            animalResolveParams.rect = rect;
            animalResolveParams.singlePawnKindDef = huntingKind;
            animalResolveParams.faction = Faction.OfInsects;
            BaseGen.symbolStack.Push("VAE_SpawnHuntingMonstersSymbol", animalResolveParams);
            BaseGen.Generate();
        }
    }

   


}