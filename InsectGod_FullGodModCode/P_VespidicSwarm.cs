using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_VespidicSwarm : Power
    {
        public P_VespidicSwarm(Map map) : base(map)
        {
        }

        public override int getCost()
        {
            return 0;
        }

        public override string getName()
        {
            return "Vespidic Swarm";
        }

        public override string getDesc()
        {
            return "Creates a <b>Vespidic Swarm</b> army, which can be semi-commanded by giving them an attack target on the map. Consumes all <b>Larval Mass</b> and gains HP based on mass consumed";
        }

        public override string getFlavour()
        {
            return "The captured humans feed the growing mass of horrors, the insects of the land mutating into ox sized abominations, buzzing towards the cities, blotting the sun with a grim swarm of death.";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.unit_insect_vespid;
        }


        public override string getRestrictionText()
        {
            return "Must target a location with <b>Larval Mass</b> above " + God_Insect.DRONE_VESPIDAE_COST + "%";
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
                    return larvae.charge > God_Insect.DRONE_VESPIDAE_COST;
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
                    int size = (int)(larvae.charge);
                    larvae.charge = 0;
                    UM_Vespidic_Swarm swarm = new UM_Vespidic_Swarm(loc, sg, size);
                    map.units.Add(swarm);
                    loc.units.Add(swarm);

                    if (map.overmind.god is God_Insect god && (!god.hasSetSwarmTarget))
                    {
                        god.hasSetSwarmTarget = true;
                        god.vespidSwarmTarget = loc;
                    }
                }
            }
        }
    }
}
