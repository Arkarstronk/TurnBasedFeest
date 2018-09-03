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
        int eventTime = 1000;
        int elapsedTime;

        private Actor source;
        private Actor[] targets;
        private float[] beginHps;
        private float[] targetsHps;

        public string GetName() => "Atomb Bomb";
        public bool HasCompleted() => elapsedTime >= eventTime;
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.ENEMY, ActionTarget.TargetAmount.ALL);

        public void SetActors(Actor source, params Actor[] targets)
        {
            this.source = source;
            this.targets = targets;
        }

        public void Initialize(BattleContainer battle)
        {
            elapsedTime = 0;

            int attackMagicStat = source.GetStats()[StatisticAttribute.ATTACK_MAGIC] * 3;

            beginHps = targets.Select(x => x.Health.CurrentHealth).ToArray();
            targetsHps = targets.Select(x => Math.Max(0, x.Health.CurrentHealth - attackMagicStat)).ToArray();

            for (int i = 0; i < targets.Length; i++)
            {
                battle.ParticleHelper.Add(new TextParticle($"-{attackMagicStat}", 1500, targets[i].Position + new Vector2(0, -30), new Vector2(30f, -30f)));
                targets[i].Health.SetColor(Color.Red);                
                targets[i].Health.Shake = true;
            }
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} has cast a nuclear bomb!");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].Health.CurrentHealth = MathHelper.SmoothStep(beginHps[i], targetsHps[i], (elapsedTime / (float)eventTime));
            }
            

            if (HasCompleted())
            {

                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Health.SetColor(Color.White);
                    targets[i].Health.CurrentHealth = targetsHps[i];
                    targets[i].Health.Shake = false;
                }                
            }
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {

        }

        
        
    }
}
