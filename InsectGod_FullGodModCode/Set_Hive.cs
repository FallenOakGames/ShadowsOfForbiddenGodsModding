using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Set_Hive : Settlement
    {
        public Pr_LarvalMass lavae;
        public bool airbornSpores;
        public Sub_HiveSpire spire;

        public Set_Hive(Location loc) : base(loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_LarvalMass mass)
                {
                    lavae = mass;
                }
            }
            if (lavae == null)
            {
                lavae = new Pr_LarvalMass(loc, this);
                loc.properties.Add(lavae);
            }

            shadowPolicy = shadowResponse.FULL_FLOW;

            spire = new Sub_HiveSpire(this);
            subs.Add(spire);
        }

        public override string getName()
        {
            return "Hive";
        }

        public override void turnTick()
        {
            base.turnTick();

            if (airbornSpores)
            {
                spire.menace += 1;
                foreach (Unit u in location.units)
                {
                    if (u is UA ua)
                    {
                        bool infected = false;
                        foreach (Trait t in ua.person.traits)
                        {
                            if (t is T_Infection) { infected = true;break; }
                        }
                        if (!infected)
                        {
                            T_Infection inf = new T_Infection();
                            ua.person.receiveTrait(inf);
                        }
                    }
                }

                foreach (Location loc in map.locations)
                {
                    int dist = map.getStepDist(loc, this.location);
                    if (dist > 2) { continue; }
                    if (loc.settlement is SettlementHuman)
                    {
                        bool assigned = false;
                        foreach (Property pr in loc.properties)
                        {
                            if (pr is Pr_InfectedPopulace)
                            {
                                assigned = true;
                                pr.influences.Add(new ReasonMsg("Airborne Spores", 1));
                            }
                        }
                        if (!assigned)
                        {
                            Pr_InfectedPopulace inf = new Pr_InfectedPopulace(loc);
                            loc.properties.Add(inf);
                            inf.influences.Add(new ReasonMsg("Airborne Spores", 1));
                        }
                    }
                }
            }
        }

        public override Sprite getSprite()
        {
            if (airbornSpores)
            {
                return EventManager.getImg("insect.loc_minor_fungus_red.png");
            }
            return EventManager.getImg("insect.loc_minor_fungus.png");
        }
    }
}
