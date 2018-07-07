using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Ui;
using static TurnBasedFeest.Ui.BattleUI;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest
{
    class BattleSystem
    {
        public bool ongoingBattle;
        List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;
        BattleUI playerChoiceUi;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
            playerChoiceUi = new BattleUI();
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
                    playerChoiceUi.initialize(currentActor.actions, actors);
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

            spritebatch.DrawString(font, ">", getCurrentActor().position - new Vector2(35, 0), Color.White);
        }

        private Actor getCurrentActor()
        {
            if (actorEnum.Current.moveRemaining)
            {
                return actorEnum.Current;
            }
            else if (!actorEnum.MoveNext())
            {
                foreach (Actor actor in actors)
                {
                    actor.moveRemaining = true;
                }

                actorEnum = actors.GetEnumerator();
                actorEnum.MoveNext();
            }
            return actorEnum.Current;
        }
    }
}
