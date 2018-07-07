using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.Ui
{
    class ListChoiceUi
    {
        public enum state
        {
            Notused,
            Used,
            Done,
        }
        public state currentState;
        string choice;
        private List<string> choices;


        public ListChoiceUi()
        {
            currentState = state.Notused;
        }

        public void PromptUser(List<string> choices)
        {
            currentState = state.Used;
            this.choices = choices;
        }

        public void Update(Input input)
        {
            if (input.Released(Keys.Enter))
            {
                choice = choices[0];
                currentState = state.Done;
            }
            if (input.Released(Keys.RightShift))
            {
                choice = choices[1];
                currentState = state.Done;
            }
        }

        public string getChoice()
        {
            currentState = state.Notused;
            return choice;
        }
    }
}
