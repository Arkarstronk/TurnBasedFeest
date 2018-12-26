using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Attributes;

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
            target.Health.SetColor(Color.Violet);
        }

        public string GetName()
        {
            return "Guard";
        }

        public bool IsSupportive()
        {
            return true;
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} used {GetName()}");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (HasCompleted())
            {
                IAttribute newAttribute = new DefendAttribute(source);
                source.HandedOutAttributes.Add(new GivenAttribute(newAttribute.GetExpiration(), newAttribute, target));
                target.Attributes.Add(newAttribute);
                target.Health.SetColor(Color.White);                

            }            
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {         
        }

        public bool HasCompleted()
        {
            return elapsedTime >= eventTime;
        }
    }
}
