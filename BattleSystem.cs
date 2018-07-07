using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Ui;
using static TurnBasedFeest.Ui.BattleUI;
using System;
using System.Linq;
using TurnBasedFeest.Actions;

namespace TurnBasedFeest
{
    class BattleSystem
    {
        public bool ongoingBattle;
        List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;
        BattleUI playerChoiceUi;
        List<IAction> options;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
            playerChoiceUi = new BattleUI();
            options = new List<IAction>
            {
                new ActionAttack() , new ActionHeal(), new ActionNothing()
            };
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }

        public void Update(Input input)
        {
            Actor currentActor = getCurrentActor();

            switch (playerChoiceUi.currentState)
            {
                case state.Start:
                    playerChoiceUi.initialize(options, actors);
                    break;
                case state.Action:
                    playerChoiceUi.Update(input);
                    break;
                case state.Target:
                    playerChoiceUi.Update(input);
                    break;
                case state.Finish:
                    playerChoiceUi.GetChosenAction().Execute(currentActor, playerChoiceUi.GetChosenActor());
                    currentActor.moveRemaining = false;
                    break;
            }
            
            foreach (Actor actor in actors)
            {
                actor.Update();

                if(actor.health.actorCurrentHealth <= 0)
                {
                    EndFight();
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(spritebatch, font);
            }

            playerChoiceUi.Draw(font, spritebatch);
        }

        private Actor getCurrentActor()
        {
            if (actorEnum.Current.moveRemaining)
            {
                return actorEnum.Current;
            }
            else if (!actorEnum.MoveNext())
            {
                foreach (Actor e in actors)
                {
                    e.moveRemaining = true;
                }

                actorEnum = actors.GetEnumerator();
                actorEnum.MoveNext();
            }

            return actorEnum.Current;
        }
    }
}
