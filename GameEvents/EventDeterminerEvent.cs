﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using System.Linq;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.TurnBehaviour;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedFeest.GameEvents
{
    class EventDeterminerEvent : IGameEvent
    {
        IGameEvent nextEvent;
        Game1 game;

        public EventDeterminerEvent(Game1 game)
        {
            this.game = game;
        }

        public void Initialize(List<Actor> actors)
        {
            if(game.hardcodedEvents.Where(x => x.Key <= game.eventCounter).ToList().Count > 0)
            {
                int hardcodedEventKey = game.hardcodedEvents.Where(x => x.Key <= game.eventCounter).OrderByDescending(x => x.Key).Last().Key;
                string hardcodedEventString = game.hardcodedEvents[hardcodedEventKey];
                game.hardcodedEvents.Remove(hardcodedEventKey);
                SetHardcodedEvent(hardcodedEventString);
            }
            else
            {
                setRandomEvent();
            }
        }

        public bool Update(Game1 game, Input input)
        {
            if (input.Pressed(Keys.Enter))
            {
                game.nextEvent = nextEvent;
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, "Press enter to go to the next event!", new Vector2(0.4f * Game1.screenWidth, 0.5f * Game1.screenHeight), Color.White);
        }

        private void setRandomEvent()
        {
            int random = Game1.rnd.Next(100);

            if(random < 100)
            {
                int randomHealth = Game1.rnd.Next(50, 150);
                game.actors.Add(new Actor("toBeRandomlyGeneratedName", randomHealth, new List<IAction> { new AttackAction(), new DefendAction() }, TextureFactory.Instance.GetTexture("actor"), new EfficientRandomAI(), false));
                nextEvent = new BattleEvent();
            }
            
        }

        private void SetHardcodedEvent(string hardcodedEvent)
        {
            switch (hardcodedEvent)
            {
                case "bossbattle1":
                    break;
            }
        } 

       
    }
}