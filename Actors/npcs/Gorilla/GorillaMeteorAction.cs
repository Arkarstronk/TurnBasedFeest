using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Gorilla
{
    class GorillaMeteorAction : IAction
    {        
        private Actor source;
        private Actor[] targets;

        private CustomSprite meteorSprite;        

        private const int MAX_METEORS = 2000;

        public string GetName() => "Gorilla Meteor";
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.ENEMY, ActionTarget.TargetAmount.ALL);

        public void Initialize(BattleContainer battle)
        {
            // Initiate meteors
            meteorSprite = CustomSprite.GetSprite("meteor");

            battle.Animations.Add(new AnimationTimer(3000));

            for (int i = 0; i < MAX_METEORS; i++)
            {                
                var direction = new Vector2((float)(-1.1 + 0.2 * Game1.rnd.NextDouble()), (float)(1.1 + 0.2 * Game1.rnd.NextDouble()));
                direction.Normalize();
                direction.X *= 240;
                direction.Y *= 240;

                var particle = new Particle(meteorSprite, 2000 + Game1.rnd.Next(1000), new Vector2(800 + Game1.rnd.Next(-8000, 800), -20 - Game1.rnd.Next(240)), direction);

                battle.ParticleHelper.Add(particle);
            }

            

            int attack = source.GetStats()[StatisticAttribute.ATTACK_MAGIC] * 4;

            int[] targetsHP = targets
                .Select(x =>
                {
                    int defence = x.GetStats()[StatisticAttribute.DEFENCE, x.Attributes];
                    int damage = Math.Max(2, attack - defence);

                    battle.ParticleHelper.Add(new TextParticle($"-{damage}", 1200, x.Position, new Vector2(30.0f, -30.0f)));
                    battle.Animations.Add(new AnimationHealthChange(x, x.Health.CurrentHealth, damage));

                    return Math.Max(0, (int)(x.Health.CurrentHealth - damage));
                }).ToArray();
        }

        public void SetActors(Actor source, params Actor[] targets)
        {
            this.source = source;
            this.targets = targets;
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {           
            
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
                         
        }

        public bool HasCompleted()
        {
            return true;
        }
    }
}
