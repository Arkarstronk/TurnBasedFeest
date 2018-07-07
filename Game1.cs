using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Entities;

namespace TurnBasedFeest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        KeyboardState oldKeyState;
        KeyboardState newKeyState;

        TurnSystem turnSystem;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            turnSystem = new TurnSystem();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/default");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();

            if (!turnSystem.ongoingBattle)
            {
                turnSystem.InitializeFight(new List<Entity> {
                    new Entity("player", new Vector2(100, 100), 100, GraphicsDevice),
                    new Entity("Enemy", new Vector2(600, 100), 100, GraphicsDevice)
                });
            }

            string command = "";
            if (oldKeyState.IsKeyDown(Keys.Enter) && newKeyState.IsKeyUp(Keys.Enter))
            {
                command = "attack";
            }
            else if (oldKeyState.IsKeyDown(Keys.LeftShift) && newKeyState.IsKeyUp(Keys.LeftShift))
            {
                command = "defend";
            }

            turnSystem.Update(command);           

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);            
            spriteBatch.Begin();

            turnSystem.Draw(spriteBatch, font);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
