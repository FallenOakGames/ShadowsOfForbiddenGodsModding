using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class Task_GoHome : Task
    {
        int stepsTaken = 0;
        public override string getShort()
        {
            return "Return to Hive";
        }
        public override string getLong()
        {
            return "This drone is returning to the hive";
        }

        public override void turnTick(Unit unit)
        {
            base.turnTick(unit);

            if (unit.map.overmind.god is God_Insect insect)
            {
                //If we have prey, place prey pheromone
                if (unit is UAEN_Drone drone)
                {
                    stepsTaken += 1;
                    insect.phFeed[unit.locIndex].charge += God_Insect.PHEROMONE_PLACE_STRENGTH_FEED * ((Math.Sqrt(drone.prey) / (stepsTaken + 5d)) * 5d);
                    if (insect.phFeed[unit.locIndex].charge > 300)
                    {
                        insect.phFeed[unit.locIndex].charge = 300;
                    }

                    //In the event everything has gone wrong, just cheat and make them go back home automatically
                    if (stepsTaken > 12)
                    {
                        //Code taken from P_HomingInstinct
                        int closest = -1;
                        Location close = null;
                        foreach (Location l in unit.map.locations)
                        {
                            if (l.settlement is Set_Hive)
                            {
                                int dist = unit.map.getStepDist(l, unit.location);
                                if (dist < closest || close == null)
                                {
                                    close = l;
                                    closest = dist;
                                }
                            }
                        }
                        if (close != null)
                        {
                            unit.task = new Task_GoToLocation(close);
                            return;
                        }
                    }
                }

                //First off, check if we're at home. If so, job done, set to null so the AI can assign a new task
                if (unit.location.settlement is Set_Hive)
                {
                    unit.task = null;
                    return;
                }

                //The home pheromone should get stronger as we approach home. All we should need to do is to step from a lower-concentration location to a higher-strength one, until eventually we get there
                //If for some reason we can't find home, we'll just take a random step in a random direction
                //We're adding in a random component (as long as it's still higher concentration), because this allows exploration of routes home which can lead to optimisation of pathing
                Location nextStep = null;
                double localPheromone = insect.phHome[unit.locIndex].charge;
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

                    double delta = insect.phHome[l.index].charge - localPheromone;
                    delta *= Eleven.random.NextDouble() + Eleven.random.NextDouble();
                    if (delta > best)
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

                    //Checking here and cancelling the task can save a turn
                    if (unit.location.settlement is Set_Hive)
                    {
                        if (unit is UAEN_Drone drone2)
                        {
                            stepsTaken += 1;
                            insect.phFeed[unit.locIndex].charge += (drone2.prey / (stepsTaken + 5d)) * 5d;
                            if (insect.phFeed[unit.locIndex].charge > 300)
                            {
                                insect.phFeed[unit.locIndex].charge = 300;
                            }
                        }

                        unit.task = null;
                        return;
                    }
                }
                else if (fallback != null)
                {
                    unit.map.adjacentMoveTo(unit, fallback);

                    //Checking here and cancelling the task can save a turn
                    //Would be highly unlikely, but may as well check
                    if (unit.location.settlement is Set_Hive)
                    {
                        if (unit is UAEN_Drone drone2)
                        {
                            stepsTaken += 1;
                            insect.phFeed[unit.locIndex].charge += (drone2.prey / (stepsTaken + 5d)) * 5d;
                            if (insect.phFeed[unit.locIndex].charge > 300)
                            {
                                insect.phFeed[unit.locIndex].charge = 300;
                            }
                        }

                        unit.task = null;
                        return;
                    }
                }
                else
                {
                    //We shouldn't be able to reach here, but if so, just wait a turn and hope whatever insanity has happened doesn't happen again
                }
            }
            else
            {
                //Can only run using the insect god, because we're referencing into it for speed reasons. Theoretically could expand to use a slower lookup method in the future
                if (unit.location.settlement is Set_Hive)
                {
                    unit.task = null;
                    return;
                }
            }
        }
    }
}
