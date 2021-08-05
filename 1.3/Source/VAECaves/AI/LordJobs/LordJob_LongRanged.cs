using Verse.AI.Group;
using RimWorld;
using Verse;
using System.Linq;
using System.Text;

namespace VAECaves
{




    public class LordJob_LongRanged : LordJob
    {
        private IntVec3 point;

        public LordJob_LongRanged()
        {
        }

        public LordJob_LongRanged(IntVec3 point)
        {
            this.point = point;
        }

        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            stateGraph.AddToil(new LordToil_DefendPoint(this.point, 200f));
            return stateGraph;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
        }
    }
}