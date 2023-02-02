using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class UM_Vespidic_Swarm : UM
    {
        public UM_Vespidic_Swarm(Location loc,SocialGroup sg,int size) : base(loc, sg)
        {
            this.maxHp = size;
            this.hp = maxHp;
        }

        public override string getName()
        {
            return "Vespidic Swarm";
        }

        public override Sprite getPortraitForeground()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.unit_insect_vespid;
        }
        public override Sprite getPortraitForegroundAlt()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.unit_insect_vespid;
        }

        public override void turnTickAI()
        {
            base.turnTickAI();

            //First off, see if you need to join a swarm battle
            int minDist = -1;
            UM bestAllyTarget = null;
            foreach (Unit u in map.units)
            {
                if (u is UM_Vespidic_Swarm um && u.task is Task_InBattle)
                {
                    int dist = map.getStepDist(this.location, u.location);
                    if (dist > 5) { continue; }
                    if (dist < minDist || bestAllyTarget == null)
                    {
                        bestAllyTarget = um;
                        minDist = dist;
                    }
                }
            }
            if (bestAllyTarget != null && bestAllyTarget.task is Task_InBattle battle)
            {
                //A friend is under attack nearby, move to assist
                if (battle.battle.attackers.Contains(bestAllyTarget))
                {
                    //Join on the side of the attackers
                    if (battle.battle.defenders.Count > 0)
                    {
                        task = new Task_AttackArmy(battle.battle.defenders[0],this);
                        return;
                    }
                }
                else
                {
                    //Join on the side of the defenders
                    if (battle.battle.attackers.Count > 0)
                    {
                        task = new Task_AttackArmy(battle.battle.attackers[0], this);
                        return;
                    }
                }
            }

            //Secondly, see if there's something nearby to kill

            //If there's a location under us, we were probably meant to kill it
            if (this.location.settlement is SettlementHuman)
            {
                Task_RazeLocation raze = new Task_RazeLocation();
                raze.ignorePeace = true;
                task = raze;
                return;
            }

            if (map.overmind.god is God_Insect god) {
                minDist = -1;
                Location closestLoc = null;
                int c = 0;
                foreach (Location loc in map.locations)
                {
                    if (loc.settlement is SettlementHuman)
                    {
                        //Gotta be the closest to us within the god's targetting 
                        if ((closestLoc == null || map.getStepDist(loc, this.location) < minDist) && (map.getStepDist(loc,god.vespidSwarmTarget) < 3))
                        {
                            minDist = map.getStepDist(loc, this.location);
                            closestLoc = loc;
                            c = 1;
                        }
                        else if ((map.getStepDist(loc, this.location) == minDist) && (map.getStepDist(loc, god.vespidSwarmTarget) < 3))//Second check for tie-breaking
                        {
                            c += 1;
                            if (Eleven.random.Next(c) == 0)
                            {
                                closestLoc = loc;
                            }
                        }
                    }
                }
                if (closestLoc != null)
                {
                    task = new Task_GoToLocation(closestLoc);
                    return;
                }

                //If we're here we've not hit a 'return' statement yet, so must be doing nothing
                List<Location> opts = god.vespidSwarmTarget.getNeighbours();
                task = new Task_GoToLocation(opts[Eleven.random.Next(opts.Count)]);//Move randomly around the swarm target
                task.turnTick(this);//Perform a step to keep us moving rapidly (normally you lose a turn to AI processing)
                return;
            }

        }
    }
}
