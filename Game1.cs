using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.TurnBehaviour;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.GameEvents.Battle;
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
        public static int screenWidth = 1280;
        public static int screenHeight = 720;

        public IGameEvent currentEvent;
        public IGameEvent nextEvent;

        public List<Actor> actors;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TextureFactory factory = TextureFactory.Instance;
            factory.Initialize(GraphicsDevice, Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/default");

            currentEvent = new BattleEvent();

            var actorPlaceHolderTexture = factory.GetTexture("actor");
            actors = new List<Actor> {
                    new Actor("Ari", 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, actorPlaceHolderTexture, new BattleUI(), true),
                    new Actor("Zino", 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, actorPlaceHolderTexture, new BattleUI(), true),
                    new Actor("Stupid", 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, actorPlaceHolderTexture, new RandomAI(), false),
                    new Actor("Smart", 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, actorPlaceHolderTexture, new EfficientRandomAI(), false)
            };
            currentEvent.Initialize(actors);
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
