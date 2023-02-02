using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class M_VespidicGuard : Minion
    {
        public M_VespidicGuard (Map m) : base(m)
        {

        }

        public override string getName()
        {
            return "Vespidic Guard";
        }

        public override int getAttack()
        {
            return 4;
        }

        public override int getCommandCost()
        {
            return 1;
        }

        public override int getMaxHP()
        {
            return 3;
        }

        public override int getMaxDefence()
        {
            return 3;
        }

        public override Sprite getIcon()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.unit_insect_vespid;
        }
    }
}
