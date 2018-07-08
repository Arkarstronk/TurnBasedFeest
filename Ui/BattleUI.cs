using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Actions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors.Behaviours;

namespace TurnBasedFeest.Ui
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
        private List<IAction> actions;
        private List<Actor> actors;
        private int actionIndex;
        private int actorIndex;

        public BattleUI()
        {
            currentState = state.Start;            
        }

        public void initialize(List<IAction> actions, List<Actor> actors)
        {
            this.actors = actors;
            this.actions = actions;
            actionIndex = 0;
            actorIndex = 0;
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
            return new PlayerTurnResult(actions[actionIndex], actors[actorIndex]);
        }

        public void Draw(SpriteFont font, SpriteBatch spritebatch)
        {
            if (currentState == state.Action || currentState == state.Target)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    spritebatch.DrawString(font, actions[i].GetName(), new Vector2(200, 200) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }

                if (currentState == state.Target)
                {
                    for (int i = 0; i < actors.Count; i++)
                    {
                        spritebatch.DrawString(font, actors[i].name, new Vector2(275, 200) + new Vector2(0, 20 * i), (i == actorIndex ? Color.Yellow : Color.White));
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
                actionIndex = actions.Count - 1;
            }
            if (actionIndex > actions.Count - 1)
            {
                actionIndex = 0;
            }
        }

    }
}
