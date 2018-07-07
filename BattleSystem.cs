using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest
{
    class BattleSystem
    {
        public bool ongoingBattle;
        List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }

        public void Update(Input input)
        {
            if (actorEnum.Current.moveRemaining)
            {
                // TODO: do not hardcode target
                Actor target = actors.Find(x => x.name != actorEnum.Current.name);

                if (input.Released(Keys.Enter))
                {
                    target.health.actorCurrentHealth -= 10;
                    actorEnum.Current.moveRemaining = false;

                }
                if (input.Released(Keys.RightShift))
                {
                    actorEnum.Current.health.actorCurrentHealth += 10;
                    actorEnum.Current.moveRemaining = false;
                }
            }
            else if (!actorEnum.MoveNext())
            {
                foreach (Actor e in actors)
                {
                    e.moveRemaining = true;
                }

                actorEnum = actors.GetEnumerator();
                actorEnum.MoveNext();
            }

            foreach(Actor actor in actors)
            {
                actor.Update();

                if(actor.health.actorCurrentHealth <= 0)
                {
                    EndFight();
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(spritebatch, font);
            }
        }
    }
}
