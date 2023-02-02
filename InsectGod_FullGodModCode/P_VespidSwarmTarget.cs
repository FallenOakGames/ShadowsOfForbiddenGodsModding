using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_VespidSwarmTarget : Power
    {
        public P_VespidSwarmTarget(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 0;
        }

        public override string getDesc()
        {
            return "";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_swarmTarget;
        }

        public override string getName()
        {
            return "Vespid Swarm Target";
        }

        public override string getRestrictionText()
        {
            return "Must target a location";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            if (map.overmind.god is God_Insect god)
            {
                god.vespidSwarmTarget = loc;
            }
        }
    }
}
