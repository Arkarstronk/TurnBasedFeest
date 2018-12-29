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
            for(int i = 0; i < battle.CurrentActor.giftedAttributes.Count; i ++)
            {
                battle.CurrentActor.giftedAttributes[i].expiration--;
                if (battle.CurrentActor.giftedAttributes[i].expiration == 0)
                {
                    //possible bug if identic attributes
                    battle.CurrentActor.giftedAttributes[i].receiver.attributes.Remove(battle.CurrentActor.giftedAttributes[i].attribute);
                }
            }
            return true;
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
