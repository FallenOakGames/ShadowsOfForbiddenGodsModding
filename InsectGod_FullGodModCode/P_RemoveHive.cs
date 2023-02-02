using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_RemoveHive : Power
    {
        public P_RemoveHive(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 0;
        }

        public override string getName()
        {
            return "Remove Hive";
        }

        public override string getDesc()
        {
            return "Removes a hive, allowing you to cause the drones to start operating out of a new hive closer to their prey";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.fungalHive_Grey.png");
        }


        public override string getRestrictionText()
        {
            return "Must target a location with <b>Hive</b>";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return loc.settlement is Set_Hive;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            loc.settlement = null;
            loc.soc = null;
            if (map.overmind.god is God_Insect god) {
                foreach (Property pr in god.phHome)
                {
                    if (pr.location == loc) { 
                        //Not a home, remove home pheromone
                        pr.charge = 0; 
                    }
                    else
                    {
                        //Decrease nearby pheromones, smoothly
                        //At dist = 1 (adjacent), mult = 4 / 8 = 0.5
                        //          2, mult = 5 / 8, 0.625
                        //       ...4, mult = 7 / 8
                        double dist = map.getStepDist(pr.location, loc);
                        if (dist < 5)
                        {
                            double mult = (3+dist) / 8d;
                            pr.charge *= mult;
                        }
                    }
                }
            }
        }
    }
}
