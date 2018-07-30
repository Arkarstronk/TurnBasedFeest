﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.UI;
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

        public int eventCounter;
        //public IGameEvent previousEvent;
        //public IGameEvent currentEvent;
        //public IGameEvent nextEvent;
        public Dictionary<int, string> hardcodedEvents = new Dictionary<int, string> { {100, "OnTurn100ThisStubEventTakesPlace" } };

        private UIScreen currentScreen;

        public List<Actor> heroes;

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

            var actorPlaceHolderTexture = factory.GetTexture("actor");

            var AriStats = new Stats(100, new List<IAction> { new AttackAction(), new HealAction(), new DefendAction() })
                .SetStat(StatisticAttribute.ATTACK, 60)
                .SetStat(StatisticAttribute.DEFENCE, 50)
                .SetStat(StatisticAttribute.SPEED, 20)
                .SetStat(StatisticAttribute.ATTACK_MAGIC, 90)
                .SetStat(StatisticAttribute.SUPPORT_MAGIC, 5);
            var ZinoStats = new Stats(100, new List<IAction> { new AttackAction(), new HealAction(), new DefendAction() })
                .SetStat(StatisticAttribute.ATTACK, 10)
                .SetStat(StatisticAttribute.DEFENCE, 40)
                .SetStat(StatisticAttribute.SPEED, 70)
                .SetStat(StatisticAttribute.ATTACK_MAGIC, 10)
                .SetStat(StatisticAttribute.SUPPORT_MAGIC, 30);

            var ariSprite = CustomSprite.GetSprite("actor");
            var zinoSprite = CustomSprite.GetSprite("actor");
            heroes = new List<Actor> {
                    new Actor("Ari", Color.Red, AriStats, ariSprite, new BattleEventSelection(), true),
                    new Actor("Zino", Color.Blue, ZinoStats, zinoSprite, new BattleEventSelection(), true)
            };

            eventCounter = 0;            
            SetUIScreen(new WelcomeScreen(this));            
        }

        public void SetUIScreen(UIScreen screen)
        {
            this.currentScreen = screen;
            screen.Initialize();
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
            currentScreen.Update(gameTime, input);

            /*if (currentEvent.Update(this, input))
            {
                previousEvent = currentEvent;
                currentEvent = nextEvent;
                nextEvent = null;

                currentEvent.Initialize(heroes);
            }*/

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //currentEvent.Draw(spriteBatch, font);
            currentScreen.Draw(spriteBatch, font);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
