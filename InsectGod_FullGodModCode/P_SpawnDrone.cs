using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_SpawnDrone : Power
    {
        public P_SpawnDrone(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 0;
        }

        public override string getName()
        {
            return "Arthropod Drone";
        }

        public override string getDesc()
        {
            return "Drones will randomly explore, seeking infected population. Once they find them, they steal them to carry back to the hive, to grow more larval mass.";
        }

        public override string getFlavour()
        {
            return "Fungal infection on smaller insects allows them to grow to the size of a bull, and larvae can then infiltrate the insect's body, letting the Cordyceps infection to control the creature to harvest more victims to consume. ";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_drone_arthopod;
        }


        public override string getRestrictionText()
        {
            return "Must target a location with <b>Larval Mass</b> above " + God_Insect.DRONE_LARVAL_COST + "%";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_LarvalMass larvae)
                {
                    return larvae.charge > God_Insect.DRONE_LARVAL_COST;
                }
            }
            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_LarvalMass larvae)
                {
                    SocialGroup sg = map.soc_dark;
                    if (loc.soc is SG_Swarm)
                    {
                        sg = loc.soc;
                    }
                    larvae.charge -= God_Insect.DRONE_LARVAL_COST;
                    Person_Nonunique non = Person_Nonunique.getNonuniquePerson(map.soc_dark);
                    UAEN_Drone drone = new UAEN_Drone(loc, sg, non);
                    map.units.Add(drone);
                    loc.units.Add(drone);
                }
            }
        }
    }
}
