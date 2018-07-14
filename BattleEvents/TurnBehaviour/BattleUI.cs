using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.BattleEvents.TurnBehaviour
{
    class BattleUI : ITurnEvent
    {
        public enum state
        {
            ACTION,
            TARGET,
            FINISH,
        }
        public state currentState;
        private int actionIndex;
        private int actorIndex;

        public void Initialize()
        {
            actionIndex = 0;
            actorIndex = 0;
            currentState = state.ACTION;
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            switch (currentState)
            {
                case state.ACTION:
                    actionIndex += Navigation(input);

                    if (input.Pressed(Keys.Enter))
                    {
                        currentState = state.TARGET;
                    }
                    battle.battle.battleText = $"What will {battle.currentActor.name} do?";
                    break;
                case state.TARGET:
                    actorIndex += Navigation(input);

                    if (input.Pressed(Keys.Enter))
                    {
                        currentState = state.FINISH;
                    }
                    if (input.Pressed(Keys.Back))
                    {
                        currentState = state.ACTION;
                    }
                    battle.battle.battleText = $"Select a target.";
                    break;
                case state.FINISH:
                    IAction chosenAction = battle.currentActor.actions[actionIndex];
                    chosenAction.SetActors(battle.currentActor, battle.aliveActors[actorIndex]);
                    battle.currentActor.battleEvents.Insert(battle.eventIndex + 1, chosenAction);
                    return true;
            }
            CheckIndexBounds(battle);
            return false;
        }
        

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
            if (currentState == state.ACTION || currentState == state.TARGET)
            {
                for (int i = 0; i < battle.currentActor.actions.Count; i++)
                {
                    spritebatch.DrawString(font, battle.currentActor.actions[i].GetName(), new Vector2(Game1.screenWidth * 0.5f - 75, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }

                if (currentState == state.TARGET)
                {
                    for (int i = 0; i < battle.aliveActors.Count; i++)
                    {
                        spritebatch.DrawString(font, battle.aliveActors[i].name, new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actorIndex ? Color.Yellow : Color.White));
                    }
                }
            }
        }

        public int Navigation(Input input)
        {
            if (input.Pressed(Keys.Down))
            {
                return 1;
            }
            if (input.Pressed(Keys.Up))
            {
                return -1;
            }
            return 0;
        }

        public void CheckIndexBounds(BattleTurnEvent battle)
        {
            if (actorIndex < 0)
            {
                actorIndex = battle.aliveActors.Count - 1;
            }
            if (actorIndex > battle.aliveActors.Count - 1)
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
