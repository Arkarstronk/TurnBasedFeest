using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.BattleEvents.TurnBehaviour
{
    // todo: refactor this with multiple choice events
    class BattleUI : ITurnEvent
    {
        public enum state
        {
            START,
            FIGHT,
            SUPPORT,
            TARGET,
            FINISH,
        }
        public state currentState;

        private IAction chosenAction;

        private int startIndex;
        private int actionIndex;
        private int targetIndex;

        public void Initialize()
        {
            startIndex = 0;
            actionIndex = 0;
            targetIndex = 0;
            currentState = state.START;
            chosenAction = null;
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            switch (currentState)
            {
                case state.START:
                    Navigation(input, ref startIndex, 2);

                    if (input.Pressed(Keys.Enter))
                    {
                        switch (startIndex)
                        {
                            case 0:
                                currentState = state.FIGHT;
                                break;
                            case 1:
                                currentState = state.SUPPORT;
                                break;
                        }                        
                    }
                    battle.battle.battleText = $"What will {battle.currentActor.name} do?";
                    break;
                case state.FIGHT:
                    Navigation(input, ref actionIndex, battle.currentActor.actions.FindAll(x => !x.IsSupportive()).Count);

                    if (input.Pressed(Keys.Enter))
                    {
                        chosenAction = battle.currentActor.actions.FindAll(x => !x.IsSupportive())[actionIndex];
                        currentState = state.TARGET;
                    }
                    if (input.Pressed(Keys.Back))
                    {
                        currentState = state.START;
                        chosenAction = null;
                        actionIndex = 0;
                    }
                    battle.battle.battleText = $"How will {battle.currentActor.name} fight?";
                    break;
                case state.SUPPORT:
                    Navigation(input, ref actionIndex, battle.currentActor.actions.FindAll(x => x.IsSupportive()).Count);

                    if (input.Pressed(Keys.Enter))
                    {
                        chosenAction = battle.currentActor.actions.FindAll(x => x.IsSupportive())[actionIndex];
                        currentState = state.TARGET;
                    }
                    if (input.Pressed(Keys.Back))
                    {
                        currentState = state.START;
                        chosenAction = null;
                        actionIndex = 0;
                    }
                    battle.battle.battleText = $"How will {battle.currentActor.name} support?";
                    break;
                case state.TARGET:
                    Navigation(input, ref targetIndex, chosenAction.IsSupportive() ? battle.aliveActors.FindAll(x => x.isPlayer).Count : battle.aliveActors.FindAll(x => !x.isPlayer).Count);
                    
                    if (input.Pressed(Keys.Enter))
                    {
                        currentState = state.FINISH;
                    }
                    if (input.Pressed(Keys.Back))
                    {
                        currentState = chosenAction.IsSupportive() ? state.SUPPORT : state.FIGHT;
                        targetIndex = 0;
                    }

                    battle.battle.battleText = $"Select a target.";
                    break;
                case state.FINISH:
                    chosenAction.SetActors(battle.currentActor, chosenAction.IsSupportive() ? battle.aliveActors.FindAll(x => x.isPlayer)[targetIndex] : battle.aliveActors.FindAll(x => !x.isPlayer)[targetIndex]);
                    battle.currentActor.battleEvents.Insert(battle.eventIndex + 1, chosenAction);
                    return true;
            }
            return false;
        }
        

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, "Fight", new Vector2(Game1.screenWidth * 0.5f - 100, Game1.screenHeight * 0.6f), (0 == startIndex ? Color.Yellow : Color.White));
            spritebatch.DrawString(font, "Support", new Vector2(Game1.screenWidth * 0.5f - 100, Game1.screenHeight * 0.6f + 20), (1 == startIndex ? Color.Yellow : Color.White));

            if (currentState == state.FIGHT)
            {
                for (int i = 0; i < battle.currentActor.actions.FindAll(x => !x.IsSupportive()).Count; i++)
                {
                    spritebatch.DrawString(font, battle.currentActor.actions.FindAll(x => !x.IsSupportive())[i].GetName(), new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }
            }

            if (currentState == state.SUPPORT)
            {
                for (int i = 0; i < battle.currentActor.actions.FindAll(x => x.IsSupportive()).Count; i++)
                {
                    spritebatch.DrawString(font, battle.currentActor.actions.FindAll(x => x.IsSupportive())[i].GetName(), new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                }
            }

            if (currentState == state.TARGET)
            {
                if (chosenAction.IsSupportive())
                {
                    for (int i = 0; i < battle.currentActor.actions.FindAll(x => x.IsSupportive()).Count; i++)
                    {
                        spritebatch.DrawString(font, battle.currentActor.actions.FindAll(x => x.IsSupportive())[i].GetName(), new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                    }

                    for (int i = 0; i < battle.aliveActors.FindAll(x => x.isPlayer).Count; i++)
                    {
                        spritebatch.DrawString(font, battle.aliveActors.FindAll(x => x.isPlayer)[i].name, new Vector2(Game1.screenWidth * 0.5f + 100, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == targetIndex ? Color.Yellow : Color.White));
                    }
                }
                else
                {
                    for (int i = 0; i < battle.currentActor.actions.FindAll(x => !x.IsSupportive()).Count; i++)
                    {
                        spritebatch.DrawString(font, battle.currentActor.actions.FindAll(x => !x.IsSupportive())[i].GetName(), new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == actionIndex ? Color.Yellow : Color.White));
                    }

                    for (int i = 0; i < battle.aliveActors.FindAll(x => !x.isPlayer).Count; i++)
                    {
                        spritebatch.DrawString(font, battle.aliveActors.FindAll(x => !x.isPlayer)[i].name, new Vector2(Game1.screenWidth * 0.5f + 100, Game1.screenHeight * 0.6f) + new Vector2(0, 20 * i), (i == targetIndex ? Color.Yellow : Color.White));
                    }
                }
                
            }
        }

        public void Navigation(Input input, ref int index, int upperBound)
        {
            //move index
            if (input.Pressed(Keys.Down))
            {
                index++;
            }
            if (input.Pressed(Keys.Up))
            {
                index--;
            }

            //check bounds
            if (index < 0)
            {
                index = upperBound - 1;
            }
            if (index >= upperBound)
            {
                index = 0;
            }

        }
    }
}
