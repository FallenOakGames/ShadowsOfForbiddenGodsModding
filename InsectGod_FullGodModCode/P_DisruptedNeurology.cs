using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_DisruptedNeurology : Power
    {
        public P_DisruptedNeurology(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 1;
        }

        public override string getDesc()
        {
            return "Causes the guards to lose focus, decreasing <b>Security</b> by 1 for 25 turns (stackable)";
        }

        public override string getFlavour()
        {
            return "As the parasitic fungus takes hold of the host's body, it infiltrates the brain functions, slowly forcing the human to follow the will of the Cordyceps Hive Mind.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.insect_disruptedNeurology;
        }

        public override string getName()
        {
            return "Disrupted Neurology";
        }

        public override string getRestrictionText()
        {
            return "Must target a human settlement with <b>Infected Poplace</b> > 25%";
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
                    if (pr is Pr_InfectedPopulace && pr.charge >= 25)
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

            Pr_DisruptedNeurology disruption = new Pr_DisruptedNeurology(loc);
            disruption.charge = 25;
            loc.properties.Add(disruption);
        }
    }
}
