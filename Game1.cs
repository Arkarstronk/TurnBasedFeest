using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Events.Actions;
using TurnBasedFeest.Events.TurnBehaviour;
using TurnBasedFeest.Utilities;

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
        Input input;  
        Battle battleSystem;
        List<Actor> actors;
        public static Random rnd = new Random();
        public static GameTime time;

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
            battleSystem = new Battle();
            actors = new List<Actor> {
                    new Actor("Ari", new Vector2(100, 100), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new BattleUI(), true),
                    new Actor("Zino", new Vector2(100, 200), 100, new List<IAction> { new AttackAction() , new HealAction(), new DefendAction() }, GraphicsDevice, new BattleUI(), true)
            };
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

            if (!battleSystem.ongoingBattle && input.Released(Keys.B))
            {
                actors.Add(new Actor("Hoer", new Vector2(600, 100), 100, new List<IAction> { new AttackAction(), new HealAction() }, GraphicsDevice, new RandomAI(), false));
                actors.Add(new Actor("Bitch", new Vector2(600, 200), 100, new List<IAction> { new AttackAction(), new HealAction() }, GraphicsDevice, new RandomAI(), false));
                battleSystem.InitializeFight(actors);
            }
            if (battleSystem.ongoingBattle)
            {
                BattleResult result = battleSystem.Update(input);
                if (result.isFinished)
                {
                    if (result.outcome)
                    {
                        battleSystem.EndFight();
                        actors = result.survivors;
                    }
                    else
                    {
                        Exit();
                    }
                }
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

            if (battleSystem.ongoingBattle)
            {
                battleSystem.Draw(spriteBatch, font);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
