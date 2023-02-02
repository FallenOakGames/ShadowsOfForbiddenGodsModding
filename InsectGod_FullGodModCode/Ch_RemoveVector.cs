using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_RemoveVector : Challenge
    {

        public Ch_RemoveVector(Location loc) : base(loc)
        {
        }


        public override string getName()
        {
            return "Remove Vector";
        }
        public override string getDesc()
        {
            return "Assassinates a human ruler to stop the infection from spreading any further. A desperate act, done only when world panic is high";
        }
        public override string getRestriction()
        {
            return "Requires a 100% infiltrated location with a ruler with no infection";
        }

        public override void complete(UA u)
        {
            if (location.settlement is SettlementHuman hum && hum.ruler != null)
            {
                map.addUnifiedMessage(u, hum.ruler, "Assassination", u.getName() + " assassinated " + hum.ruler.getName() + " in a desperate bid to stop the spread of infection. An infected ruler will doom the population, and so " + u.getName() + " took action to replace " + hum.ruler.getName() + " with their uninfected heir, no matter the cost.", "EXTREME MEASURES TAKEN", true);

                hum.ruler.die("Killed to prevent the spread of disease", true, u, true);
            }
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getComplexity()
        {
            return 20;
        }

        public override double getMenace()
        {
            return 0;
        }

        public override double getProfile()
        {
            return 50;
        }

        public override int getCompletionMenace()
        {
            return 10;
        }

        public override int getCompletionProfile()
        {
            return 20;
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
            return EventManager.getImg("insect.iconDeadFish.png");
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double u =  base.getUtility(ua, msgs);

            double localU = -100;
            if (msgs != null) { msgs.Add(new ReasonMsg("Extreme Measure", localU)); }
            u += localU;

            localU = map.worldPanic*130;
            if (msgs != null) { msgs.Add(new ReasonMsg("Desperate Times (World Panic)", localU)); }
            u += localU;

            return u;
        }
        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return 1;
        }

        public override bool valid()
        {
            if (location.settlement is SettlementHuman hum && hum.ruler != null && hum.heir != null)
            {
                foreach (Trait t in hum.heir.traits)
                {
                    if (t is T_Infection) { return false; }
                } 
                foreach (Trait t in hum.ruler.traits)
                {
                    if (t is T_Infection) { return true ; }
                }
                return false;
            }
            return false;
        }

        public override bool validFor(UA ua)
        {
            return ua.person.awareness > 0.9;
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { ModCore.INSECT, Tags.DISEASE,Tags.COOPERATION };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { Tags.CRUEL,Tags.DANGER,Tags.DISCORD};
        }
    }
}
