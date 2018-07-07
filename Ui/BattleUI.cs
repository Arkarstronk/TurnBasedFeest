using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Actions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        private List<IAction> choices;
        private List<Actor> targets;
        private int choiceIndex;
        private int targetIndex;

        public BattleUI()
        {
            currentState = state.Start;            
        }

        public void initialize(List<IAction> actions, List<Actor> actors)
        {
            targets = actors;
            this.choices = actions;
            choiceIndex = 0;
            targetIndex = 0;
            currentState = state.Action;
        }

        public void Update(Input input)
        {
            switch (currentState)
            {
                case state.Action:
                    choiceIndex += Navigation(input);

                    if (input.Released(Keys.Enter))
                    {
                        currentState = state.Target;
                    }

                    break;
                case state.Target:
                    targetIndex += Navigation(input);

                    if (input.Released(Keys.Enter))
                    {
                        currentState = state.Finish;
                    }
                    break;
            }

            CheckIndexBounds();
        }

        public IAction GetChosenAction()
        {
            currentState = state.Start;
            return choices[choiceIndex];
        }

        public Actor GetChosenActor()
        {
            return targets[targetIndex];
        }

        public void Draw(SpriteFont font, SpriteBatch spritebatch)
        {
            for (int i = 0; i < choices.Count; i++)
            {
                spritebatch.DrawString(font, choices[i].ToString(), new Vector2(200, 200) + new Vector2(0, 20 * i), (i == choiceIndex ? Color.Yellow : Color.White));
            }

            if(currentState == state.Target)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    spritebatch.DrawString(font, targets[i].name, new Vector2(250, 200) + new Vector2(0, 20 * i), (i == targetIndex ? Color.Yellow : Color.White));
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
            if (targetIndex < 0)
            {
                targetIndex = targets.Count - 1;
            }
            if (targetIndex > targets.Count - 1)
            {
                targetIndex = 0;
            }
            if (choiceIndex < 0)
            {
                choiceIndex = choices.Count - 1;
            }
            if (choiceIndex > choices.Count - 1)
            {
                choiceIndex = 0;
            }
        }

    }
}
