using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace ShadowsInsectGod.Code
{
    public class T_TestedForInfection : Trait
    {
        public int expiry = 0;

        public T_TestedForInfection()
        {
            expiry = 20;
        }

        public override string getName()
        {
            return "Tested for Infection (" + expiry + ")";
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            expiry -= 1;
            if (expiry <= 0)
            {
                p.traits.Remove(this);
            }
        }
    }
}
