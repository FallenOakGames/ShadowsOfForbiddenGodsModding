using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_Infect : Power
    {
        public P_Infect(Map map) : base(map)
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
            return "Infect";
        }

        public override string getRestrictionText()
        {
            return "Must target a unit which taken a move";
        }

        public override bool validTarget(Unit unit)
        {
            if (unit.person != null)
            {

                foreach (Trait t in unit.person.traits)
                {
                    if (t is T_Infection) { return false; }
                }
            }
            return true;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);

            foreach (Trait t in unit.person.traits)
            {
                if (t is T_Infection) { return; }
            }
            T_Infection spread = new T_Infection();
            unit.person.receiveTrait(spread);
        }
    }
}
