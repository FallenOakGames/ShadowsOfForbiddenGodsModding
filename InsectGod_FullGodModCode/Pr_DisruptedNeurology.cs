using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Pr_DisruptedNeurology : Property
    {
        public List<Challenge> challenges = new List<Challenge>();

        public Pr_DisruptedNeurology(Location loc) : base(loc)
        {
            charge = 0;
        }

        public override string getName()
        {
            return "Disrupted Neurology";
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override void turnTick()
        {
            base.turnTick();

            influences.Add(new ReasonMsg("Dissipation", -1));
        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override string getDesc()
        {
            return "Guards in this location are experiencing difficulty thinking, <b>Security</b> is reduced by 1";
        }

        public override bool deleteOnZero()
        {
            return true;
        }
        public override Sprite getSprite(World world)
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_disruptedNeurology;
        }

        public override bool survivesRuin()
        {
            return false;
        }

        public override int getSecurityChange(SettlementHuman hum)
        {
            return -1;
        }
    }
}
