using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_InfectPopulace : Challenge
    {
        public Pr_InfectedPopulace inf;

        public Ch_InfectPopulace(Location loc, Pr_InfectedPopulace infection) : base(loc)
        {
            inf = infection;
        }


        public override string getName()
        {
            return "Infect Populace";
        }
        public override string getDesc()
        {
            return "Spreads the fungal Cordyceps infection amongst the populace, increasing the total infection by 25%";
        }
        public override string getRestriction()
        {
            return "Requires a location with infected human population";
        }

        public override string getCastFlavour()
        {
            return "The parasite spreads by cough and touch. All it takes to spread is a few sickly individuals forcing themselves to lurch through the streets while spewing their infection, or, worse, being forced by the parasitse inside their brain.";
        }

        public override void complete(UA u)
        {
            //The message string is a way for this method to relay information to the challenge completion screen
            //It must be reset each time
            //A somewhat inelegant implementation, brought about by needs of a changing UI
            msgString = "";

            inf.charge += 25;
            if (inf.charge > 100) { inf.charge = 100; }
            msgString = "Infection of the population is now at " + (int)(inf.charge) + "%";
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
            return 50;
        }

        public override double getProfile()
        {
            return 50;
        }

        public override int getCompletionMenace()
        {
            return 8;
        }

        public override int getCompletionProfile()
        {
            return 2;
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
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_infectedPopulace;
        }

        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return -1;
        }

        public override bool valid()
        {
            return inf.charge < 100;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override int[] buildNegativeTags()
        {
            return new int[] {  };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { ModCore.INSECT , Tags.DISEASE };
        }
    }
}
