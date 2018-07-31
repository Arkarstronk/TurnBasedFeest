using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Gorilla
{
    class GorillaAI : BattleEvent
    {
        private enum GorillaState { FIGHTING, CHARGING };

        private BattleContainer battle;
        private GorillaState state;        
        private ITurnEvent action;
        private Actor actor;

        private int chargingCount;
        private int maxCharge = 4;
        private bool initialized = false;
        private bool powerUp = false;
        private int turnCounter = 0;

        public void Initialize(BattleContainer battle)
        {
            if (!initialized)
            {
                this.actor = battle.CurrentActor;
                this.battle = battle;
                this.state = GorillaState.FIGHTING;

                initialized = true;
            }

            action = new TimerAction(10000);

            if (state == GorillaState.CHARGING)
            {
                this.chargingCount--;
                battle.PushSplashText($"{(int)(((maxCharge - chargingCount)/(double)maxCharge) * 100)}% charged");

                if (chargingCount <= 0)
                {
                    state = GorillaState.FIGHTING;
                    var action = new GorillaMeteorAction();
                    action.SetActors(actor, battle.GetAliveActors().FindAll(x => x.IsPlayer).ToArray());
                    action.Initialize();

                    this.action = action;
                } else
                {
                    action = new TimerAction(1000);
                    action.Initialize();
                }                
            }
            else
            {
                int chance = Game1.rnd.Next(100);
                double healthPercentage = (double)actor.Health.CurrentHealth / (double)actor.Health.MaxHealth;

                if (healthPercentage >= 0.5)
                {
                    if (chance <= 50)
                    {
                        // 50% Go charge
                        state = GorillaState.CHARGING;
                        chargingCount = 4;
                        maxCharge = 4;
                        this.action = new TimerAction(1200);
                        this.action.Initialize();
                        this.battle.PushSplashText($"{actor.Name} is starting to charge...");
                    }
                    else if (chance <= 70)
                    {

                        // 20% chance heal
                        var action = new HealAction();
                        action.SetActors(actor, actor);
                        action.Initialize();
                        this.action = action;
                        this.battle.PushSplashText($"{actor.Name} is healing");
                    }
                    else
                    {
                        // 30% chance attack everyone
                        var action = new GorillaSweepAction();
                        action.SetActors(actor, battle.GetAliveActors().FindAll(x => x.IsPlayer).ToArray());
                        action.Initialize();
                        this.action = action;
                        this.battle.PushSplashText($"{actor.Name} takes a sweep");
                    }
                }
                else
                {
                    if (!powerUp)
                    {
                        powerUp = true;
                        actor.Attributes.Add(new DefendAttribute(actor));
                        actor.Attributes.Add(new DefendAttribute(actor));
                        actor.Attributes.Add(new AttackAttribute(actor));
                        actor.Attributes.Add(new AttackAttribute(actor));
                        actor.Attributes.Add(new AttackMagicAttribute(actor));
                        actor.Attributes.Add(new AttackMagicAttribute(actor));

                        battle.PushSplashText($"{actor.Name} is getting serious!");
                        this.action = new TimerAction(1200);
                        this.action.Initialize();
                    }
                    else
                    {
                        turnCounter++;
                        if (turnCounter % 3 == 0) {
                            // 50% Go charge
                            state = GorillaState.CHARGING;
                            chargingCount = 3;
                            maxCharge = 3;
                            this.action = new TimerAction(1200);
                            this.action.Initialize();
                            this.battle.PushSplashText($"{actor.Name} is starting to charge...");
                        } else
                        {                            
                            var action = new GorillaSweepAction();
                            action.SetActors(actor, battle.GetAliveActors().FindAll(x => x.IsPlayer).ToArray());
                            action.Initialize();
                            this.action = action;
                            this.battle.PushSplashText($"{actor.Name} takes a sweep");
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            action.Draw(battle, batch, font);
        }

        public void Update(GameTime gameTime, Input input)
        {
            action.Update(battle, gameTime, input);
        }

        public bool HasCompleted()
        {
            if (action.HasCompleted() && action is IContinueableAction && (action as IContinueableAction).HasNextEvent())
            {
                action = (action as IContinueableAction).NextEvent();
            }

            return action.HasCompleted();
        }

        
    }
}
