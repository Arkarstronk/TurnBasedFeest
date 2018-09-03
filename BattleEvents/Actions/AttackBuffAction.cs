using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents.Battle;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AttackBuffAction : IAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;

        public void SetActors(Actor source, params Actor[] target)
        {
            this.source = source;
            this.target = target[0];
        }

        public void Initialize(BattleContainer battle)
        {
            elapsedTime = 0;            
            target.Health.SetColor(Color.Violet);
        }

        public string GetName() => "Buff";
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.FRIENDLY);
        public bool HasCompleted() => elapsedTime >= eventTime;

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} used {GetName()}");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (HasCompleted())
            {
                IAttribute newAttribute = new AttackAttribute(source);
                source.HandedOutAttributes.Add(new GivenAttribute(newAttribute.GetExpiration(), newAttribute, target));
                target.Attributes.Add(newAttribute);
                target.Health.SetColor(Color.White);                

            }            
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {         
        }

        
    }
}
