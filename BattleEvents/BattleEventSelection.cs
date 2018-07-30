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
    public enum Targets { ENEMY, FRIENDLY }
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
                    battle.Enqueue(new BattleEventActionSelection(Targets.ENEMY, battle.CurrentActor.GetStats().Actions.FindAll(x => !x.IsSupportive())));
                    hasCompleted = true;
                } else
                {
                    battle.Enqueue(new BattleEventActionSelection(Targets.FRIENDLY, battle.CurrentActor.GetStats().Actions.FindAll(x => x.IsSupportive())));
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
        private Targets target;
        

        public BattleEventActionSelection(Targets target, List<IAction> possibleActions)
        {
            this.target = target;
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
                    battle.Enqueue(new BattleEventTarget(this, possibleActions[result], target));
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
        private Targets targets;
        private List<Actor> possibleTargets;
        private BattleContainer battle;
        private MultipleChoiceUI multipleChoiceEvent;
        private bool hasCompleted = false;

        public BattleEventTarget(BattleEvent previous, IAction action, Targets targets)
        {
            this.previous = previous;
            this.action = action;
            this.targets = targets;
        }

        public void Initialize(BattleContainer battle)
        {
            this.hasCompleted = false;
            this.battle = battle;
            this.possibleTargets = getTargets();
            this.multipleChoiceEvent = new MultipleChoiceUI(possibleTargets.Select(x => x.Name).ToList());
        }

        private List<Actor> getTargets()
        {
            List<Actor> alive = battle.GetAliveActors();

            switch(targets)
            {
                case Targets.ENEMY:
                    return alive.FindAll(x => !x.isPlayer);                
                case Targets.FRIENDLY:
                    return alive.FindAll(x => x.isPlayer);
            }
            return new List<Actor>();            
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
                    action.SetActors(battle.CurrentActor, possibleTargets[result]);
                    battle.Enqueue(new BattleEventFight(action, possibleTargets[result]));
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
        private Actor target;
        private BattleContainer battle;

        public BattleEventFight(IAction action, Actor target)
        {
            this.action = action;
            this.target = target;
        }

        public void Initialize(BattleContainer battle)
        {            
            this.battle = battle;            
            this.action.Initialize();            
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
