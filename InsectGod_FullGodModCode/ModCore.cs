using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using static Assets.Code.EventRuntime;
using Property = Assets.Code.Property;

namespace ShadowsInsectGod.Code
{
    public class ModCore : Assets.Code.Modding.ModKernel
    {
        public static int INSECT;
        public Map map;
        public bool warningGiven = false;

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            gods.Add(new God_Insect());
            this.map = map;

            //Gotta add this even if the god isn't an insect, or the events will fail to find a target
            if (EventRuntime.fields.ContainsKey("is_agent_cordyceps") == false)
            {
                EventRuntime.fields.Add("is_agent_cordyceps", new TypedField<bool>(c => { if (c.unit == null) { return false; } return c.unit is UAEN_Drone || c.unit is UAE_LateStageInfected; }));
            }
        }

        public override void beforeMapGen(Map map)
        {
            //If they're not using the god, don't do anything
            if (map.overmind.god is God_Insect == false) { return; }

            base.beforeMapGen(map);

            int response = Tags.addTagEnemy("Cordyceps");
            if (response != -1)
            {
                //A -1 signal indicates we already added this tag
                INSECT = response;
            }
            map.overmind.agentsGeneric.Add(new UAE_Abs_DemoAgent(map));

        }

        public override void afterLoading(Map map)
        {
            base.afterLoading(map);

            //Gotta add this even if the god isn't an insect, or the events will fail to find a target
            if (EventRuntime.fields.ContainsKey("is_agent_cordyceps") == false)
            {
                EventRuntime.fields.Add("is_agent_cordyceps", new TypedField<bool>(c => { if (c.unit == null) { return false; } return c.unit is UAEN_Drone || c.unit is UAE_LateStageInfected; }));
            }

            //If they're not using the god, don't do anything
            if (map.overmind.god is God_Insect == false) { return; }

            //We need to ensure the tag loaded properly, because it's not inherent to the map
            int response = Tags.addTagEnemy("Cordyceps");
            if (response != -1)
            {
                //A -1 signal indicates we already added this tag
                INSECT = response;
            }
        }

        public override void afterMapGenAfterHistorical(Map map)
        {
            base.afterMapGenAfterHistorical(map);

            if (map.overmind.god is God_Insect == false) { return; }

            map.world.prefabStore.popMsgHint(map.overmind.god.getName() + " is a god focused on infecting humans, then exploiting the infected to eradicate them from the map. Your agents can initially spread infection, to rulers, then to heroes and human settlements." +
                "\n\nAfter some human populations have been infected you should create a Hive, using your power on an infected hero or agent, to begin harvesting the infected populace. Note that drones can't harvest from locations defended by an army, so either enshadow the city (preventing them from defending themselves), remove the army or harvest from outlying villages." +
                "\n\nAfter you have harvested enough, you can begin to eradicate humanity with Vespidic Swarms, taking advantage of the fact that infected rulers won't rebuild armies", map.overmind.god.getName());
        }

        public override void onTurnEnd(Map map)
        {
            //If they're not using the god, don't do anything
            if (map.overmind.god is God_Insect == false) { return; }

            base.onTurnEnd(map);

            if ((!warningGiven)  && map.worldPanic > 0.2)
            {
                foreach (Location loc in map.locations)
                {
                    if (loc.settlement is Set_TombOfGods)
                    {
                        warningGiven = true;
                        map.addUnifiedMessage(loc, null, "Fear and Paranoia", "As the panic spreads and grows, in response to the actions of the infected humans and insects, humanity's response will change. " +
                            "Those who are <b>Aware</b> will begin to fear travelling abroad, to avoid contracting or spreading the infection, and will be more likely to kill <b>Drones</b> on sight to try to stop the Cordyceps Hive Mind's replacement of the world's biosphere", "CORDYCEPS AWARENESS", true);
                        break;
                    }
                }
            }

            //Ensure all locations have the desired mod-specific challenges
            foreach (Location l in map.locations)
            {
                if (l.settlement is SettlementHuman hum)
                {
                    bool hasChallenge = false;
                    foreach (Challenge c in hum.customChallenges)
                    {
                        if (c is Ch_InfectRuler)
                        {
                            hasChallenge = true;
                            break;
                        }
                    }
                    if (!hasChallenge)
                    {
                        hum.customChallenges.Add(new Ch_InfectRuler(l));
                        hum.customChallenges.Add(new Ch_TargettedInfection(l));
                        hum.customChallenges.Add(new Ch_RemoveVector(l));
                    }
                }
            }
            //Ensure all our agents have the infection
            foreach (Unit u in map.units)
            {
                if (u is UA && u.isCommandable())
                {
                    bool hasInfection = false;
                    foreach (Trait t in u.person.traits)
                    {
                        if (t is T_Infection inf)
                        {
                            inf.maturation = 100;
                            hasInfection = true;
                        }
                    }
                    if (!hasInfection)
                    {
                        T_Infection infection = new T_Infection();
                        infection.maturation = 100;
                        u.person.receiveTrait(infection);
                    }
                }
            }

            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is HolyOrder order)
                {
                    bool hasTenet = false;
                    foreach (HolyTenet tenet in order.tenets)
                    {
                        if (tenet is H_InsectFaith)
                        {
                            hasTenet = true;
                            break;
                        }
                    }
                    if (!hasTenet)
                    {
                        H_InsectFaith faith = new H_InsectFaith(order);
                        order.tenets.Add(faith);
                    }
                }
            }
        }

        public override void populatingThreats(Overmind overmind, List<MsgEvent> threats)
        {
            //If they're not using the god, don't do anything
            if (map == null) { return; }//For tutorial situations
            if (map.overmind.god is God_Insect == false) { return; }

            base.populatingThreats(overmind, threats);


            Unit possibleVictim = null;
            foreach (Unit u in map.units)
            {
                if (u.isCommandable()) { continue; }
                if (u is UA)
                {
                    foreach (Trait t in u.person.traits)
                    {
                        if (t is T_Infection inf && inf.maturation == 100)
                        {
                            possibleVictim = u;
                            break;
                        }
                    }
                }
            }
            if (possibleVictim != null)
            {
                threats.Add(new MsgEvent("100% Mature Infection: " + possibleVictim.getName(), 0.8, true, possibleVictim.location.hex));
            }
        }

        public override double unitAgentAI(Map map, UA ua, Challenge c, List<ReasonMsg> reasons, double initialUtility)
        {
            //If they're not using the god, don't do anything
            if (map.overmind.god is God_Insect == false) { return base.unitAgentAI(map, ua, c, reasons, initialUtility); }

            if (ua.person.awareness > 0 && c.location.soc != ua.society)
            {
                double localU = ua.person.awareness * map.worldPanic * -50;
                initialUtility += localU;
                if (reasons != null)
                {
                    ReasonMsg msg = new ReasonMsg("Can't Risk Infection Spread", localU);
                    reasons.Add(msg);
                }
            }
            return initialUtility;
        }

        public override int shouldAIRetreatInBattle_ternary(UA us, UA them, bool amFirst, double u)
        {
            if (us is UAEN_Haematophage) { return 1; }
            if (them is UAEN_Haematophage) { return 1; }
            return 0;
        }

        public override void onAgentBattleTerminate(BattleAgents battle)
        {
            if (battle.att is UAEN_Haematophage)
            {
                battle.att.addMenace(10);
                battle.att.addProfile(10);
            }
        }

        public override void onCheatEntered(string command)
        {
            //If they're not using the god, don't do anything
            if (map.overmind.god is God_Insect == false) { return; }

            base.onCheatEntered(command);

            if (command == "hive")
            {
                if (map.overmind.god is God_Insect god)
                {
                    Location loc = GraphicalMap.selectedHex.location;

                    SG_Swarm swarm;
                    if (god.swarm == null)
                    {
                        god.swarm = new SG_Swarm(map, loc);
                    }
                    swarm = god.swarm;
                    loc.settlement = new Set_Hive(loc);
                    loc.soc = swarm;

                    for (int i = 0; i < 1; i++)
                    {
                        Person_Nonunique non = Person_Nonunique.getNonuniquePerson(map.soc_dark);
                        UAEN_Drone testDrone = new UAEN_Drone(loc, swarm, non);
                        map.units.Add(testDrone);
                        loc.units.Add(testDrone);
                    }
                }
            }
            if (command == "infectPop")
            {
                foreach (Property pr in GraphicalMap.selectedHex.location.properties)
                {
                    if (pr is Pr_InfectedPopulace)
                    {
                        GraphicalMap.selectedHex.location.properties.Remove(pr);
                        break;
                    }
                }
                Pr_InfectedPopulace infection = new Pr_InfectedPopulace(GraphicalMap.selectedHex.location);
                infection.charge = 100;
                GraphicalMap.selectedHex.location.properties.Add(infection);
            }
            if (command == "infectLow")
            {
                if (GraphicalMap.selectedUnit != null)
                {
                    GraphicalMap.selectedUnit.person.receiveTrait(new T_Infection());
                }
                else
                {
                    GraphicalMap.selectedHex.location.person().receiveTrait(new T_Infection());
                }
            }
            if (command == "infectHigh")
            {
                T_Infection inf = new T_Infection();
                inf.maturation = 100;
                if (GraphicalMap.selectedUnit != null)
                {
                    GraphicalMap.selectedUnit.person.receiveTrait(inf);
                }
                else
                {
                    GraphicalMap.selectedHex.location.person().receiveTrait(inf);
                }
            }
            if (command == "hiveSwarm")
            {
                if (map.overmind.god is God_Insect god)
                {
                    Location loc = GraphicalMap.selectedHex.location;

                    SG_Swarm swarmSG;
                    if (god.swarm == null)
                    {
                        god.swarm = new SG_Swarm(map, loc);
                    }
                    swarmSG = god.swarm;
                    Set_Hive hive = new Set_Hive(loc);
                    loc.settlement = hive;
                    loc.soc = swarmSG;
                    hive.lavae.charge = 100;

                    for (int i = 0; i < 4; i++)
                    {
                        UM_Vespidic_Swarm swarm = new UM_Vespidic_Swarm(loc, swarmSG,30);
                        map.units.Add(swarm);
                        loc.units.Add(swarm);
                        
                    }
                    god.vespidSwarmTarget = loc;
                }
            }
            if (command == "insectEat")
            {
                if (map.overmind.god is God_Insect god)
                {
                    god.eat(50);
                }
            }
            if (command == "insectEatBig")
            {
                if (map.overmind.god is God_Insect god)
                {
                    god.eat(500);
                }
            }
            if (command == "addLarvae")
            {
                if (GraphicalMap.selectedHex.location.settlement is Set_Hive hive)
                {
                    hive.lavae.charge += 25;
                }
            }
            if (command == "insectDoom")
            {
                if (GraphicalMap.selectedUnit is UM_Refugees refu)
                {
                    refu.task = new Task_Doomed(refu);

                }
            }
            if (command == "swarm")
            {
                Location loc = GraphicalMap.selectedHex.location;
                UM_Vespidic_Swarm swarm = new UM_Vespidic_Swarm(loc, map.soc_dark, 35);
                map.units.Add(swarm);
            }
        }
    }
}
