using System;
using System.Collections.Generic;
using System.Text;
using Assets.Code;
using UnityEngine;

namespace ShadowsInsectGod.Code
{
    public class God_Insect : God
    {
        public static double MENACE_FROM_HARVEST = 7;
        public static double PROFILE_FROM_HARVEST = 4;
        public static double PHEROMONE_DECAY_FACTOR = -0.075;
        public static double PHEROMONE_PLACE_STRENGTH_HOME = 12;
        public static double PHEROMONE_PLACE_STRENGTH_FEED = 2;
        public static double P_TRAIT_SPREAD = 0.025;
        public static int INFECTION_MAX_MATURATION = 100;
        public static int DRONE_LARVAL_COST = 10;
        public static int DRONE_VESPIDAE_COST = 10;
        public static double POPULATION_INFECTION_RATE = 2;
        public static int MAX_VESPID_STRENGTH = 35;

        public Pr_Pheromone_Feeding[] phFeed;
        public Pr_Pheromone_Home[] phHome;

        public SG_Swarm swarm;
        public Location vespidSwarmTarget;
        public bool hasSetSwarmTarget;

        public override string getName()
        {
            return "Cordyceps Hive Mind";
        }
        public override void setup(Map map)
        {
            base.setup(map);

            powers.Add(new P_StartHive(map));
            powerLevelReqs.Add(0);
            powers.Add(new P_MotorFunctionOverride(map));
            powerLevelReqs.Add(0);
            powers.Add(new P_SpawnDroneFromHuman(map));
            powerLevelReqs.Add(0);
            powers.Add(new P_SpawnDrone(map));
            powerLevelReqs.Add(0);
            powers.Add(new P_HomingInstinct(map));
            powerLevelReqs.Add(0);

            //First seal: Help Drones a bit
            powers.Add(new P_AssignGuard(map));
            powerLevelReqs.Add(1);
            powers.Add(new P_DisruptedNeurology(map));
            powerLevelReqs.Add(1);

            //Second seal: Arachnids
            powers.Add(new P_Haematophage(map));
            powerLevelReqs.Add(2);
            powers.Add(new P_RemoveHive(map));
            powerLevelReqs.Add(2);

            //Second seal: Can initially start the vespid swarm
            powers.Add(new P_VespidicSwarm(map));
            powerLevelReqs.Add(3);
            powers.Add(new P_VespidSwarmTarget(map));
            powerLevelReqs.Add(3);

            //Third: Add way to make hives spread infection nearby
            powers.Add(new P_AirborneSpores(map));
            powerLevelReqs.Add(4);
            powers.Add(new P_SpawnAllPossibleDrones(map));
            powerLevelReqs.Add(4);

            //Awaken:
            powers.Add(new P_ParanoidHysteria(map));
            powerLevelReqs.Add(5);
            powers.Add(new P_InternalMaturation(map));
            powerLevelReqs.Add(5);


            //Awaken: Explode people into worms
        }

        public override void onStart(Map map)
        {
            base.onStart(map);

            vespidSwarmTarget = map.locations[0];

            phFeed = new Pr_Pheromone_Feeding[map.locations.Count];
            phHome = new Pr_Pheromone_Home[map.locations.Count];
            foreach (Location loc in map.locations)
            {
                phFeed[loc.index] = new Pr_Pheromone_Feeding(loc, this);
                loc.properties.Add(phFeed[loc.index]);
                phHome[loc.index] = new Pr_Pheromone_Home(loc, this);
                loc.properties.Add(phHome[loc.index]);
            }

            foreach (Unit u in map.units)
            {
                if (u is UAE_Supplicant supp)
                {
                    T_Infection t = new T_Infection();
                    t.maturation = 100;
                    supp.person.receiveTrait(t);
                }
            }
        }

        public override string getDescMechanics()
        {
            return "The first stage is infection of individuals. Agents, heroes, rulers. They then spread it to others, and to the human populations. The second stage involves drones carrying away the paralysed population of undefended villages back to their hives to be devoured. The last stage involves these hives disgorging swarms of lethal predatory mutated insects to destroy entire kingdoms";
        }
        public override string getDescFlavour()
        {
            return "A vast hive-mind, each spore of this fungal entity is linked together into a single consciousness. It exists parasitically, infecting and mutating creatures, taking control of their nervous systems and driving them towards its own goals or paralysing them for consumption by its enslaved victims. Fungal spores spread by the wind, often from nightmarish fungal stalks growing from former victims bodies";
        }
        public override string getDetailedMechanics()
        {
            string msg = "";
            msg += "<b>Stage 1: Infection</b>";
            msg += "\nInitially, this god revolves around infecting rulers, then human populations, with the Cordyceps fungus. Heroes who are infected will gain infection maturity, until you can completely destroy them, turning them into <b>Hives</b> and <b>Drones</b>. Your own agents start infected, so are ready-made feedstock to serve your needs";
            msg += "\n\n<b>Stage 2: Harvesting</b>";
            msg += "\nInfected human populations can be paralysed and carried away by <b>Drones</b>, who will take them back to the <b>Hives</b> to be turned into Larval Masses. These are how you break seals, and can be turned into more <b>Drones</b> to expand your colony.";
            msg += "\nDrones can't enter cities defended by armies if the location is below 50% shadow, but infected rulers won't rebuild lost armies, so triggering wars to destroy the armies can leave them defenceless against the swarming masses of the insects";
            msg += "\n\n<b>Stage 3: Swarming</b>";
            msg += "\nOnce sufficient larval mass has been accumulated, Vespidae Swarms can be created, which serve as armies, and will aggressively destroy human kingdoms, to ultimately remove humanity entirely";
            return msg;
        }


        public void eat(int amount)
        {
            if (awake) { return; }

            int[] levels = getSealLevels();
            int target = levels[map.overmind.sealsBroken];
            map.overmind.sealProgress += amount;

            if (map.overmind.sealProgress >= target)
            {
                map.overmind.sealsBroken += 1;
                breakSeal(map.overmind.sealsBroken);
            }
        }

        public override void turnTick()
        {
            base.turnTick();

            if (!awake)
            {
                int[] levels = getSealLevels();
                int target = levels[map.overmind.sealsBroken];
                if (map.overmind.sealProgress >= target)
                {
                    map.overmind.sealsBroken += 1;
                    breakSeal(map.overmind.sealsBroken);
                }
            }
        }

        public override int[] getSealLevels()
        {
            return new int[] { 10, 20, 40, 100, 200 };
        }

        public override int[] getAgentCaps()
        {
            return new int[] { 2, 3, 3, 4, 5, 5 };//Note this must be one longer than the seal levels, to account for the 'awakened' seal state
        }


        public override Sprite getGodPortrait(World world)
        {
            return EventManager.getImg("insect.god_portrait.png");
        }

        public override Sprite getGodBackground(World world)
        {
            return EventManager.getImg("insect.god_background.jpg");
        }

        public override Sprite getSupplicant()
        {
            return EventManager.getImg("insect.unit_insect_supplicant.png");
        }

        public override double getWorldPanicOnAwake()
        {
            return 0.25;
        }

        public override bool usesConventionalSeals()
        {
            return false;
        }

        public override string getPowerDesc()
        {
            return this.getName() + " grows as infected human populations are brought to the <b>Hives</b> by <b>Drones</b>. These paralysed victims serve as living incubators for the larvae, which add to the iocord fungal hive-mind's power, letting you employ more powers";
        }
        public override string getSealDesc()
        {
            return this.getName() + " grows as infected human populations are brought to the <b>Hives</b> by <b>Drones</b>. These paralysed victims serve as living incubators for the larvae, which add to the Cordyceps fungal hive-mind's power, letting you employ more powers";
        }
        public override string getSealUITextUpper()
        {
            return "Seals Broken: " + map.overmind.sealsBroken + " of " + this.getSealLevels().Length;
        }
        public override string getSealUITextLower()
        {

            int turnsLeft = getSealLevels()[map.overmind.sealsBroken] - map.overmind.sealProgress;
            return "Amount to next Seal " + turnsLeft;
        }

        public override string getAwakenMessage()
        {
            return "The parasites grow inside, and writhe around in the bodies of the victims. The air feels heavy with spores, and the nights are filled with sounds of mutated insect wings buzzing incessantly. Humanity recoils in disgust, horror and terror.";
        }

        public override bool hasSupplicantStartingTraits()
        {
            return true;
        }
        public override List<Trait> getSupplicantStartingTraits()
        {
            List<Trait> traits = new List<Trait>();
            traits.Add(new T_TheScentOfPrey());
            return traits;
        }
    }
}
