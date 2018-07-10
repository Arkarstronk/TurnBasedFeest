using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.GameEvents;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class DefendAction : IAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            this.source.health.color = Color.Violet;
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int) Game1.time.ElapsedGameTime.TotalMilliseconds;
            
            if(elapsedTime > eventTime)
            {
                source.health.color = Color.White;
                battle.currentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;
                return true;
            }
            else
            {
                return false;
            }            
        }

        public string GetName()
        {
            return "Defend";
        }

        public bool isPositive()
        {
            return true;
        }

        public void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
