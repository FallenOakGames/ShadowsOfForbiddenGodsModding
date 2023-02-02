using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_InternalMaturation : Power
    {
        public P_InternalMaturation(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 3;
        }

        public override string getDesc()
        {
            return "Causes <b>Vespidic Swarms</b> to explode out of infected victims. Creates a Vespidic Swarm army in a location, with strength proportional to population and level of infection. Removes population and defences.";
        }

        public override string getFlavour()
        {
            return "The creatures grow inside the bodies of their victims, slowly replacing the internal organs of their victims, the biological function being replaced by those of the maturing Vespid. Eventually, the host is barely more than a shell around a lethal predatory insect-like abomination, ready to burst free.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("insect.ParasiteWithin.png");
        }

        public override string getName()
        {
            return "Internal Maturation";
        }

        public override string getRestrictionText()
        {
            return "Must target a human settlement with <b>Infected Poplace</b> > 50%";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if ((loc.settlement is SettlementHuman))
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_InfectedPopulace && pr.charge >= 50)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            if (loc.settlement is SettlementHuman hum && map.overmind.god is God_Insect god) {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_InfectedPopulace && pr.charge >= 50)
                    {

                        int size = (int)Math.Min(50,(pr.charge * 0.01 * hum.population));
                        hum.population -= size;
                        hum.addDefence(-size);
                        if (hum.population <= 0) { hum.fallIntoRuin("Population erupted into parasites"); }

                        
                        SG_Swarm swarmSG;
                        if (god.swarm == null)
                        {
                            god.swarm = new SG_Swarm(map, loc);
                        }
                        swarmSG = god.swarm;

                        UM_Vespidic_Swarm swarm = new UM_Vespidic_Swarm(loc, swarmSG, size);
                        map.units.Add(swarm);
                        loc.units.Add(swarm);

                        if ((!god.hasSetSwarmTarget))
                        {
                            god.hasSetSwarmTarget = true;
                            god.vespidSwarmTarget = loc;
                        }

                        return;
                    }
                }
            }
        }
    }
}
