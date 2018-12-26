using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest
{
    class TextureFactory
    {
        private static TextureFactory instance = null;        

        public static TextureFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TextureFactory();
                }
                return instance;
            }
        }

        private GraphicsDevice device;
        private ContentManager content;
        private Dictionary<String, Texture2D> textures;


        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.device = graphicsDevice;
            this.content = content;
            this.textures = new Dictionary<string, Texture2D>();
            this.LoadTextures();
        }

        private void LoadTextures()
        {
            // Load health texture, do this more elegant later
            var healthTexture = new Texture2D(device, 1, 1);
            healthTexture.SetData(new[] { Color.White });
            

            textures.Add("health", healthTexture);
            textures.Add("actor", content.Load<Texture2D>("graphics/actor"));
            textures.Add("buff", content.Load<Texture2D>("graphics/buff"));
            textures.Add("guard", content.Load<Texture2D>("graphics/guard"));
            textures.Add("monkey", content.Load<Texture2D>("graphics/monkey"));
            textures.Add("background", content.Load<Texture2D>("graphics/background"));
            textures.Add("meteor", content.Load<Texture2D>("graphics/meteor"));

        }

        // TODO: Should probably be changed to something like an enum, and then register all textures in the enum?
        public Texture2D GetTexture(String name)
        {
            Texture2D texture;
            if (textures.TryGetValue(name, out texture)) {
                return texture;
            } else
            {
                throw new Exception("Texture is not registered");
            }
        }
    }
}
