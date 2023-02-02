using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_InfectRuler : Challenge
    {

        public Ch_InfectRuler(Location loc) : base(loc)
        {
        }


        public override string getName()
        {
            return "Infect Ruler";
        }
        public override string getDesc()
        {
            return "Spreads the fungal infection to the ruler of an infiltrated location";
        }
        public override string getRestriction()
        {
            return "Requires a 100% infiltrated location with a ruler with no infection";
        }
        public override string getCastFlavour()
        {
            return "During times of plague, the rulers of a city will seal themseles off. They may not understand the nature of microscopic diseases, but they know the pattern of infection enough to implement a rudimentary quarantine. Infecting them reliably will require infecting key elements of their personal entourage.";
        }

        public override void complete(UA u)
        {
            if (location.settlement is SettlementHuman hum && hum.ruler != null)
            {
                Person person = hum.ruler;

                foreach (Trait t in person.traits)
                {
                    if (t is T_Infection) { return; }
                }
                T_Infection spread = new T_Infection();
                person.receiveTrait(spread);
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
            return 50;
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
            if (location.settlement is SettlementHuman hum && hum.ruler != null)
            {
                if (hum.infiltration < 1) { return false; }

                Person person = hum.ruler;

                foreach (Trait t in person.traits)
                {
                    if (t is T_Infection) { return false; }
                }
                return true;
            }
            return false;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override int[] buildNegativeTags()
        {
            return new int[] { };
        }

        public override int[] buildPositiveTags()
        {
            return new int[] { ModCore.INSECT , Tags.DISEASE };
        }
    }
}
