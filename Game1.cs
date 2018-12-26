using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.BattleEvents.Gorilla;
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

        Stats AriStats = new Stats(11, new List<IAction> { new AttackAction(), new HealAction(), new DefendAction(), new AtombBombMagicAction(), new GorillaMeteorAction() })
            .SetStat(StatisticAttribute.ATTACK, 11)
            .SetStat(StatisticAttribute.DEFENCE, 8)
            .SetStat(StatisticAttribute.SPEED, 8)
            .SetStat(StatisticAttribute.ATTACK_MAGIC, 2)
            .SetStat(StatisticAttribute.SUPPORT_MAGIC, 3);
        Stats ZinoStats = new Stats(10, new List<IAction> { new AttackAction(), new HealAction(), new DefendAction(), new AttackBuffAction(), new AtombBombMagicAction(), new GorillaMeteorAction() })
            .SetStat(StatisticAttribute.ATTACK, 4)
            .SetStat(StatisticAttribute.DEFENCE, 5)
            .SetStat(StatisticAttribute.SPEED, 9)
            .SetStat(StatisticAttribute.ATTACK_MAGIC, 7)
            .SetStat(StatisticAttribute.SUPPORT_MAGIC, 7);


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

            var ariDict = new Dictionary<StatisticAttribute, double>();
            ariDict.Add(StatisticAttribute.ATTACK, 1);
            ariDict.Add(StatisticAttribute.ATTACK_MAGIC, 0.1);
            ariDict.Add(StatisticAttribute.DEFENCE, 0.9);
            ariDict.Add(StatisticAttribute.SPEED, 0.5);
            ariDict.Add(StatisticAttribute.SUPPORT_MAGIC, 0.1);

            var zinoDict = new Dictionary<StatisticAttribute, double>();
            zinoDict.Add(StatisticAttribute.ATTACK, 0.4);
            zinoDict.Add(StatisticAttribute.ATTACK_MAGIC, 0.9);
            zinoDict.Add(StatisticAttribute.DEFENCE, 0.5);
            zinoDict.Add(StatisticAttribute.SPEED, 0.7);
            zinoDict.Add(StatisticAttribute.SUPPORT_MAGIC, 1);

            var ariSprite = CustomSprite.GetSprite("actor");
            var ariLevelingScheme = new LevelingScheme(ariDict);

            var zinoSprite = CustomSprite.GetSprite("actor");
            var zinoLevelingScheme = new LevelingScheme(zinoDict);

            heroes = new List<Actor> {
                    new Actor("Ari", Color.Red, AriStats, ariSprite, new BattleEventSelection(), new PlayerInfo(ariLevelingScheme)),
                    new Actor("Zino", Color.Blue, ZinoStats, zinoSprite, new BattleEventSelection(), new PlayerInfo(zinoLevelingScheme))
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            //currentEvent.Draw(spriteBatch, font);
            currentScreen.Draw(spriteBatch, font);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
