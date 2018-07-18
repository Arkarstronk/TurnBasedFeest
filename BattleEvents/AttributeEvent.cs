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
            for(int i = 0; i < battle.currentActor.giftedAttributes.Count; i ++)
            {
                battle.currentActor.giftedAttributes[i].expiration--;
                if (battle.currentActor.giftedAttributes[i].expiration == 0)
                {
                    //possible bug if identic attributes
                    battle.currentActor.giftedAttributes[i].receiver.attributes.Remove(battle.currentActor.giftedAttributes[i].attribute);
                }
            }
            return true;
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
