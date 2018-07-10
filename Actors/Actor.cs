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
        public List<IBattleEvent> battleEvents;
        public bool hasTurn;
        public bool isPlayer;
        public Color color = Color.White;

        public Actor(string name, Vector2 position, int maxHealth, List<IAction> actions, Texture2D texture, IBattleEvent behaviourEvent, bool isPlayer)
        {
            this.name = name;
            this.position = position;
            health = new Health(maxHealth, texture);
            this.actions = actions;
            battleEvents = new List<IBattleEvent> { behaviourEvent, behaviourEvent };
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
            spritebatch.DrawString(font, name, position + new Vector2(0,-20), color);
            health.Draw(spritebatch, position);
        }
    }
}
