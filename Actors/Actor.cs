using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest.Actors
{
    class Actor
    {
        public BattleEvent StartEvent { get; }
        public bool Shake { get; internal set; }

        public string Name;
        public Vector2 Position;
        public Health Health;
        public List<GivenAttribute> HandedOutAttributes = new List<GivenAttribute>();
        public List<IAttribute> attributes = new List<IAttribute>();
        //public List<IAction> actions;
        //public List<ITurnEvent> battleEvents;
        public bool hasTurn;
        public bool isPlayer;
        public Color color;
        private Stats stats;
        private CustomSprite sprite;

        

        public Actor(string name, Color color, Stats stats, CustomSprite sprite, BattleEvent behaviourEvent, bool isPlayer)
        {
            this.Name = name;
            this.color = color;
            this.stats = stats;
            this.Health = new Health(stats.MaxHealth);
            this.sprite = sprite;
            this.StartEvent = behaviourEvent;
            //battleEvents = new List<ITurnEvent> { new AttributeEvent(), behaviourEvent };
            this.isPlayer = isPlayer;
        }

        public void Initialize()
        {
            hasTurn = true;
        }

        // The actor starts its turn here
        public void OnTurnStart()
        {
            // Handle attributes expiration
            HandedOutAttributes.ForEach(x => x.Expiration--);
            HandedOutAttributes.FindAll(x => x.Expiration <= 0).ForEach(x => {
                x.receiver.attributes.Remove(x.attribute);
            });
        }

        public void Update(GameTime gameTime)
        {
            Health.Update();   
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode offset size            
            spritebatch.DrawString(font, Name, Position + new Vector2(0,-90), Color.White);            
            Health.Draw(spritebatch, Position, font);

            for (int i = 0; i < attributes.Count; i++)
            {
                attributes[i].Draw(spritebatch, font, Position + new Vector2(i * 12, 0));
            }

            sprite.Draw(spritebatch, Position.X, Position.Y);            
        }

        public bool IsAlive()
        {
            return Health.CurrentHealth > 0;
        }

        public List<IAction> GetActions()
        {
            return stats.Actions;
        }

        public Stats GetStats()
        {
            return this.stats;
        }
    }
}
