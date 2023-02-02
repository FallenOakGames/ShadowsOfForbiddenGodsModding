using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Pr_LarvalMass : Property
    {
        public List<Challenge> challenges = new List<Challenge>();

        public Set_Hive hive;

        public Pr_LarvalMass(Location loc, Set_Hive set_Hive) : base(loc)
        {
            challenges.Add(new Ch_RemoveInsectStuff(loc,this));
            hive = set_Hive;
            charge = 0;
        }

        public override string getName()
        {
            return "Larval Mass";
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override bool deleteOnZero()
        {
            return false;
        }

        public override string getDesc()
        {
            return "Larval Mass";
        }


        public override Sprite getSprite(World world)
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_larvalMass;
        }
    }
}
