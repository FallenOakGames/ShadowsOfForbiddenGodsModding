using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Pr_InfectedPopulace : Property
    {
        public List<Challenge> challenges = new List<Challenge>();


        public Pr_InfectedPopulace(Location loc) : base(loc)
        {
            charge = 0;
            challenges.Add(new Ch_InfectPopulace(loc,this));
            challenges.Add(new Ch_TreatPopulace(loc, this));
        }

        public override string getName()
        {
            return "Infected Populace";
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override void turnTick()
        {
            base.turnTick();
            if (charge > 100) { charge = 100; }

        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override string getDesc()
        {
            return "This population has been at least partially infected with the paralytic strain of the fungus, and is ready for drones to arrive to carry them away to be used to as food for the larval mass of the hives";
        }

        public override bool deleteOnZero()
        {
            return false;
        }
        public override Sprite getSprite(World world)
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_lateStage;
        }

        public override bool survivesRuin()
        {
            return false;
        }
    }
}
