using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.UI;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Battle
{
    //public enum Targets { ENEMY, FRIENDLY, ALL_ENEMY, ALL_FRIENDLY }

    class BattleEventSelection : BattleEvent
    {
        private BattleContainer battle;
        private MultipleChoiceUI multipleChoiceEvent;
        private bool hasCompleted;

        public void Initialize(BattleContainer battle)
        {
            this.hasCompleted = false;
            this.battle = battle;
            this.multipleChoiceEvent = new MultipleChoiceUI(new List<string>() {
                "Fight",
                "Support"
            });
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {            
            multipleChoiceEvent.Draw(batch, font, new Vector2(40, 400));
        }

        public void Update(GameTime gameTime, Input input)
        {
            battle.PushSplashText($"What will {battle.CurrentActor.Name} do?");
            int result = multipleChoiceEvent.Update(gameTime, input);

            if (result >= 0)
            {
                if (result == 0)
                {
                    battle.Enqueue(new BattleEventActionSelection(battle.CurrentActor.GetStats().Actions.FindAll(x => x.GetTarget().Side == ActionTarget.TargetSide.ENEMY)));
                    hasCompleted = true;
                } else
                {
                    battle.Enqueue(new BattleEventActionSelection(battle.CurrentActor.GetStats().Actions.FindAll(x => x.GetTarget().Side == ActionTarget.TargetSide.FRIENDLY)));
                    hasCompleted = true;
                }
            }
        }

        public bool HasCompleted()
        {            
            return hasCompleted;
        }
    }

    class BattleEventActionSelection : BattleEvent
    {
        private BattleContainer battle;
        private MultipleChoiceUI multipleChoiceEvent;

        private bool hasCompleted = false;
        private List<IAction> possibleActions;
        

        public BattleEventActionSelection(List<IAction> possibleActions)
        {
            this.possibleActions = possibleActions;            
        }

        public void Initialize(BattleContainer battle)
        {
            var currentActor = battle.CurrentActor;

            this.hasCompleted = false;            
            this.battle = battle;            
            this.multipleChoiceEvent = new MultipleChoiceUI(possibleActions.Select(x => x.GetName()).ToList());            
        }

        public void Update(GameTime gameTime, Input input)
        {
            battle.PushSplashText($"What will {battle.CurrentActor.Name} do next?");           

            if (input.Pressed(Keys.Back))
            {
                battle.Enqueue(new BattleEventSelection());
                hasCompleted = true;
            }
            else
            {
                int result = multipleChoiceEvent.Update(gameTime, input);                
                if (result >= 0)
                {
                    battle.Enqueue(new BattleEventTarget(this, possibleActions[result]));
                    hasCompleted = true;
                }
            }
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            this.multipleChoiceEvent.Draw(batch, font, new Vector2(40, 400));
        }

        public bool HasCompleted()
        {
            return hasCompleted;
        }
    }

    
    class BattleEventTarget : BattleEvent
    {
        private BattleEvent previous;
        private IAction action;        
        private List<Actor> possibleTargets;
        private BattleContainer battle;
        private MultipleChoiceUI multipleChoiceEvent;
        private bool hasCompleted = false;

        public BattleEventTarget(BattleEvent previous, IAction action)
        {
            this.previous = previous;
            this.action = action;            
        }

        public void Initialize(BattleContainer battle)
        {
            this.hasCompleted = false;
            this.battle = battle;
            this.possibleTargets = getTargets();

            if (action.GetTarget().Amount == ActionTarget.TargetAmount.ALL)            
            {
                this.multipleChoiceEvent = new MultipleChoiceUI(new List<String>{ "All" });
            }   
            else
            {
                this.multipleChoiceEvent = new MultipleChoiceUI(possibleTargets.Select(x => x.Name).ToList());
            }
        }

        private List<Actor> getTargets()
        {
            List<Actor> alive = battle.GetAliveActors();
            return action.GetTarget().GetPossibleTargets(battle.CurrentActor, alive);
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            this.multipleChoiceEvent.Draw(batch, font, new Vector2(40, 400));
        }

        public void Update(GameTime gameTime, Input input)
        {
            if (input.Pressed(Keys.Back))
            {
                battle.Enqueue(previous);
                hasCompleted = true;
            }
            else
            {
                int result = multipleChoiceEvent.Update(gameTime, input);
                if (result >= 0)
                {
                    // TODO: Change this to more general approach.
                    var targets = action.GetTarget().GetTargets(battle.CurrentActor, possibleTargets, result);
                    action.SetActors(battle.CurrentActor, targets.ToArray());
                    battle.Enqueue(new BattleEventFight(action));                    
                    hasCompleted = true;
                }
            }
        }

        public bool HasCompleted()
        {
            return hasCompleted;
        }
    }
    class BattleEventFight : BattleEvent
    {
        private ITurnEvent action;
        
        private BattleContainer battle;

        public BattleEventFight(IAction action)
        {
            this.action = action;        
        }

        public void Initialize(BattleContainer battle)
        {            
            this.battle = battle;            
            this.action.Initialize(battle);
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            this.action.Draw(battle, batch, font);

            batch.DrawString(font, action.ToString(), new Vector2(200, 15), Color.Red);
        }

        public void Update(GameTime gameTime, Input input)
        {
            this.action.Update(battle, gameTime, input);
            
            // No more events for the player after this, so don't enqueue anything.
        }

        public bool HasCompleted()
        {
            if (action.HasCompleted() && action is IContinueableAction && (action as IContinueableAction).HasNextEvent())
            {
                action = (action as IContinueableAction).NextEvent() ?? action;
            }
            return action.HasCompleted();
        }
    }
}
