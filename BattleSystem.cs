using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Ui;
using static TurnBasedFeest.Ui.ListChoiceUi;
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
        ListChoiceUi playerChoiceUi;
        Dictionary<string, IAction> options;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
            playerChoiceUi = new ListChoiceUi();
            options = new Dictionary<string, IAction>
            {
                { "attack", new ActionAttack() },
                { "heal", new ActionHeal() },
                { "defend", new ActionNothing() }
            };
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }

        public void Update(Input input)
        {
            Actor currentActor = getCurrentActor();
            Actor target = actors.Find(x => x.name != actorEnum.Current.name);

            switch (playerChoiceUi.currentState)
            {
                case state.Notused:
                    playerChoiceUi.PromptUser(options.Keys.ToList());
                    break;
                case state.Used:
                    playerChoiceUi.Update(input);
                    break;
                case state.Done:
                    options[playerChoiceUi.getChoice()].Execute(currentActor, target);
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

            playerChoiceUi.Update(input);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(spritebatch, font);
            }
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
