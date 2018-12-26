using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedFeest.Graphics
{
    class ParticleHelper
    {
        public enum ParticleScope { PUBLIC, LOCAL }

        private Dictionary<ParticleScope, List<Particle>> particles = new Dictionary<ParticleScope, List<Particle>>();

        public void RemoveAll(ParticleScope scope = ParticleScope.LOCAL)
        {
            particles[scope] = new List<Particle>();
        }

        public void Add(Particle particle, ParticleScope scope = ParticleScope.LOCAL)
        {
            if (!particles.ContainsKey(scope)) particles[scope] = new List<Particle>();

            particles[scope].Add(particle);
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            foreach (var c in particles)
            {
                c.Value.ForEach(x => x.Draw(batch, font));
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (var c in particles)
            {
                c.Value.AsParallel().ForAll(x => x.Update(gameTime));                
                c.Value.RemoveAll(x => x.IsDead);
            }
        }

    }

    class Particle
    {
        
        private float age;
        private readonly float maxAge;
        public CustomSprite sprite;

        protected Vector2 position;
        protected Vector2 direction;
        protected Action<Particle, double> extraAction;
        public bool IsDead => age >= maxAge;

        public Particle(CustomSprite sprite, float maxAge, Vector2 position, Vector2 direction)
        {
            this.age = 0;
            this.maxAge = maxAge;
            this.sprite = sprite;
            this.position = position;
            this.direction = direction;
        }

        public void SetExtra(Action<Particle, double> action)
        {
            extraAction = action;
        }

        public void Update(GameTime gameTime)
        {            
            position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * (maxAge - age) / 1000;
            age += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            extraAction?.Invoke(this, age / maxAge);
        }

        public virtual void Draw(SpriteBatch batch, SpriteFont font)
        {
            sprite.Draw(batch, position.X, position.Y);
        }        
    }

    class TextParticle : Particle
    {
        private readonly string text;

        public TextParticle(String text, float maxAge, Vector2 position, Vector2 direction) : base(null, maxAge, position, direction)
        {
            this.text = text;            
        }

        public override void Draw(SpriteBatch batch, SpriteFont font)
        {
            
            batch.DrawString(font, text, position, Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1f);
        }
    }
}
