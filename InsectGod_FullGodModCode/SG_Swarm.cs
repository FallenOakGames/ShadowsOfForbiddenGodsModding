using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;


namespace ShadowsInsectGod.Code
{
    public class SG_Swarm : SocialGroup
    {
        public SG_Swarm(Map map,Location loc):base(map)
        {
            name = loc.shortName;
        }

        public override string getName()
        {
            return name + " Swarm";
        }

        public override bool isDark()
        {
            return true;
        }
    }
}
