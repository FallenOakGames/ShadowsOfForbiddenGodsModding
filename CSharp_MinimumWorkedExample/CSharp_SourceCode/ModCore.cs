using System;
using System.Collections.Generic;
using Assets.Code;
using Assets.Code.Modding;

namespace ShadowsInsectGod
{
    public class ModCore :  ModKernel
    {

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            gods.Add(new God_Insect());
        }

        public override void afterMapGenAfterHistorical(Map map)
        {
            base.afterMapGenAfterHistorical(map);

            map.overmind.agentsUnique.Add(new UAE_Abs_DemoAgent(map));
        }

    }
}
