using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;

namespace ShadowsInsectGod
{
    public class T_InsectTrait : Trait
    {
        public override string getDesc()
        {
            return "A trait given by the insect god";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override string getName()
        {
            return "Insect trait";
        }

        public override int[] getTags()
        {
            return new int[] { Tags.RELIGION, Tags.CRUEL };
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (p.unit != null && p.unit.location.settlement is SettlementHuman hum)
            {
                Property.addToProperty("Insect Stuff", Property.standardProperties.FAMINE, 2, p.unit.location);
            }
        }
    }
}
