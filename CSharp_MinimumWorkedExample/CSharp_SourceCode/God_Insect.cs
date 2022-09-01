using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class God_Insect : God
    {

        public override void setup(Map map)
        {
            base.setup(map);

            powers.Add(new P_Speed(map));
            powerLevelReqs.Add(0);
        }

        public override Sprite getGodPortrait(World world)
        {
            return EventManager.getImg("insect.god_portrait.png");
        }

        public override Sprite getGodBackground(World world)
        {
            return EventManager.getImg("insect.god_background.jpg");
        }
    }
}
