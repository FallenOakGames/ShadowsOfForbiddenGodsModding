using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class Task_Explore : Task
    {

        public int stepsTaken;
        public int stepsToTake = 8;
        public HashSet<int> visited = new HashSet<int>();

        public override string getShort()
        {
            return "Explore";
        }
        public override string getLong()
        {
            return "This drone is seeking prey by exploring its environment";
        }

        public override void turnTick(Unit unit)
        {
            base.turnTick(unit);

            if (unit.map.overmind.god is God_Insect insect)
            {
                stepsTaken += 1;
                visited.Add(unit.locIndex);

                //Place pheromone trail, so we can get home
                //The +5 and /5 here are just to avoid the decay curve being insane. It's roughly a "place less pheromone the further from home you go, so the gradient is smooth towards home")
                insect.phHome[unit.locIndex].charge += (God_Insect.PHEROMONE_PLACE_STRENGTH_HOME / (stepsTaken + 5)) * 5;
                if (insect.phHome[unit.locIndex].charge > 300)
                {
                    insect.phHome[unit.locIndex].charge = 300;
                }

                if (unit.location.settlement is SettlementHuman hum && unit is UAEN_Drone drone)
                {
                    foreach (Property pr in unit.location.properties)
                    {
                        if (pr is Pr_InfectedPopulace infect && infect.charge > 5)
                        {
                            drone.addMenace(God_Insect.MENACE_FROM_HARVEST);
                            drone.addProfile(God_Insect.PROFILE_FROM_HARVEST);
                            int delta = (int)(Math.Min(20, Math.Max(2, hum.population / 3)) * infect.charge * 0.01);
                            delta = Math.Max(1, delta);
                            infect.charge -= delta;
                            if (infect.charge < 0) { infect.charge = 0; }
                            hum.population -= delta;
                            if (hum.population <= 0)
                            {
                                hum.fallIntoRuin("Devoured by insects", drone);
                            }
                            drone.prey += delta;
                            unit.task = new Task_GoHome();
                            return;
                        }
                    }
                }

                //See if we're done exploring
                if (stepsTaken > stepsToTake)
                {
                    unit.task = new Task_GoHome();
                    return;
                }



                //Just step randomly. We initially want to try to avoid re-visiting a location, but if need be, we will
                int c = 0;
                Location nextStep = null;
                int c2 = 0;
                Location fallback = null;
                foreach (Location l in unit.location.getNeighbours())
                {
                    bool denied = false;
                    foreach (Unit u in l.units)
                    {
                        if (u is UM_HumanArmy && l.settlement != null && l.settlement.shadow < 0.5) { denied = true; break; }
                    }
                    if (denied) { continue; }

                    if (visited.Contains(l.index) == false)
                    {
                        c += 1;
                        if (Eleven.random.Next(c) == 0)
                        {
                            nextStep = l;
                        }
                    }
                    c2 += 1;
                    if (Eleven.random.Next(c2) == 0)
                    {
                        fallback = l;
                    }
                }

                if (nextStep != null)
                {
                    unit.map.adjacentMoveTo(unit, nextStep);
                }
                else if (fallback != null)
                {
                    unit.map.adjacentMoveTo(unit, fallback);
                }
                else
                {
                    //We shouldn't be able to reach here, but if so, just wait a turn and hope whatever insanity has happened doesn't happen again
                }
            }
            else
            {
                unit.task = null;
                return;
            }
        }
    }
}
