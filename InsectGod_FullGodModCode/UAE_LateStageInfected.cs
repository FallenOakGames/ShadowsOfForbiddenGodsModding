using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class UAE_LateStageInfected : UAE
    {
        public UAE_LateStageInfected(Location loc,Society soc) : base(loc, soc)
        {
            //this.rituals.Add(new Rti_AgentUniqueAbility(loc));

            T_Infection infection = new T_Infection();
            infection.maturation = 100;
            person.receiveTrait(infection);

            this.person.XP = this.person.XPForNextLevel;
        }

        public override bool isCommandable()
        {
            return true;
        }

        public override Sprite getPortraitForeground()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_lateStage;
        }
        public override Sprite getPortraitForegroundAlt()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.textureStore.agent_insect_lateStage;
        }

        public override bool definesName()
        {
            return false;
        }

        public override bool definesTitle()
        {
            return true;
        }

        public override string getTitle(Person p)
        {
            return "Infected";
        }

        public override bool hasStartingTraits()
        {
            return true;
        }
        public override List<Trait> getStartingTraits()
        {
            List<Trait> traits = new List<Trait>();
            traits.Add(new T_TheScentOfPrey());
            return traits;
        }
    }
}
