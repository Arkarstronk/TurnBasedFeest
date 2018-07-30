using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actors;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest
{
    // This class contains all information regarding a battle,
    class BattleContainer
    {        
        private List<Actor> actors;
        private Queue<BattleEvent> battleEvents = new Queue<BattleEvent>();
        private BattleEvent CurrentEvent => battleEvents.Peek();

        public static BattleContainer CreateBattle(List<Actor> actors)
        {
            BattleContainer battle = new BattleContainer(actors);

            return battle;
        }

        private BattleContainer(List<Actor> actors)
        {
            this.actors = actors;
        }

        public void Update(GameTime gameTime, Input input)
        {

        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            //draw current event
            if (battleEvents.Peek() != null)
            {
                battleEvents.Peek().Draw(batch, font);
            }

            //debug            
            int i = 0;
            foreach(var battleEvent in battleEvents) {
                batch.DrawString(font, battleEvent.ToString(), new Vector2(0, 15 * i), battleEvent == CurrentEvent ? Color.Red : Color.White);
            }
        }


        public enum Victors
        {
            ENEMY, HEROES, NONE
        }

        public Victors GetVictors()
        {
            if (actors.FindAll(x => x.isPlayer).TrueForAll(x => !x.IsAlive()))
            {
                return Victors.ENEMY;
            }
            if (actors.FindAll(x => !x.isPlayer).TrueForAll(x => !x.IsAlive()))
            {
                return Victors.HEROES;
            }
            return Victors.NONE;
        }
    }
}
