using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Ch_TargettedInfection : Challenge
    {

        public Ch_TargettedInfection(Location loc) : base(loc)
        {
        }


        public override string getName()
        {
            return "Targeted Infection";
        }
        public override string getDesc()
        {
            return "Spreads the fungal infection to all heroes and acolytes who have this location as their home location (except for the Chosen One)";
        }
        public override string getRestriction()
        {
            return "Requires a location with 100% infected human population which is home to either a hero or an acolyte";
        }

        public override string getCastFlavour()
        {
            return "To ensure travellers and passers-through are infected, every inn must be infested, every well vomited into, and every barrel of ale contaminated.";
        }

        public override void complete(UA u)
        {
            //The message string is a way for this method to relay information to the challenge completion screen
            //It must be reset each time
            //A somewhat inelegant implementation, brought about by needs of a changing UI
            msgString = "";

            List<string> names = new List<string>();
            if (location.settlement is SettlementHuman hum)
            {
                foreach (Unit other in map.units)
                {
                    if (other == map.awarenessManager.getChosenOne()) { continue; }

                    if ((!other.isDead) && other.person != null && (other is UAG || other is UAA) && (!other.person.isDead) && other.homeLocation == location.index)
                    {
                        bool hasInfection = false;
                        Person person = other.person;

                        foreach (Trait t in person.traits)
                        {
                            if (t is T_Infection) { hasInfection = true; }
                        }
                        if (!hasInfection)
                        {
                            T_Infection spread = new T_Infection();
                            person.receiveTrait(spread);
                            names.Add(person.getFullName());
                        }
                    }
                }
            }
            if (names.Count == 0)
            {
                msgString = "There was no-one to spread the infection to";
            }
            else if (names.Count == 1)
            {
                msgString = "The infection was spread to " + names[0];
            }else
            {
                msgString = "The infection was spread to ";
                for (int i = 0; i < names.Count - 1; i++)
                {
                    msgString += names[i] + ", ";
                }
                
                msgString += "and "+ names[names.Count - 1];
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
            return map.world.textureStore.agent_insect_lateStage;
        }

        public override int isGoodTernary()
        {
            //1 is good, 0 is neutral, -1 is evil
            return -1;
        }

        public override bool valid()
        {
            if (location.settlement is SettlementHuman)
            {
                foreach (Property pr in location.properties)
                {
                    if (pr is Pr_InfectedPopulace && pr.charge > 99)
                    {
                        foreach (Unit other in map.units)
                        {
                            if ((!other.isDead) && other.person != null && (!other.person.isDead) && other.homeLocation == location.index)
                            {
                                //There is at least one person who calls here home. We now need to see if they are uninfected
                                bool hasInfection = false;
                                Person person = other.person;

                                foreach (Trait t in person.traits)
                                {
                                    if (t is T_Infection) { hasInfection = true; }
                                }
                                if (!hasInfection)
                                {
                                    return true;
                                }
                            }
                        }
                        //If we've gotten here, there are no uninfected with their home here
                        return false;
                    }
                }
            }
            //If we got here there wasn't even any infected populace
            return false;
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
