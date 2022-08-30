using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class UAE_Abs_DemoAgent : UAE_Abstraction
    {
        public UAE_Abs_DemoAgent(Map map) : base(map, -1)
        {

        }

        public override void createAgent(Location target)
        {
            var agent = new UAE_DemoAgent(target, target.map.soc_dark);
            agent.person.stat_might = this.getStatMight();
            agent.person.stat_lore = this.getStatLore();
            agent.person.stat_intrigue = this.getStatIntrigue();
            agent.person.stat_command = this.getStatCommand();
            target.units.Add(agent);
            map.units.Add(agent);
            GraphicalMap.selectedUnit = agent;
        }

        public override Sprite getForeground()
        {
            return EventManager.getImg("insect.iconDeadFish.png");
        }

        public override string getDesc()
        {
            return "A test agent for the insect god";
        }

        public override string getFlavour()
        {
            return "Tastes of insect";
        }

        public override string getRestrictions()
        {
            return "Can be placed in an empty location next to civilisation";
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement != null) { return false; }
            foreach (Location l2 in loc.getNeighbours())
            {
                if (l2.settlement is SettlementHuman)
                {
                    return true;
                }
            }
            return false;
        }

        public override string getName()
        {
            return "Test insect agent";
        }

        public override int getStatIntrigue()
        {
            return 2;
        }
        public override int getStatMight()
        {
            return 3;
        }
        public override int getStatLore()
        {
            return 3;
        }
        public override int getStatCommand()
        {
            return 4;
        }
    }
}
