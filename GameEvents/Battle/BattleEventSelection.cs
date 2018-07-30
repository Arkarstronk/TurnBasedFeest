using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.GameEvents.UI;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleEventSelection : BattleEvent
    {
        private BattleContainer battle;
        private MultipleChoiceEvent multipleChoiceEvent;
        private bool hasCompleted;

        public void Initialize(BattleContainer battle)
        {
            this.hasCompleted = false;
            this.battle = battle;
            this.multipleChoiceEvent = new MultipleChoiceEvent(new List<string>() {
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
                    battle.Enqueue(new BattleEventActionSelection());
                    hasCompleted = true;
                } else
                {
                    battle.Enqueue(new BattleEventActionSelection());
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
        private MultipleChoiceEvent multipleChoiceEvent;

        private bool hasCompleted = false;
        private List<IAction> possibleActions;

        public void Initialize(BattleContainer battle)
        {
            var currentActor = battle.CurrentActor;

            this.battle = battle;            
            this.possibleActions = currentActor.GetStats().Actions.FindAll(x => !x.IsSupportive());
            
            this.multipleChoiceEvent = new MultipleChoiceEvent(possibleActions.Select(x => x.GetName()).ToList());
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
                    battle.Enqueue(new BattleEventTarget(possibleActions[result], BattleEventTarget.Targets.ENEMY_SINGLE));
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
        public enum Targets { ENEMY_SINGLE, ENEMY_ALL, FRIENDLY_SINGLE, FRIENDLY_ALL }

        private IAction action;
        private Targets targets;
        private List<Actor> possibleTargets;
        private BattleContainer battle;
        private MultipleChoiceEvent multipleChoiceEvent;
        private bool hasCompleted = false;

        public BattleEventTarget(IAction action, Targets targets)
        {
            this.action = action;
            this.targets = targets;
        }

        public void Initialize(BattleContainer battle)
        {
            this.battle = battle;
            this.possibleTargets = getTargets();
            this.multipleChoiceEvent = new MultipleChoiceEvent(possibleTargets.Select(x => x.Name).ToList());
        }

        private List<Actor> getTargets()
        {
            List<Actor> alive = battle.GetAliveActors();

            switch(targets)
            {
                case Targets.ENEMY_SINGLE:
                    return alive.FindAll(x => !x.isPlayer);
                case Targets.ENEMY_ALL:
                    return alive.FindAll(x => !x.isPlayer);
                case Targets.FRIENDLY_ALL:
                    return alive.FindAll(x => x.isPlayer);
                case Targets.FRIENDLY_SINGLE:
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
                battle.Enqueue(new BattleEventSelection());
                hasCompleted = true;
            }
            else
            {
                int result = multipleChoiceEvent.Update(gameTime, input);
                if (result >= 0)
                {                    
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
        private IAction action;
        private Actor target;
        private BattleContainer battle;

        public BattleEventFight(IAction action, Actor target)
        {
            this.action = action;
            this.target = target;
        }

        public void Initialize(BattleContainer battle)
        {
            Console.WriteLine($"Attacking: {target.Name}");
            this.battle = battle;
            this.action.SetActors(battle.CurrentActor, target);
            this.action.Initialize();            
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            this.action.Draw(battle, batch, font);
        }

        public void Update(GameTime gameTime, Input input)
        {
            this.action.Update(battle, gameTime, input);
            
            // No more events for the player after this, so don't enqueue anything.
        }

        public bool HasCompleted()
        {
            return action.HasCompleted();
        }
    }
}
