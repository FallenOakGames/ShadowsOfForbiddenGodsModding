using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_HomingInstinct : Power
    {
        public P_HomingInstinct(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 0;
        }

        public override string getName()
        {
            return "Homing Instinct";
        }

        public override string getDesc()
        {
            return "Guides a <b>Drone</b> back to the nearest <b>Hive</b>, to stop it exploring in useless locations, or if it has gotten lost or is moving in an undesirable direction";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_homingInstinct;
        }


        public override string getRestrictionText()
        {
            return "Must target a drone";
        }

        public override bool validTarget(Unit unit)
        {
            return unit is UAEN_Drone;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);

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
                unit.task = new Task_GoToLocation(close);
            }
        }
    }
}
