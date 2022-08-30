using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod
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

        public override string getDesc()
        {
            return "Allows a unit to move again";
        }

        public override string getFlavour()
        {
            return "Insect scuttle";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.eyes_0.png");
        }

        public override string getName()
        {
            return "Speed";
        }

        public override string getRestrictionText()
        {
            return "Must target a unit which taken a move";
        }

        public override bool validTarget(Unit unit)
        {
            return unit.movesTaken > 0;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
            unit.movesTaken = 0;
        }
    }
}
