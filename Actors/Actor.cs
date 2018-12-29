using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.Actors
{
    class Actor
    {
        public string name;
        public Vector2 position;
        public Health health;
        public List<GivenAttribute> giftedAttributes = new List<GivenAttribute>();
        public List<IAttribute> attributes = new List<IAttribute>();
        //public List<IAction> actions;
        public List<ITurnEvent> battleEvents;
        public bool hasTurn;
        public bool isPlayer;
        public Color color;
        private Stats stats;
        private Texture2D texture;

        public Actor(string name, Color color, Stats stats, Texture2D texture, ITurnEvent behaviourEvent, bool isPlayer)
        {
            this.name = name;
            this.color = color;
            this.stats = stats;
            this.health = new Health(stats[StatAttribute.HP] * 10);
            this.texture = texture;
            battleEvents = new List<ITurnEvent> { new AttributeEvent(), behaviourEvent };
            this.isPlayer = isPlayer;
        }

        public void Initialize()
        {
            hasTurn = true;
        }

        public void Update()
        {
            health.Update();   
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode offset size            
            spritebatch.DrawString(font, name, position + new Vector2(0,-90), Color.White);            
            health.Draw(spritebatch, position, font);

            for (int i = 0; i < attributes.Count; i++)
            {
                attributes[i].Draw(spritebatch, font, position + new Vector2(i * 12, 0));
            }

            if (isPlayer)
            {
                spritebatch.Draw(texture, position, color);
            }
            else
            {
                spritebatch.Draw(texture, position, null, color, 0, new Vector2(), new Vector2(1,1), SpriteEffects.FlipHorizontally, 0);
                Console.WriteLine(color.ToString());
            }               
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
