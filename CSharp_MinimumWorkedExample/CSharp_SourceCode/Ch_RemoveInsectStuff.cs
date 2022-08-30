using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class Ch_RemoveInsectStuff : Challenge
    {
        public Ch_RemoveInsectStuff(Location loc) : base(loc)
        {

        }

        public override string getDesc()
        {
            return "Do stuff related to insects";
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { Tags.DISEASE };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { Tags.DANGER };
        }

        public override void complete(UA u)
        {
            Pr_InsectFamine toReduce = null;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_InsectFamine fam)
                {
                    toReduce = fam;
                }
            }
            if (toReduce != null)
            {
                toReduce.charge /= 2;
            }
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override double getComplexity()
        {
            return 25;
        }

        public override double getMenace()
        {
            return 60;
        }

        public override string getName()
        {
            return "Insect Test Challenge";
        }

        public override double getProfile()
        {
            return 50;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            //Messages are used to display values to the UI. Since not all passes gather data for the UI, messages can be null at some points, to save compute time while processing AI
            if (msgs != null)
            {
                msgs.Add(new ReasonMsg("Stat: Might", unit.getStatMight()));
            }
            return Math.Max(1, unit.getStatMight());
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("insect.iconDeadFish.png");
        }

        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return 1;
        }

        public override bool valid()
        {
            return true;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }
    }
}
