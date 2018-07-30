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
        public Actor CurrentActor;
        
        private List<Actor> actors;
        private IEnumerator<Actor> cyclicActorList;
        private Queue<BattleEvent> battleEvents = new Queue<BattleEvent>();
        private BattleEvent CurrentEvent => battleEvents.Peek();
        private String splashText = "A new fight awaits~!";

        public static BattleContainer CreateBattle(List<Actor> actors)
        {
            BattleContainer battle = new BattleContainer(actors);

            return battle;
        }

        private BattleContainer(List<Actor> actors)
        {
            cyclicActorList = GetCyclicAutorList();
            this.actors = actors;
            this.actors.ForEach(x =>
            {
                x.Initialize();                
            });

            calculateDistances(actors);

            NextActor();
            Console.WriteLine(CurrentActor);
        }

        public void Enqueue(BattleEvent nextEvent)
        {
            nextEvent.Initialize(this);
            battleEvents.Enqueue(nextEvent);
            
        }

        private void calculateDistances(List<Actor> actors)
        {
            int parseDistanceAlly = (int)((0.7f * Game1.screenHeight) / (actors.FindAll(x => x.isPlayer).Count + 1));
            for (int i = 0; i < actors.FindAll(x => x.isPlayer).Count; i++)
            {
                actors.FindAll(x => x.isPlayer)[i].Position = new Vector2(Game1.screenWidth - 50 - 0.8f * Game1.screenWidth, 0.1f * Game1.screenHeight + (i + 1) * parseDistanceAlly);
            }

            int parseDistanceEnemies = (int)((0.7f * Game1.screenHeight) / (actors.FindAll(x => !x.isPlayer).Count + 1));
            for (int i = 0; i < actors.FindAll(x => !x.isPlayer).Count; i++)
            {
                actors.FindAll(x => !x.isPlayer)[i].Position = new Vector2(Game1.screenWidth - 50 - 0.2f * Game1.screenWidth, 0.1f * Game1.screenHeight + (i + 1) * parseDistanceEnemies);
            }
        }

        public List<Actor> GetAliveActors()
        {
            return actors.FindAll(x => x.IsAlive());
        }

        public void Update(GameTime gameTime, Input input)
        {
            // Handle the battle events
            if (battleEvents.Count > 0)
            {
                BattleEvent currentEvent = battleEvents.Peek();
                currentEvent.Update(gameTime, input);

                // Take from the queue if the event is completed.
                if (currentEvent.HasCompleted())
                {
                    Console.WriteLine("yo?");
                    battleEvents.Dequeue();
                }
            } else
            {
                // No more battle events are left,
                // Go to the next actor
                NextActor();
                var nextEvent = CurrentActor.StartEvent;
                nextEvent.Initialize(this);
                battleEvents.Enqueue(nextEvent);
                //throw new NotImplementedException("Battle events need to be populized");
            }

            // Update all the actors
            actors.ForEach(x => x.Update(gameTime));

        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            // Draw the splash text
            batch.DrawString(font, splashText, new Vector2(0.2f * Game1.screenWidth, 0.75f * Game1.screenHeight), Color.White);

            // Draw the current actors that are present:
            foreach (Actor actor in actors) {
                actor.Draw(batch, font);
            }

            //draw current event
            if (battleEvents.Count > 0 && battleEvents.Peek() != null)
            {
                battleEvents.Peek().Draw(batch, font);
            }

            //debug            
            int i = 0;
            foreach(var battleEvent in battleEvents) {
                batch.DrawString(font, battleEvent.ToString(), new Vector2(0, 15 * i), battleEvent == CurrentEvent ? Color.Red : Color.White);
                i++;
            }
        }

        public void PushSplashText(string text)
        {
            this.splashText = text;
        }

        public void EndBattle()
        {
            foreach (Actor actor in actors)
            {
                actor.attributes.RemoveAll(x => true);
                actor.giftedAttributes.RemoveAll(x => true);
            }
        }

        private Actor NextActor()
        {
            this.CurrentActor = cyclicActorList.Current;
            cyclicActorList.MoveNext();
            return this.CurrentActor;
        }

        private IEnumerator<Actor> GetCyclicAutorList()
        {
            while(true)
            {
                if (actors.Count(x => x.IsAlive()) == 0)
                {
                    yield return null;
                }

                foreach(Actor actor in this.actors)
                {
                    yield return actor;
                }
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
