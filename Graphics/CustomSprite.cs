using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedFeest.Graphics
{
    class CustomSprite
    {
        private Texture2D texture;
        private SpriteDirection direction;
        private Color color;

        private Vector2 scale;
        private float depth = 0;


        public static CustomSprite GetSprite(string name)
        {
            return new CustomSprite(TextureFactory.Instance.GetTexture(name), SpriteDirection.RIGHT);
        }

        public static CustomSprite GetSprite(string name, SpriteDirection direction)
        {
            return new CustomSprite(TextureFactory.Instance.GetTexture(name), direction);
        }

        private CustomSprite(Texture2D texture, SpriteDirection direction)
        {
            this.color = Color.White;
            this.texture = texture;
            this.direction = direction;
            Scale(1, 1);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void SetDepth(float depth)
        {
            this.depth = Math.Max(0, Math.Min(1, depth)) + 0.01f;
            
        }

        public void Scale(float x, float y)
        {
            this.scale = new Vector2(x, y);
        }

        public void Draw(SpriteBatch batch, float x, float y)
        {
            var rectangle = new Rectangle();
            rectangle.X = (int)x;
            rectangle.Y = (int)y;
            rectangle.Width = (int)(texture.Width * scale.X);
            rectangle.Height = (int)(texture.Height * scale.Y);

            if (direction == SpriteDirection.RIGHT)
            {
                batch.Draw(texture, rectangle, null, color, 0, new Vector2(0, 0), SpriteEffects.None, depth);
            }
            else
            {
                batch.Draw(texture, rectangle, null, color, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, depth);                
            }            
        }
    }

    enum SpriteDirection
    {
        LEFT, RIGHT
    }
}
