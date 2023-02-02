using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class T_Infection : Trait
    {
        public int maturation = 0;
        public bool discovered = false;
        public override string getName()
        {
            return "Cordyceps Infection (" + maturation  + "%)" + (discovered ? " DISCOVERED" : "");
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            foreach (Trait t in p.traits)
            {
                if (t is T_ChosenOne)
                {
                    p.traits.Remove(this);
                    return;
                }
            }

            if (maturation < God_Insect.INFECTION_MAX_MATURATION)
            {
                maturation += 2;
            }
            if (maturation >= God_Insect.INFECTION_MAX_MATURATION)
            {
                maturation = God_Insect.INFECTION_MAX_MATURATION;
            }

            if (p.unit is UAEN) { return; }//Drones can't spread the infection, nor can vampires or othersuch

            Location l = p.getLocation();
            if (l.settlement is SettlementHuman hum)
            {
                //Spread the infection to the civilian population
                bool present = false;
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_InfectedPopulace pop)
                    {
                        pop.influences.Add(new ReasonMsg("From " + p.getName(), God_Insect.POPULATION_INFECTION_RATE));
                        present = true;
                        break;
                    }
                }
                if (!present)
                {
                    Pr_InfectedPopulace add = new Pr_InfectedPopulace(l);
                    add.charge = God_Insect.POPULATION_INFECTION_RATE;
                    l.properties.Add(add);
                }

                //Then we spread to both the ruler and to other characters nearby
                double pSpread = God_Insect.P_TRAIT_SPREAD * (maturation*0.01);
                if (Eleven.random.NextDouble() < pSpread)
                {
                    if (hum.ruler != null && hum.ruler.awareness < 1)
                    {
                        bool preExisting = false;
                        foreach (Trait t in hum.ruler.traits)
                        {
                            if (t is T_Infection) { preExisting = true; break; }
                        }
                        if (!preExisting)
                        {
                            hum.map.addUnifiedMessage(assignedTo, hum.ruler, "Infection", assignedTo.getName() + " has infected " + hum.ruler.getName() + " with the Cordyceps fungal infection. Infection can pass from human to human if they are in the same location. It cannot infect a character with 100% awareness, as they will take precautions to avoid contagion", "PERSON INFECTS OTHER");
                            hum.map.addMessage("Infection: " + hum.ruler.getName(), 0.75, true, hum.location.hex);
                            T_Infection trait = new T_Infection();
                            hum.ruler.receiveTrait(trait);
                        }
                    }
                }
                foreach (Unit u in l.units)
                {
                    if (u is UAEN) { continue; }//Can't infect drones and suchlike

                    if (u is UA ua && ua.person != p && (u.map.awarenessManager.getChosenOne() != u) && ua.person.awareness < 1)
                    {
                        if (Eleven.random.NextDouble() < pSpread)
                        {
                            bool preExisting = false;
                            foreach (Trait t in ua.person.traits)
                            {
                                if (t is T_Infection) { preExisting = true; break; }
                            }
                            if (!preExisting)
                            {
                                hum.map.addUnifiedMessage(assignedTo, ua.person, "Infection", assignedTo.getName() + " has infected " + ua.person.getName() + " with the Cordyceps fungal infection. Infection can pass from human to human if they are in the same location. It cannot infect a character with 100% awareness, as they will take precautions to avoid contagion", "PERSON INFECTS OTHER");
                                hum.map.addMessage("Infection: " + ua.getName(), 0.75, true, hum.location.hex);
                                T_Infection trait = new T_Infection();
                                ua.person.receiveTrait(trait);
                            }
                        }
                    }
                }
            }
        }

        public override double getActionUtility(Person person, SettlementHuman hum, Assets.Code.Action act, List<ReasonMsg> reasons)
        {
            if (act is Act_RaiseArmy)
            {
                double mult = maturation * 0.01;
                double localU = mult * -100;
                if (reasons != null)
                {
                    reasons.Add(new ReasonMsg("Infection Affects Judgement", localU));
                }
                return localU;
            }
            else
            {
                return 0;
            }
        }
    }
}
