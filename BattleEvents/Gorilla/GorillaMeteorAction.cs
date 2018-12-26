using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Gorilla
{
    class GorillaMeteorAction : IAction
    {        
        private Actor source;
        private Actor[] targets;
        private int[] targetsHP;

        private AnimationHelper animationHelper;

        private CustomSprite meteorSprite;
        private double[] scales;
        private Vector2[] positions;
        private Vector2[] speeds;

        private const int MAX_METEORS = 2000;

        public string GetName()
        {
            return "Gorilla Meteor";
        }

        public void Initialize()
        {
            // Initiate meteors
            meteorSprite = CustomSprite.GetSprite("meteor");
            scales = new double[MAX_METEORS];
            positions = new Vector2[MAX_METEORS];
            speeds = new Vector2[MAX_METEORS];

            for (int i = 0; i < MAX_METEORS; i++)
            {
                int distance = Game1.rnd.Next((int)(2000 * 4 * scales[i]));
                scales[i] = Game1.rnd.NextDouble() + 0.5;
                positions[i] = new Vector2(distance + 800 + Game1.rnd.Next(-8000, 800), -20 + distance - Game1.rnd.Next(500));
                speeds[i] = new Vector2((float)(-1.1 + 0.2 * Game1.rnd.NextDouble()), (float)(1.1 + 0.2 * Game1.rnd.NextDouble()));
                speeds[i].Normalize();
                speeds[i].X *= 1400 * (float)(1.6 - scales[i]);
                speeds[i].Y *= 1400 * (float)(1.6 - scales[i]);
            }

            foreach(Actor target in this.targets)
            {
                target.Health.SetColor(Color.DarkRed);
                target.Health.Shake = true;
            }

            int attack = source.GetStats()[StatisticAttribute.ATTACK_MAGIC] * 4;

            this.targetsHP = targets
                .Select(x =>
                {
                    int defence = x.GetStats()[StatisticAttribute.DEFENCE, x.Attributes];
                    int damage = Math.Max(2, attack - defence);
                    return Math.Max(0, (int)(x.Health.CurrentHealth - damage));
                })
                .ToArray();

            animationHelper = new AnimationHelper(3000, percentage => {
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
            for (int i = 0; i < MAX_METEORS; i++)
            {
                meteorSprite.Scale(2.5f * (float)scales[i], 2.5f * (float)scales[i]);
                var v = Math.Max(0.4f, 1.6f - (float)scales[i]);
                meteorSprite.SetColor(new Color(v, v, v));
                //meteorSprite.SetDepth((float)scales[i]);
                meteorSprite.Draw(spritebatch, positions[i].X, positions[i].Y);
            }         
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            animationHelper.Update(gameTime);

            for (int i = 0; i < MAX_METEORS; i++)
            {
                positions[i].X += speeds[i].X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                positions[i].Y += speeds[i].Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (animationHelper.HasCompleted())
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
                }
            }                        
        }

        public bool HasCompleted()
        {
            return animationHelper.HasCompleted();
        }
    }
}
