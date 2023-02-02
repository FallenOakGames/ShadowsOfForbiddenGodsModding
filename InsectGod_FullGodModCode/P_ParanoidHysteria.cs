using Assets.Code;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class P_ParanoidHysteria : Power
    {
        public P_ParanoidHysteria(Map map) : base(map)
        {

        }

        public override int getCost()
        {
            return 2;
        }

        public override string getDesc()
        {
            return "Makes a hero or acolyte with 100% Infection Maturity gain 10 <b>menace</b> and cause madness effect `Paranoid Hysteria' or `Hypochondria' on the location. Adds 1% to temporary <b>World Panic</b>";
        }

        public override string getFlavour()
        {
            return "Paranoid Hysteria";
        }

        public override Sprite getIconFore()
        {
            //For legal reasons, the image can't be shared in the mod example repository, so has been built directly into the game
            //Normally a mod would use the Event system, such as EventManager.getImg("insect.god_background.jpg"); to reference an image
            return map.world.iconStore.insect_paranoia;
        }

        public override string getName()
        {
            return "Paranoid Hysteria";
        }

        public override string getRestrictionText()
        {
            return "Must target a hero or acolyte with 100% Infection Maturity in a human settlement with <b>Infected Populace</b> no greater than 50%. Cannot target the Chosen One";
        }

        public override bool validTarget(Unit unit)
        {
            if (unit == map.awarenessManager.getChosenOne()) { return false; }

            if (unit.person != null && (unit.location.settlement is SettlementHuman hum)){
                foreach (Property pr in unit.location.properties)
                {
                    if (pr is Pr_InfectedPopulace)
                    {
                        if (pr.charge > 50) { return false; }
                    }
                }
                foreach (Trait t in unit.person.traits)
                {
                    if (t is T_Infection infest)
                    {
                        return infest.maturation >= 100;
                    }
                }
            }
            return false;
        }

        public override bool validTarget(Location loc)
        {

            return false;
        }


        public override void cast(Unit unit)
        {
            base.cast(unit);

            if (unit is UA ua)
            {
                ua.addMenace(10);
                //Property.addToPropertySingleShot("Paranoid Hysteria", Property.standardProperties.UNREST, 25, ua.location);
                map.overmind.panicTemporaryChange += 0.01;

                Pr_MadnessEffect effect = new Pr_MadnessEffect(ua.location);
                int q = Eleven.random.Next(2);
                if (q == 0)
                {
                    effect.form = 4;
                }
                if (q == 1)
                {
                    effect.form = 5;
                }
                ua.location.properties.Add(effect);
                effect.onStart();
                World.log("Hysteria effect started: " + effect.form);
            }
        }
    }
}
