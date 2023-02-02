using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class Pr_Pheromone_Feeding : Property
    {
        public List<Challenge> challenges = new List<Challenge>();

        public God_Insect parent;

        public Pr_Pheromone_Feeding(Location loc, God_Insect parent) : base(loc)
        {
            this.parent = parent;
            charge = 0;
        }

        public override string getName()
        {
            return "Pheromone: Feeding";
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override void turnTick()
        {
            base.turnTick();

            influences.Add(new ReasonMsg("Dissipation", God_Insect.PHEROMONE_DECAY_FACTOR * charge));
        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override string getDesc()
        {
            return "Drones seeking prey will follow this pheromone to its source";
        }

        public override bool deleteOnZero()
        {
            return false;
        }
        public override Sprite getSprite(World world)
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_pheromoneFeed;
        }

        public override bool survivesRuin()
        {
            return true;
        }
    }
}
