using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;
using System;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class HealAction : IAction
    {
        
        Actor source;
        Actor target;        

        public void SetActors(Actor source, params Actor[] target)
        {
            this.source = source;
            this.target = target[0];
        }

        public void Initialize(BattleContainer battle)
        {            
            int heal = source.GetStats()[StatisticAttribute.SUPPORT_MAGIC];
            int beginHP = (int)target.Health.CurrentHealth;

            heal = Math.Min(target.Health.MaxHealth, beginHP + heal) - beginHP;
            
            
            battle.ParticleHelper.Add(new TextParticle($"+{beginHP + heal}", 1200, target.Position, new Vector2(30.0f, -30.0f)));
            battle.Animations.Add(new AnimationHealthChange(target, beginHP, -heal));
        }

        public string GetName() => "Heal";
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.FRIENDLY);



        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} used {GetName()}");
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {         
        }

        public bool HasCompleted() => true;
    }
}
