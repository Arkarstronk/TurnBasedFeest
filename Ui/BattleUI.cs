using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Actions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors.Behaviours;

namespace TurnBasedFeest.UI
{
    class BattleUI
    {
        public enum state
        {
            Start,
            Action,
            Target,
            Finish,
        }
        public state currentState;
        private List<Actor> actors;
        private int actionIndex;
        private int actorIndex;
        private Actor currentActor;

        public BattleUI()
        {
            currentState = state.Start;            
        }

        public void initialize()
        {
            actionIndex = 0;
            actorIndex = 0;
        }

        public void startTurn(Actor currentActor, List<Actor> actors)
        {
            this.currentActor = currentActor;
            this.actors = actors;
            currentState = state.Action;
        }

        public void Update(Input input)
        {
            switch (currentState)
            {
                case state.Action:
                    actionIndex += Navigation(input);

                    if (input.Released(Keys.Enter))
                    {
                        currentState = state.Target;
                    }
                    break;
                case state.Target:
                    actorIndex += Navigation(input);

                    if (input.Released(Keys.Enter))
                    {
                        currentState = state.Finish;
                    }
                    if (input.Released(Keys.Back))
                    {
                        currentState = state.Action;
                    }
                    break;
            }
            CheckIndexBounds();
        }

        public ITurnResult GetTurnResult()
        {
            currentState = state.Start;
            return new PlayerTurnResult(currentActor.actions[actionIndex], actors[actorIndex], currentActor);
        }

        public void Draw(SpriteFont font, SpriteBatch spritebatch)
        {
            if (currentState == state.Action || currentState == state.Target)
            {
                for (int i = 0; i < currentActor.actions.Count; i++)
                {
                    spritebatch.DrawString(font, currentActor.actions[i].GetName(), new Vector2(300, 200) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }

                if (currentState == state.Target)
                {
                    for (int i = 0; i < actors.Count; i++)
                    {
                        spritebatch.DrawString(font, actors[i].name, new Vector2(375, 200) + new Vector2(0, 20 * i), (i == actorIndex ? Color.Yellow : Color.White));
                    }
                }
            }
        }

        public int Navigation(Input input)
        {
            if (input.Released(Keys.Down))
            {
                return 1;
            }
            if (input.Released(Keys.Up))
            {
                return -1;
            }
            return 0;
        }

        public void CheckIndexBounds()
        {
            if (actorIndex < 0)
            {
                actorIndex = actors.Count - 1;
            }
            if (actorIndex > actors.Count - 1)
            {
                actorIndex = 0;
            }
            if (actionIndex < 0)
            {
                actionIndex = currentActor.actions.Count - 1;
            }
            if (actionIndex > currentActor.actions.Count - 1)
            {
                actionIndex = 0;
            }
        }

    }
}
