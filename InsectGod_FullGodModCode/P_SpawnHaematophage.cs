using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_Haematophage : Power
    {
        public P_Haematophage(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 3;
        }

        public override string getName()
        {
            return "Haematophage";
        }

        public override string getDesc()
        {
            return "Ideal for harassment and disruption, the Haematophage will attack heroes who come near its territory (where it was created). It will retreat after a single attack, causing heroes to need to heal up without killing them an increasing world panic. It will slowly build defensive minions, but if you spawn it at a hive you can assign a guard minion to make it attack more often.";
        }

        public override string getFlavour()
        {
            return "Arachnids the size of wolves prowl the shadows, seeking live prey. Able to hide themselves in the tightest of places, they squeeze into roofing spaces or under floorboards while the sun is up, and stalk by night. Their attacks are rarely lethal, but their constant parasitic assaults leave their victims weakened, shaken and disturbed.";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_haematophage;
        }


        public override string getRestrictionText()
        {
            return "Can be cast in any location";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            Person_Nonunique non = Person_Nonunique.getNonuniquePerson(map.soc_dark);
            UAEN_Haematophage drone = new UAEN_Haematophage(loc, map.soc_dark, non);
            map.units.Add(drone);
            loc.units.Add(drone);
        }
    }
}
