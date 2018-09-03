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

    // This class handles the AI actions
    class BattleEventAI : BattleEvent
    {
        private BattleContainer battle;
        private ITurnEvent action;        

        public void Initialize(BattleContainer battle)
        {
            this.battle = battle;

            // Get the possible actions
            var actions = battle.CurrentActor.GetActions();

            // Choose a random action
            IAction action = actions[Game1.rnd.Next(actions.Count)];

            // Get the possible targets
            List<Actor> possibleTargets = action.GetTarget().GetPossibleTargets(battle.CurrentActor, battle.GetAliveActors());

            // Select a random target
            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];


            action.SetActors(battle.CurrentActor, randomActor);
            action.Initialize(battle);

            this.action = action;
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            action.Draw(battle, batch, font);   
        }

        public void Update(GameTime gameTime, Input input)
        {
            action.Update(battle, gameTime, input);
        }

        public bool HasCompleted()
        {
            if (action.HasCompleted() && action is IContinueableAction && (action as IContinueableAction).HasNextEvent())
            {
                action = (action as IContinueableAction).NextEvent() ?? action;
            }
            return action.HasCompleted();
        }

    }
}
