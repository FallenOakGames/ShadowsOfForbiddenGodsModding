using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class M_Carapace : Minion
    {
        public M_Carapace(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Carapace";
        }

        public override int getAttack()
        {
            return 0;
        }

        public override int getMaxDefence()
        {
            return 2;
        }

        public override int getMaxHP()
        {
            return 1;
        }

        public override int getCommandCost()
        {
            return 0;
        }

        public override Sprite getIcon()
        {
            return map.world.iconStore.insect_carapace;
        }
        public override Sprite getIconBack()
        {
            return base.getIconBack();
        }
    }
}
