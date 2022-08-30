using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class Rti_AgentUniqueAbility : Ritual
    {
        public Rti_AgentUniqueAbility(Location loc) : base(loc)
        {

        }

        public override string getName()
        {
            return "Insect unique ability";
        }
        public override void complete(UA u)
        {
            Pr_InsectFamine famine = new Pr_InsectFamine(u.location);
            famine.charge = 10;
            u.location.properties.Add(famine);
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getComplexity()
        {
            return 30;
        }

        public override double getMenace()
        {
            //Does not apply, since this is an evil challenge
            return 0;
        }

        public override double getProfile()
        {
            return 5;
        }

        public override int getCompletionMenace()
        {
            return 10;
        }
        public override int getCompletionProfile()
        {
            return 10;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            if (msgs != null)
            {
                msgs.Add(new ReasonMsg("Stat: Lore", unit.getStatLore()));
            }
            return Math.Max(1, unit.getStatLore());
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("insect.iconDeadFish.png");
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override bool valid()
        {
            return true;
        }

        public override bool validFor(UA ua)
        {
            return ua.location.settlement is SettlementHuman;
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { Tags.DISCORD };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { Tags.DISEASE };
        }
    }
}
