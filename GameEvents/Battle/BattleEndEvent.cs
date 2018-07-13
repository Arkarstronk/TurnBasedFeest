﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.TurnBehaviour;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleEndEvent : IGameEvent
    {
        BattleEvent battle;
        int eventTime = 2000;
        int elapsedTime = 0;

        public BattleEndEvent(BattleEvent battle)
        {
            this.battle = battle;
        }

        public void Initialize(List<Actor> actors)
        {
        }

        public bool Update(Game1 game, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= eventTime)
            {
                // if there are no more alive players
                if (battle.aliveActors.Count == 0)
                {
                    // TODO: go to a game-over event, which on its turn can go to a load-save event or a quit event
                    game.Exit();
                }
                else
                {
                    // TODO: go to a loot event, or a level event or something like that
                    if (battle.aliveActors.TrueForAll(x => x.isPlayer))
                    {
                        game.actors = battle.aliveActors;
                        game.actors.Add(new Actor(
                            "Smart",
                            100,
                            new Vector2(600, 200), new List<IAction> { new AttackAction(), new HealAction(), new DefendAction() },
                            TextureFactory.Instance.GetTexture("actor"),
                            new EfficientRandomAI(),
                            false));
                        game.nextEvent = new BattleEvent();
                    }

                    // TODO: go to a game-over event, which on its turn can go to a load-save event or a quit event
                    if (battle.aliveActors.TrueForAll(x => !x.isPlayer))
                    {
                        game.Exit();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
