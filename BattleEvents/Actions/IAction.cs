using System;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Battle;

namespace TurnBasedFeest.BattleEvents.Actions
{
    interface IAction : ITurnEvent
    {
        void SetActors(Actor source, params Actor[] targets);        
        ActionTarget GetTarget();
        string GetName();
    }

    class ActionTarget
    {
        public enum TargetSide { ENEMY, FRIENDLY }
        public enum TargetAmount { SINGLE, ALL }

        public TargetSide Side;
        public TargetAmount Amount;

        public ActionTarget(TargetSide targetSide) : this(targetSide, TargetAmount.SINGLE) { }
        public ActionTarget(TargetSide targetSide, TargetAmount amount)
        {
            this.Side = targetSide;
            this.Amount = amount;
        }

        public List<Actor> GetPossibleTargets(Actor source, List<Actor> actors)
        {
            Predicate<Actor> predicate;

            if ((source.IsPlayer() && Side == TargetSide.FRIENDLY) || (!source.IsPlayer() && Side == TargetSide.ENEMY))
            {
                predicate = x => x.IsPlayer();
            }
            else
            {
                predicate = x => !x.IsPlayer();
            }

            return actors.FindAll(predicate);
        }

        public List<Actor> GetTargets(Actor source, List<Actor> actors, int index = 0)
        {
            List<Actor> candidates = GetPossibleTargets(source, actors);
            List<Actor> targets = new List<Actor>();

            switch(Amount)
            {
                case TargetAmount.ALL:
                    targets.AddRange(candidates);
                    break;
                case TargetAmount.SINGLE:
                    targets.Add(candidates[index]);
                    break;
            }

            return targets;
        }
    }
}
