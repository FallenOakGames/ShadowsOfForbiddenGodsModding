using ShadowsInsectGod.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code
{
    public class H_InsectFaith : HolyTenet
    {
        public H_InsectFaith(HolyOrder us) : base(us)
        {
        }

        public override string getName()
        {
            return "Insectine Devotion";
        }

        public override string getDesc()
        {
            return "Refugees in a location with a temple of this faith are compelled to walk to the nearest <b>Hive</b> to be devoured. At -2, this occurs in all settlements following this faith";
        }
        public override int getMaxNegativeInfluence()
        {
            return -2;
        }

        public override void turnTickTemple(Sub_Temple temple)
        {
            base.turnTickTemple(temple);

            if (status < 0)
            {
                foreach (Unit u in temple.settlement.location.units)
                {
                    if (u is UM_Refugees refugees)
                    {
                        if (refugees.task is Task_Doomed == false)
                        {
                            refugees.task = new Task_Doomed(refugees);
                        }
                    }
                }
            }
        }

        public override void turnTickSettlement(SettlementHuman settlement)
        {
            base.turnTickSettlement(settlement);

            if (status < -1)
            {
                foreach (Unit u in settlement.location.units)
                {
                    if (u is UM_Refugees refugees)
                    {
                        if (refugees.task is Task_Doomed == false)
                        {
                            refugees.task = new Task_Doomed(refugees);
                        }
                    }
                }
            }
        }
    }
}
