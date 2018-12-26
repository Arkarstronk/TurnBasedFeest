using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Gorilla
{
    class GorillaSweepAction : IAction
    {        
        private Actor source;
        private Actor[] targets;
        private int[] targetsHP;
        private AnimationHelper animationHelper;

        public string GetName()
        {
            return "Gorilla Sweep";
        }

        public void Initialize()
        {            
            foreach (Actor target in this.targets)
            {
                target.Health.Shake = true;
            }

            int attack = source.GetStats()[StatisticAttribute.ATTACK];

            this.targetsHP = targets
                .Select(x =>
                {
                    int defence = x.GetStats()[StatisticAttribute.DEFENCE, x.Attributes];
                    int damage = Math.Max(2, attack - defence);
                    return Math.Max(0, (int)(x.Health.CurrentHealth - damage));
                })
                .ToArray();

            animationHelper = new AnimationHelper(1000, percentage => {
                for (int i = 0; i < targets.Length; i++)
                {
                    Actor target = targets[i];
                    int targetHP = targetsHP[i];
                    int difference = (int)(target.Health.CurrentHealth - targetHP);
                    target.Health.CurrentHealth = targetHP + (int)(difference * percentage);
                }
            });
        }

        public bool IsSupportive()
        {
            return false;
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
            battle.PushSplashText($"{source.Name} sweeped over the arena!");
            animationHelper.Update(gameTime);

            if (HasCompleted())
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    Actor target = targets[i];
                    int targetHP = targetsHP[i];
                    target.Health.Shake = false;
                    target.Health.SetColor(Color.White);
                    target.Health.CurrentHealth = targetHP;
                    target.Health.Shake = false;

                    if (!target.IsAlive())
                    {
                        //nextEvent = new DeathEvent(target);
                    }
                    if (target.Attributes.Count > 0)
                        target.Attributes.RemoveAt(0);
                }
            }
        }

        public bool HasCompleted()
        {
            return animationHelper.HasCompleted();
        }
    }
}
