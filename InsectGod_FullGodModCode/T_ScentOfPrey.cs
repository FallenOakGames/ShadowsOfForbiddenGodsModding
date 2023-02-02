using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class T_TheScentOfPrey : Trait
    {
        public override string getDesc()
        {
            return "If this unit is in a location with <b>Infected Populace</b> it will emit <b>Feeding Pheromone</b> in the surrounding area, to call <b>Drones</b> to come and harvest the victims";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override string getName()
        {
            return "The Scent of Prey";
        }

        public override int[] getTags()
        {
            return new int[] { ModCore.INSECT, Tags.CRUEL };
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            Location loc = p.getLocation();
            if (loc.settlement is SettlementHuman hum && loc.map.overmind.god is God_Insect insectGod)
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_InfectedPopulace infected)
                    {
                        //We want to make the pheromone stronger the closer the drone gets to the prey, and stronger the more prey there is
                        double str = infected.charge * 0.005 * hum.population;

                        foreach (Location l2 in p.map.locations)
                        {
                            int dist = p.map.getStepDist(l2, loc);
                            if (dist < 5)
                            {
                                double appliedStrength = str / (1 + dist);
                                insectGod.phFeed[l2.index].charge += appliedStrength;
                                if (insectGod.phFeed[l2.index].charge > 300)
                                {
                                    insectGod.phFeed[l2.index].charge = 300;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
