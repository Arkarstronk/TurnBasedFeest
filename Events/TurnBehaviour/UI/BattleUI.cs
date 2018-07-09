using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Actions;
using TurnBasedFeest.Events.TurnBehaviour;

namespace TurnBasedFeest.UI
{
    class BattleUI : ITurnBehaviourEvent
    {
        public enum state
        {
            Action,
            Target,
            Finish,
        }
        public state currentState;
        private int actionIndex;
        private int actorIndex;

        public void Initialize()
        {
            actionIndex = 0;
            actorIndex = 0;
            currentState = state.Action;
        }

        public bool Update(Battle battle, Input input)
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
                case state.Finish:
                    IAction chosenAction = battle.currentActor.actions[actionIndex];
                    chosenAction.SetActors(battle.currentActor, battle.actors[actorIndex]);
                    int index = battle.currentActor.battleEvents.IndexOf(this);
                    battle.currentActor.battleEvents.Insert(index + 1, chosenAction);
                    return true;
            }
            CheckIndexBounds(battle);
            return false;
        }
        

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
            if (currentState == state.Action || currentState == state.Target)
            {
                for (int i = 0; i < battle.currentActor.actions.Count; i++)
                {
                    spritebatch.DrawString(font, battle.currentActor.actions[i].GetName(), new Vector2(300, 200) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }

                if (currentState == state.Target)
                {
                    for (int i = 0; i < battle.actors.Count; i++)
                    {
                        spritebatch.DrawString(font, battle.actors[i].name, new Vector2(375, 200) + new Vector2(0, 20 * i), (i == actorIndex ? Color.Yellow : Color.White));
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

        public void CheckIndexBounds(Battle battle)
        {
            if (actorIndex < 0)
            {
                actorIndex = battle.actors.Count - 1;
            }
            if (actorIndex > battle.actors.Count - 1)
            {
                actorIndex = 0;
            }
            if (actionIndex < 0)
            {
                actionIndex = battle.currentActor.actions.Count - 1;
            }
            if (actionIndex > battle.currentActor.actions.Count - 1)
            {
                actionIndex = 0;
            }
        }

    }
}
