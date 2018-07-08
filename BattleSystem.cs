using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors.Behaviours;
using TurnBasedFeest.Actions;

namespace TurnBasedFeest
{
    class BattleSystem
    {
        public bool ongoingBattle;
        List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;
        Actor currentActor;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
            currentActor = actorEnum.Current;
        }

        public void Update(Input input)
        {
            // If a behaviour is determined
            if(currentActor.turnBehaviour.DetermineBehaviour(input, actors, currentActor))
            {
                ITurnResult turnResult = currentActor.turnBehaviour.GetTurnResult();
                IActionResult actionResult = turnResult.Preform(currentActor);
                currentActor = getNextActor();
            }
            
            foreach (Actor actor in actors)
            {
                actor.Update();

                if(actor.health.actorCurrentHealth <= 0)
                {
                    ongoingBattle = false;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(spritebatch, font);
            }

            spritebatch.DrawString(font, ">", currentActor.position - new Vector2(35, 0), Color.White);
        }

        private Actor getNextActor()
        {
            if (!actorEnum.MoveNext())
            {
                actorEnum = actors.GetEnumerator();
                actorEnum.MoveNext();
            }
            return actorEnum.Current;
        }
    }
}
