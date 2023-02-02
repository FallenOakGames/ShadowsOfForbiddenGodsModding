using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class Task_SeekPrey : Task
    {
        public int stepsTaken = 0;
        public HashSet<int> visited = new HashSet<int>();

        public override string getShort()
        {
            return "Seeking Prey";
        }
        public override string getLong()
        {
            return "This drone is seeking prey by following feeding pheromone trails";
        }
        public override void turnTick(Unit unit)
        {
            base.turnTick(unit);

            if (unit.map.overmind.god is God_Insect insect)
            {
                visited.Add(unit.locIndex);
                stepsTaken += 1;

                //Place pheromone trail, so we can get home
                //The +5 and /5 here are just to avoid the decay curve being insane. It's roughly a "place less pheromone the further from home you go, so the gradient is smooth towards home")
                insect.phHome[unit.locIndex].charge += (God_Insect.PHEROMONE_PLACE_STRENGTH_HOME / (stepsTaken + 5)) * 5;
                if (insect.phHome[unit.locIndex].charge > 300)
                {
                    insect.phHome[unit.locIndex].charge = 300;
                }

                //First off, check if we're at home. If so, job done, set to null so the AI can assign a new task
                if (unit.location.settlement is SettlementHuman hum && unit is UAEN_Drone drone)
                {
                    foreach (Property pr in unit.location.properties)
                    {
                        if (pr is Pr_InfectedPopulace infect)
                        {
                            drone.addMenace(God_Insect.MENACE_FROM_HARVEST);
                            drone.addProfile(God_Insect.PROFILE_FROM_HARVEST);
                            int delta = (int)(Math.Min(20, Math.Max(2, hum.population / 3))*infect.charge*0.01);
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

                //We'll do a bit of exploring if there are other drones who can pick up the slack if we get lost
                //Exploration will mostly follow the pheromone trail, but can deviate along it a bit, to possibly find other sources of prey or to reduce path length
                bool canExploreSlightly = false;
                foreach (Unit u in unit.location.units)
                {
                    if (u is UAEN_Drone && u != unit) { canExploreSlightly = true; }
                }

                Location nextStep = null;
                double best = 0;
                int c = 0;
                Location fallback = null;
                foreach (Location l in unit.location.getNeighbours())
                {
                    bool denied = false;
                    foreach (Unit u in l.units)
                    {
                        if (u is UM_HumanArmy && l.settlement != null && l.settlement.shadow < 0.5) { denied = true; break; }
                    }
                    if (denied) { continue; }

                    double delta = insect.phFeed[l.index].charge;

                    if (canExploreSlightly) {
                        delta *= Eleven.random.NextDouble() + Eleven.random.NextDouble();
                    }
                    if ((!visited.Contains(l.index)) && delta > best)
                    {
                        best = delta;
                        nextStep = l;
                    }
                    c += 1;
                    if (Eleven.random.Next(c) == 0)
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
                //Can only run using the insect god, because we're referencing into it for speed reasons. Theoretically could expand to use a slower lookup method in the future
                unit.task = null;
                return;
            }
        }
    }
}
