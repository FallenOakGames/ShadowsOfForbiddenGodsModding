using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class Task_Doomed : Task
    {
        public UM_Refugees refugees;
        public int target;

        public Task_Doomed(UM_Refugees refugees)
        {
            this.refugees = refugees;
        }
        public override string getShort()
        {
            return "Doomed";
        }
        public override string getLong()
        {
            return "These humans are compelled to walk towards the nearest <b>Hive</b>, where they will be devoured";
        }

        public override void turnTick(Unit unit)
        {
            base.turnTick(unit);

            if (unit.location.settlement is Set_Hive)
            {
                foreach (Property pr in unit.location.properties)
                {
                    if (pr is Pr_LarvalMass mass)
                    {
                        mass.charge += refugees.hp*4;
                        refugees.die(unit.map, "Devoured");
                        return;
                    }
                }
            }

            Location targetLoc = unit.map.locations[target];
            if (targetLoc.settlement is Set_Hive == false)
            {
                int bestDist = -1;
                foreach (Location l2 in unit.map.locations)
                {
                    if (l2.settlement is Set_Hive)
                    {
                        if (bestDist == -1 || unit.map.getStepDist(unit.location, l2) < bestDist)
                        {
                            bestDist = unit.map.getStepDist(unit.location, l2);
                            target = l2.index;
                        }
                    }
                }
            }

            targetLoc = unit.map.locations[target];
            if (targetLoc.settlement is Set_Hive)
            {
                unit.map.moveTowards(unit, targetLoc);
            }
        }
    }
}
