using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_MotorFunctionOverride : Power
    {
        public P_MotorFunctionOverride(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 1;
        }

        public override string getDesc()
        {
            return "Cause the nearest hero with a mature (100%) infection to move to this location, so they can be used to construct a <b>Hive</b>";
        }

        public override string getFlavour()
        {
            return "By infecting the brain-stem, the parasitic fungus takes control of its victim, and compels them to walk out of civilisation, into the wilderness, seeking a clear spot from which the fungus can erupt, developing into the next stage of its life-cycle. The host becomes a vessel for the giant fungal stalk which serves as hive and spore-spreader";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_motorFunctionTakeover;
        }

        public override string getName()
        {
            return "Motor Function Takeover";
        }

        public override string getRestrictionText()
        {
            return "Must target an empty non-ocean location";
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if ((!loc.isOcean) && (loc.settlement == null || loc.settlement is Set_CityRuins) && loc.soc == null)
            {
                foreach (Unit u in map.units)
                {
                    if (u.isCommandable()) { continue; }
                    if (u is UAEN) { continue; }
                    if (u is UA)
                    {
                        foreach (Trait t in u.person.traits)
                        {
                            if (t is T_Infection infest && infest.maturation == 100) { return true; }
                        }
                    }
                }
            }
            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            int minDist = -1;
            Unit closest = null;
            foreach (Unit u in map.units)
            {
                if (u.location == loc) { continue; }//We're presumably not trying to draw someone here who's already here
                if (u.isCommandable()) { continue; }
                if (u is UAEN) { continue; }
                if (u is UA)
                {
                    foreach (Trait t in u.person.traits)
                    {
                        if (t is T_Infection infest && infest.maturation == 100) {
                            int dist = map.getStepDist(u.location, loc);
                            if (dist < minDist || closest == null)
                            {
                                minDist = dist;
                                closest = u;
                            }
                        }
                    }
                }
            }

            if (closest != null)
            {
                map.popMsg(closest.getName() + " has been compelled to travel to " + loc.getName());
                closest.task = new Task_GoToLocation(loc);
                return;
            }
        }
    }
}
