using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class UAE_DemoAgent : UAE
    {
        public UAE_DemoAgent(Location loc,Society soc) : base(loc, soc)
        {
            this.rituals.Add(new Rti_AgentUniqueAbility(loc));
        }

        public override bool isCommandable()
        {
            return true;
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("insect.iconDeadFish.png");
        }

        public override string getName()
        {
            return "Insect Test Agent";
        }
        public override bool definesName()
        {
            return true;
        }
    }
}
