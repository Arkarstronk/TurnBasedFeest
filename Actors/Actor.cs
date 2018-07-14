using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.BattleEvents;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.Actors
{
    class Actor
    {
        public string name;
        public Vector2 position;
        public Health health;
        public List<IAction> actions;
        public List<ITurnEvent> battleEvents;
        public bool hasTurn;
        public bool isPlayer;
        public Color color = Color.White;

        private Texture2D texture;

        public Actor(string name, int maxHealth, List<IAction> actions, Texture2D texture, ITurnEvent behaviourEvent, bool isPlayer)
        {
            this.name = name;
            this.health = new Health(maxHealth);
            this.actions = actions;
            this.texture = texture;
            battleEvents = new List<ITurnEvent> { behaviourEvent, behaviourEvent };
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
            spritebatch.DrawString(font, name, position + new Vector2(0,-90), color);            
            health.Draw(spritebatch, position, font);

            if (isPlayer)
            {
                spritebatch.Draw(texture, position, Color.White);
            }
            else
            {
                spritebatch.Draw(texture, position, null, Color.White, 0, new Vector2(), new Vector2(1,1), SpriteEffects.FlipHorizontally, 0);
            }   
        }
    }
}
