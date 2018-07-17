using System;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Attributes;

namespace TurnBasedFeest.BattleEvents
{
    class AttributeEvent : ITurnEvent
    {
        public void Initialize()
        {            
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            for(int i = 0; i < battle.currentActor.attributes.Count; i ++)
            {
                if (battle.currentActor.attributes[i].GetExpiration() == 0)
                {
                    battle.currentActor.attributes.RemoveAt(i);
                }
            }
            return true;
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
