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

        public Rectangle scale;

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
            this.scale = new Rectangle(0, 0, 1, 1);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void Scale(int x, int y)
        {
            this.scale = new Rectangle(0, 0, x, y);
        }

        public void Draw(SpriteBatch batch, float x, float y)
        {
            var rectangle = new Rectangle();
            rectangle.X = (int)x;
            rectangle.Y = (int)y;
            rectangle.Width = texture.Width * scale.Width;
            rectangle.Height = texture.Height * scale.Height;

            if (direction == SpriteDirection.RIGHT)
            {
                batch.Draw(texture, rectangle, color);
            }
            else
            {
                batch.Draw(texture, rectangle, null, color, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);                
            }            
        }
    }

    enum SpriteDirection
    {
        LEFT, RIGHT
    }
}
