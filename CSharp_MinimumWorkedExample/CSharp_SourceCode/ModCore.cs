using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace ShadowsInsectGod
{
    public class ModCore : Assets.Code.Modding.ModKernel
    {
        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            gods.Add(new God_Insect());
        }

        public override void beforeMapGen(Map map)
        {
            base.beforeMapGen(map);

            map.overmind.agentsUnique.Add(new UAE_Abs_DemoAgent(map));

        }
    }
}
