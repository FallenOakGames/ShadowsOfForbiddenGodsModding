using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_TreatPopulace : Challenge
    {
        public Pr_InfectedPopulace inf;

        public Ch_TreatPopulace(Location loc, Pr_InfectedPopulace infection) : base(loc)
        {
            inf = infection;
        }


        public override string getName()
        {
            return "Treat Populace";
        }
        public override string getDesc()
        {
            return "Cures the Cordyceps, decreases the total infection by 50%";
        }
        public override string getRestriction()
        {
            return "Requires a location with infected human population";
        }

        public override void complete(UA u)
        {
            inf.charge -= 50;
            if (inf.charge < 0) { inf.charge = 0; }
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getComplexity()
        {
            return 25;
        }

        public override double getMenace()
        {
            return inf.charge*0.5;
        }

        public override double getProfile()
        {
            return 50;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 0;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            //Messages are used to display values to the UI. Since not all passes gather data for the UI, messages can be null at some points, to save compute time while processing AI
            if (msgs != null)
            {
                msgs.Add(new ReasonMsg("Stat: Intrigue", unit.getStatIntrigue()));
                msgs.Add(new ReasonMsg("Stat: Lore", unit.getStatLore()));
            }
            return Math.Max(1, unit.getStatIntrigue() + unit.getStatLore());
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.plagueImmunity;
        }

        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return 1;
        }

        public override bool valid()
        {
            return inf.charge > 10;
        }

        public override bool validFor(UA ua)
        {
            return ua.person.awareness > 0;
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { ModCore.INSECT, Tags.DISEASE };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { };
        }
    }
}
