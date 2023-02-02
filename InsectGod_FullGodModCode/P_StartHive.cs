using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_StartHive : Power
    {
        public P_StartHive(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 0;
        }

        public override string getDesc()
        {
            return "Turns a hero or agent with a mature infection into a <b>Hive</b>, letting you spawn drones and military units";
        }

        public override string getFlavour()
        {
            return "Insect scuttle";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.fungalHive.png");
        }

        public override string getName()
        {
            return "Start Hive";
        }

        public override string getRestrictionText()
        {
            return "Must target a hero or agent with mature infection on an empty location (or location with city ruins)";
        }

        public override bool validTarget(Unit unit)
        {
            if (unit.person != null && (!unit.location.isOcean) && (unit.location.settlement == null || unit.location.settlement is Set_CityRuins) && unit.location.soc == null){
                foreach (Trait t in unit.person.traits)
                {
                    if (t is T_Infection infest)
                    {
                        return infest.maturation >= 100;
                    }
                }
            }
            return false;
        }

        public override bool validTarget(Location loc)
        {

            return false;
        }


        public override void cast(Unit unit)
        {
            base.cast(unit);

            if (unit.map.overmind.god is God_Insect god)
            {
                Location loc = unit.location;
                unit.die(map, "Erupted into Fungus");

                SG_Swarm swarm = new SG_Swarm(map, loc);
                if (god.swarm == null)
                {
                    god.swarm = swarm;
                };
                loc.settlement = new Set_Hive(loc);
                loc.soc = swarm;

                for (int i = 0; i < 1; i++)
                {
                    Person_Nonunique non = Person_Nonunique.getNonuniquePerson(map.soc_dark);
                    UAEN_Drone testDrone = new UAEN_Drone(loc, swarm, non);
                    map.units.Add(testDrone);
                    loc.units.Add(testDrone);
                }
            }
        }
    }
}
