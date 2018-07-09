using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.TurnBehaviour;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Input input;  
        public static Random rnd = new Random();
        public static GameTime time;

        public IGameEvent currentEvent;
        public IGameEvent nextEvent;

        public List<Actor> actors;

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
            input = new Input();
            currentEvent = new BattleEvent();
            actors = new List<Actor> {
                    new Actor("Ari", new Vector2(100, 100), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new BattleUI(), true),
                    new Actor("Zino", new Vector2(100, 200), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new BattleUI(), true),
                    new Actor("Stupid", new Vector2(600, 100), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new RandomAI(), false),
                    new Actor("Smart", new Vector2(600, 200), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new EfficientRandomAI(), false)
            };
            currentEvent.Initialize(actors);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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
            time = gameTime;
            input.Update();

            if (currentEvent.Update(this, input))
            {
                currentEvent = nextEvent;
                currentEvent.Initialize(actors);
            }

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
            
            currentEvent.Draw(spriteBatch, font);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
