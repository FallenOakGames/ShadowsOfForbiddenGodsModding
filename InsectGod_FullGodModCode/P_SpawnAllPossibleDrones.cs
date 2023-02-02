using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_SpawnAllPossibleDrones : Power
    {
        public P_SpawnAllPossibleDrones(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 0;
        }

        public override string getName()
        {
            return "Synchronised Infestation";
        }

        public override string getDesc()
        {
            return "Turns <b>all</b> infected heroes and acolytes not under your control into <b>drones</b>. Drones will randomly explore, seeking infected population. Once they find them, they steal them to carry back to the hive, to grow more larval mass. Adds world panic from `Fallen Hero' for each hero lost.";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_drone;
        }


        public override string getRestrictionText()
        {
            return "Must target a hero or acolyte with <b>Infection</b> at 100%. You must have a <b>Hive</b>. Cannot target a drone";
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UAEN) { return false; }

            bool hasHive = false;
            foreach (Location l in map.locations)
            {
                if (l.settlement is Set_Hive) { hasHive = true;break; }
            }
            if (!hasHive) { return false; }


            if (unit is UA ua)
            {
                foreach (Trait t in ua.person.traits)
                {
                    if (t is T_Infection inf)
                    {
                        return inf.maturation >= 100;
                    }
                }
            }

            return false;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit initial)
        {
            base.cast(initial);

            int n = 0;
            string names = "";
            List<Unit> opts = new List<Unit>();
            opts.AddRange(map.units);
            foreach (Unit unit in opts)
            {
                if (unit.isCommandable()) { continue; }
                if (unit is UAEN) { continue; }

                bool valid = false;
                if (unit is UA ua)
                {
                    foreach (Trait t in ua.person.traits)
                    {
                        if (t is T_Infection inf)
                        {
                            //return inf.maturation >= 100;
                            if (inf.maturation >= 100)
                            {
                                valid = true;
                                break;
                            }
                        }
                    }
                }
                if (valid)
                {
                    n += 1;
                    names += unit.getName() + ", ";
                    UAEN_Drone drone = new UAEN_Drone(unit.location, map.soc_dark, unit.person);
                    map.units.Add(drone);
                    unit.location.units.Add(drone);
                    map.units.Remove(unit);
                    unit.isDead = true;
                    unit.location.units.Remove(unit);

                    int closest = -1;
                    Location close = null;
                    foreach (Location l in map.locations)
                    {
                        if (l.settlement is Set_Hive)
                        {
                            int dist = map.getStepDist(l, unit.location);
                            if (dist < closest || close == null)
                            {
                                close = l;
                                closest = dist;
                            }
                        }
                    }
                    if (close != null)
                    {
                        drone.task = new Task_GoToLocation(close);
                    }

                    if (unit is UAG || unit is UAA)
                    {
                        map.overmind.panicHeroesFallen += map.param.panic_fallenHero;
                    }
                }
            }

            if (n > 1)
            {
                map.popMsg(n + " people were turned into Drones: " + names.Substring(0, names.Length - 2));
            }
        }
    }
}
