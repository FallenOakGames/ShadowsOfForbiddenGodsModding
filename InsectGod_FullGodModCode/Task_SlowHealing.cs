using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class Task_SlowHealing : Task
    {

        public int stepsTaken;

        public override string getShort()
        {
            return "Slow Healing";
        }
        public override string getLong()
        {
            return "This unit is gaining 1HP every 3 turns";
        }

        public override void turnTick(Unit unit)
        {
            base.turnTick(unit);

            if (unit.hp >= unit.maxHp)
            {
                unit.hp = unit.maxHp;
                unit.task = null;
                return;
            }

            stepsTaken += 1;
            if (stepsTaken % 3 == 0)
            {
                unit.hp += 1;
                if (unit.hp >= unit.maxHp)
                {
                    unit.hp = unit.maxHp;
                    unit.task = null;
                    return;
                }
            }
        }
    }
}
