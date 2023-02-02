using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Sub_HiveSpire : Subsettlement
    {
        public Sub_HiveSpire(Settlement set) : base(set)
        {
        }

        public override string getName()
        {
            return "Hive Spire";
        }

        public override Sprite getIcon()
        {
            return EventManager.getImg("insect.fungalHive.png");
        }
    }
}
