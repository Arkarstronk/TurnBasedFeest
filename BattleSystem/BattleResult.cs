using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.BattleSystem
{
    class BattleResult
    {
        public Boolean isFinished;
        public Boolean outcome;
        public List<Actor> survivors;

        public BattleResult(bool isFinished, bool outcome, List<Actor> survivors)
        {
            this.isFinished = isFinished;
            this.outcome = outcome;
            this.survivors = survivors;
        }
    }
}
