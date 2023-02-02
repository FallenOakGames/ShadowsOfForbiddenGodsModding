using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_Speed : Power
    {
        public P_Speed(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 1;
        }

        public override string getName()
        {
            return "Compulsive Movement";
        }

        public override string getDesc()
        {
            return "Causes a <b>Drone</b> to take another move, at the cost of 1HP";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.eyes_0.png");
        }


        public override string getRestrictionText()
        {
            return "Must target a drone which is either seeking prey or returning";
        }

        public override bool validTarget(Unit unit)
        {
            return unit.movesTaken > 0 && unit is UAEN_Drone;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
            if (unit.task != null)
            {
                unit.task.turnTick(unit);
            }
            unit.movesTaken = 0;
        }
    }
}
