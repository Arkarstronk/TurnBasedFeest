using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedFeest.GameEvents.UI
{
    class MultipleChoiceEvent
    {
        List<string> choices;
        int index;

        public MultipleChoiceEvent(List<string> choices)
        {
            this.choices = choices;
        }

        public int Update(GameTime gameTime, Input input)
        {
            //move index
            if (input.Pressed(Keys.Down))
            {
                index++;
            }
            if (input.Pressed(Keys.Up))
            {
                index--;
            }

            //check bounds
            if (index < 0)
            {
                index = choices.Count - 1;
            }
            if (index >= choices.Count)
            {
                index = 0;
            }

            if (input.Pressed(Keys.Enter))
            {
                return index;
            }
            return -1;
        }
        
        public void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position)
        {
            for (int i = 0; i < choices.Count; i++)
            {
                spritebatch.DrawString(font, choices[i], position + new Vector2(0, 20 * i), (i == index ? Color.Yellow : Color.White));
            }
        }
    }
}
