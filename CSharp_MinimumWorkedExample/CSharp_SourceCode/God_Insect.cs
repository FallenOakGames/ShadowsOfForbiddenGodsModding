using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class God_Insect : God
    {
        public int[] agentCaps = new int[] { 2, 3, 4, 5, 6, 7 };
        public int[] sealLevels = new int[] { 20, 40, 100, 150, 200, 300 };


        public override string getName()
        {
            return "Insect God";
        }

        public override void setup(Map map)
        {
            base.setup(map);

            powers.Add(new P_Speed(map));
            powerLevelReqs.Add(0);
        }


        public override Sprite getGodPortrait(World world)
        {
            return EventManager.getImg("insect.god_portrait.png");
        }

        public override Sprite getGodBackground(World world)
        {
            return EventManager.getImg("insect.god_background.jpg");
        }

        public override int[] getAgentCaps()
        {
            return agentCaps;
        }

        public override string getAwakenMessage()
        {
            return "Insect god awaken message";
        }

        public override string getDescFlavour()
        {
            return "A god of insects. A god made of insects. A god for insects.";
        }

        public override string getDescMechanics()
        {
            return "Swarming nightmares";
        }

        public override int getMaxTurns()
        {
            return 500;
        }

        public override List<Power> getPowers()
        {
            return powers;
        }

        public override int[] getSealLevels()
        {
            return sealLevels;
        }

        public override double getWorldPanicOnAwake()
        {
            return 1.0;
        }

        public override Sprite getSupplicant()
        {
            return EventManager.getImg("insect.iconWorm.png");
        }

        public override bool hasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> getSupplicantStartingTraits()
        {
            List<Trait> startingTraits = new List<Trait>();
            startingTraits.Add(new T_InsectTrait());
            return startingTraits;
        }
    }
}
