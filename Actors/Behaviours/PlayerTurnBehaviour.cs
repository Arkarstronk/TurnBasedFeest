using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actions;
using TurnBasedFeest.UI;
using TurnBasedFeest.Utilities;
using static TurnBasedFeest.UI.BattleUI;

namespace TurnBasedFeest.Actors.Behaviours
{
    class PlayerTurnBehaviour : ITurnBehaviour
    {
        BattleUI playerUI;

        public PlayerTurnBehaviour()
        {
            playerUI = new BattleUI();
        }

        public bool DetermineBehaviour(Input input, List<Actor> actors, Actor currentActor)
        {
            switch (playerUI.currentState)
            {
                case state.Start:
                    playerUI.initialize(currentActor.actions, actors);
                    break;
                case state.Action:
                    playerUI.Update(input);
                    break;
                case state.Target:
                    playerUI.Update(input);
                    break;
                case state.Finish:
                    return true;
            }

            return false;
        }

        public ITurnResult GetTurnResult()
        {
            return playerUI.GetTurnResult();
        }

        public void Draw(SpriteFont font, SpriteBatch spritebatch)
        {
            playerUI.Draw(font, spritebatch);
        }
    }

    class PlayerTurnResult : ITurnResult
    {
        IAction resultAction;
        Actor targetActor;

        public PlayerTurnResult(IAction action, Actor target)
        {
            this.resultAction = action;
            targetActor = target;
        }

        public void Preform(Actor preformer)
        {
            resultAction.Execute(preformer, targetActor);
        }
    }
}
