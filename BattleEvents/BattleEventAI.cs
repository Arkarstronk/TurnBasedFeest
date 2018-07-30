using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Battle
{
    class BattleEventAI : BattleEvent
    {
        private BattleContainer battle;
        private IAction randomAction;

        public void Initialize(BattleContainer battle)
        {
            this.battle = battle;

            // Get the possible actions
            var actions = battle.CurrentActor.GetActions();

            // Choose a random action
            randomAction = actions[Game1.rnd.Next(actions.Count)];

            // Get the possible targets
            List<Actor> possibleTargets;
            if (randomAction.IsSupportive())
            {
                possibleTargets = battle.GetAliveActors().FindAll(x => !x.isPlayer);
            }
            else
            {
                possibleTargets = battle.GetAliveActors().FindAll(x => x.isPlayer);
            }

            // Select a random target
            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];


            randomAction.SetActors(battle.CurrentActor, randomActor);
            randomAction.Initialize();
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            randomAction.Draw(battle, batch, font);   
        }

        public void Update(GameTime gameTime, Input input)
        {
            randomAction.Update(battle, gameTime, input);
        }

        public bool HasCompleted()
        {
            return randomAction.HasCompleted();
        }

    }
}
