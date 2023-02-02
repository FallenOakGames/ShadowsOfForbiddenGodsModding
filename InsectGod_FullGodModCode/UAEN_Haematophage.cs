using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class UAEN_Haematophage : UAEN
    {
        public int age;

        public UAEN_Haematophage(Location loc,SocialGroup soc,Person non) : base(loc, soc,non)
        {
            person.alert_halfShadow = true;
            person.alert_maxShadow = true;
            person.alert_aware = true;
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);

            age += 1;
            if (age % 14 == 0)
            {
                for (int i = 0; i < minions.Length; i++)
                {
                    if (minions[i] == null)
                    {
                        minions[i] = new M_Carapace(map);
                        break;
                    }
                }
            }
        }
        public override void turnTickAI()
        {
            if (this.hp < this.maxHp)
            {
                if (this.location.index == homeLocation)
                {
                    task = new Task_SlowHealing();
                    return;
                }
                else
                {
                    task = new Task_GoToLocation(map.locations[homeLocation]);
                    return;
                }
            }
            int c = 0;
            UA target = null;
            foreach (Unit u in map.units)
            {
                if (u is UAG ua && (!u.isCommandable()) && u.hp == u.maxHp && (map.getStepDist(map.locations[homeLocation],u.location) < 4))
                {
                    int dmgInbound = ua.getStatAttack();
                    if (ua.minions[0] != null) { dmgInbound += ua.minions[0].getAttack(); }

                    if (this.minions[0] == null && dmgInbound >= this.hp) { continue; }//Avoid getting oneshot

                    c += 1;
                    if (Eleven.random.Next(c) == 0)
                    {
                        target = ua;
                    }
                }
            }
            if (target != null)
            {
                task = new Task_AttackUnit(this, target);
                return;
            }
        }


        public override Sprite getPortraitForeground()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_haematophage;
        }
        public override Sprite getPortraitForegroundAlt()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_haematophage;
        }

        public override string getName()
        {
            return "Haematophage";
        }
        public override bool definesName()
        {
            return true;
        }

        public override int[] getPositiveTags()
        {
            return new int[] { ModCore.INSECT };
        }
    }
}
