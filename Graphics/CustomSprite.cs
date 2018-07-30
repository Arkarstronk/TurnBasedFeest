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
            this.texture = texture;
            this.direction = direction;
        }

        public void Draw(SpriteBatch batch, float x, float y)
        {
            if (direction == SpriteDirection.RIGHT)
            {
                batch.Draw(texture, new Vector2(x, y), Color.White);
            }
            else
            {
                batch.Draw(texture, new Vector2(x, y), null, Color.White, 0, new Vector2(), new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0);                
            }            
        }
    }

    enum SpriteDirection
    {
        LEFT, RIGHT
    }
}
