using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod
{
    public class Pr_InsectFamine : Property
    {
        public List<Challenge> challenges = new List<Challenge>();

        public Pr_InsectFamine(Location loc) : base(loc)
        {
            challenges.Add(new Ch_RemoveInsectStuff(loc));
        }

        public override string getName()
        {
            return "Insect famine";
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override string getDesc()
        {
            return "Insect famine";
        }

        public override double foodGenMult()
        {
            return Math.Max(0, 1 - (charge * 0.01));
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("insect.iconDeadFish.png");
        }
    }
}
