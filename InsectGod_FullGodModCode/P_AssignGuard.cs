using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_AssignGuard : Power
    {
        public P_AssignGuard(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 1;
        }

        public override string getName()
        {
            return "Assign Guard";
        }

        public override string getDesc()
        {
            return "Turns 5% <b>Larval Mass</b> into a <b>Vespid Guard</b> minion for a <b>Drone</b> or agent with a free slot, reducing the risk of attack by heroes";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_larvalMass;
        }


        public override string getRestrictionText()
        {
            return "Must target a drone which is either seeking prey or returning";
        }

        public override bool validTarget(Unit unit)
        {
            foreach (Property pr in unit.location.properties)
            {
                if (pr is Pr_LarvalMass larvae)
                {
                    if (larvae.charge < 5) { return false; }
                    if (unit is UA ua)
                    {
                        foreach (Minion m in ua.minions)
                        {
                            if (m == null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);

            foreach (Property pr in unit.location.properties)
            {
                if (pr is Pr_LarvalMass larvae)
                {
                    if (larvae.charge < 5) { return; }

                    if (unit is UA ua)
                    {
                        for (int m=0;m<ua.minions.Length;m++)
                        {
                            if (ua.minions[m] == null)
                            {
                                ua.minions[m] = new M_VespidicGuard(map);
                                larvae.charge -= 5;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
