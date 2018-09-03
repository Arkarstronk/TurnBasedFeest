using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AtombBombMagicAction : IAction
    {        
        private Actor source;
        private Actor[] targets;

        private CustomSprite meteorSprite;


        public string GetName() => "Atomb Bomb";
        public bool HasCompleted() => true;
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.ENEMY, ActionTarget.TargetAmount.ALL);

        public void SetActors(Actor source, params Actor[] targets)
        {
            this.source = source;
            this.targets = targets;
        }

        public void Initialize(BattleContainer battle)
        {
            meteorSprite = CustomSprite.GetSprite("meteor");
            int attackMagicStat = source.GetStats()[StatisticAttribute.ATTACK_MAGIC] * 3;

            float[] beginHps = targets.Select(x => x.Health.CurrentHealth).ToArray();


            battle.Animations.Add(new AnimationTimer(3000));

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    float dx = (float)Math.Sin(j * Math.PI / 50);
                    float dy = (float)Math.Cos(j * Math.PI / 50);

                    Vector2 direction = new Vector2(dx, dy) * (i + 2) * 7;

                    Particle particle = new Particle(meteorSprite, 2000 + Game1.rnd.Next(1000), source.Position, direction);
                    
                    battle.ParticleHelper.Add(particle);
                }
            }

            for (int i = 0; i < targets.Length; i++)
            {
                battle.ParticleHelper.Add(new TextParticle($"-{attackMagicStat}", 1500, targets[i].Position + new Vector2(0, -30), new Vector2(30f, -30f)));
                targets[i].Health.SetColor(Color.Red);                
                targets[i].Health.Shake = true;
                battle.Animations.Add(new AnimationHealthChange(targets[i], beginHps[i], attackMagicStat));
            }
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} has cast a nuclear bomb!");            
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
