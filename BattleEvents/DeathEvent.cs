using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.BattleEvents
{
    class DeathEvent : ITurnEvent
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor deceased;

        public DeathEvent(Actor deceased)
        {
            this.deceased = deceased;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            deceased.color = Color.Red;
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= eventTime)
            {
                // potentially an extreme small amount of chance to cause an innocent bug if actors do not have to be unique
                battle.aliveActors.Remove(deceased);

                battle.currentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;

                deceased.color = Color.White;

                // if the current player dies during his turn
                if (battle.currentActor == deceased)
                {
                    battle.currentActor = battle.getNextActor();
                    // There is no event after the current actors death, therefore we dont want to signal that we should go to the next event
                    return false;
                }

                return true;
            }

            return false;
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }

    }
}
