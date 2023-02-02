using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_RemoveInsectStuff : Challenge
    {
        public Pr_LarvalMass larva;

        public Ch_RemoveInsectStuff(Location loc,Pr_LarvalMass larva) : base(loc)
        {
            this.larva = larva;
        }

        public override string getDesc()
        {
            return "Removes a third of the <b>Larval Mass</b> in this location. Heroes with more awareness and no shadow will be motivated to do this action in times of high World Panic";
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { Tags.DISEASE ,ModCore.INSECT};
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { Tags.DANGER };
        }

        public override void complete(UA u)
        {
            int deleted = (int)Math.Max(1,larva.charge * 0.33);
            larva.charge -= deleted;
            map.addUnifiedMessage(u, u.location, "Larvae Destroyed", u.getName() + " has destroyed larvae at " + u.location.getName() + ", killing " + deleted + " of them, leaving " + (int)(larva.charge), "LARVAE_DESTROYED",true);
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override double getComplexity()
        {
            return 15;
        }

        public override double getMenace()
        {
            return larva.charge;
        }

        public override string getName()
        {
            return "Destroy Larva";
        }

        public override double getProfile()
        {
            return 50;
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double u = base.getUtility(ua, msgs);

            double localU = ua.person.awareness * map.worldPanic * larva.charge * (1-ua.person.shadow);
            u += localU;
            if (msgs != null)
            {
                msgs.Add(new ReasonMsg("Desperate (World Panic)", u));
            }

            return u;
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
            return EventManager.getImg("insect.fungalHive_Grey.png");
        }

        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return 1;
        }

        public override bool valid()
        {
            return larva.charge >= 3;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }
    }
}
