using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class UAE_Abs_DemoAgent : UAE_Abstraction
    {
        public UAE_Abs_DemoAgent(Map map) : base(map, -1)
        {

        }

        public override void createAgent(Location target)
        {
            var agent = new UAE_LateStageInfected(target, target.map.soc_dark);
            agent.person.stat_might = this.getStatMight();
            agent.person.stat_lore = this.getStatLore();
            agent.person.stat_intrigue = this.getStatIntrigue();
            agent.person.stat_command = this.getStatCommand();
            target.units.Add(agent);
            map.units.Add(agent);
            GraphicalMap.selectedUnit = agent;

            map.overmind.availableEnthrallments -= 1;
        }

        public override Sprite getForeground()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_lateStage;
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
            return "Can be placed in any location with an infected population";
        }

        public override bool validTarget(Location loc)
        {
            if (World.allowAllAgents) { return true; }
            if (map.world.map.overmind.nEnthralled >= map.world.map.overmind.getAgentCap()) { return false; }

            if (loc.settlement is SettlementHuman)
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_InfectedPopulace && pr.charge > 0) { return true; }
                }
            }
            return false;
        }

        public override string getName()
        {
            return "Late-Stage Infection Victim";
        }

        public override int getStatIntrigue()
        {
            return 3;
        }
        public override int getStatMight()
        {
            return 3;
        }
        public override int getStatLore()
        {
            return 2;
        }
        public override int getStatCommand()
        {
            return 2;
        }
    }
}
