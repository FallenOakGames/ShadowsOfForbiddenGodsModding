using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_AirborneSpores : Power
    {
        public P_AirborneSpores(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 2;
        }

        public override string getName()
        {
            return "Airborne Spores";
        }

        public override string getDesc()
        {
            return "Cause a Hive to begin to infect all heroes and acolytes in its location with the <b>Cordyceps Infection</b> trait, and to cause all human settlements within two links' distance to gain +1% <b>Infected Population</b> per turn. Causes the Hive to gain 1 <b>menace</b> per turn";
        }

        public override string getFlavour()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.fungalHive_Red.png");
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
            return loc.settlement is Set_Hive hive && (hive.airbornSpores == false);
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            if (loc.settlement is Set_Hive hive)
            {
                hive.airbornSpores = true;
            }
        }
    }
}
