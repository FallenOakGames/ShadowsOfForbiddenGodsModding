using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class UAEN_Drone : UAEN
    {
        public int prey;

        public UAEN_Drone(Location loc,SocialGroup soc,Person non) : base(loc, soc,non)
        {
            person.alert_halfShadow = true;
            person.alert_maxShadow = true;
            person.alert_aware = true;
        }

        public override void turnTickAI()
        {
            if (map.overmind.god is God_Insect insect)
            {
                if (this.location.settlement is Set_Hive hive)
                {
                    //We're at home

                    //Drop off prey
                    hive.lavae.charge += prey;
                    insect.eat(prey);
                    prey = 0;

                    //See if you can hunt anything else. If not, explore
                    if (Eleven.random.NextDouble() < 0.8 && insect.phFeed[locIndex].charge > 1)
                    {
                        task = new Task_SeekPrey();
                    }
                    else
                    {
                        task = new Task_Explore();
                    }
                }
                else
                {
                    task = new Task_GoHome();
                }
            }
        }


        public override Sprite getPortraitForeground()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            if (person is Person_Nonunique)
            {
                return map.world.textureStore.agent_insect_drone_arthopod;
            }
            else
            {
                return map.world.textureStore.agent_insect_drone;
            }
        }
        public override Sprite getPortraitForegroundAlt()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            if (person is Person_Nonunique)
            {
                return map.world.textureStore.agent_insect_drone_arthopod;
            }
            else
            {
                return map.world.textureStore.agent_insect_drone;
            }
        }

        public override string getName()
        {
            if (person is Person_Nonunique)
            {
                return "Arthropodic Drone (" + prey + ")";
            }
            else
            {
                return "Drone " + person.firstName + " (" + prey + ")";
            }
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
