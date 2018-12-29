using System;
using System.Collections.Generic;
using System.Linq;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.Actors
{
    // The stats of an actor
    // Stats are like attack, defence or maybe even speed points.
    class Stats
    {
        public List<IAction> Actions;
        private Dictionary<StatAttribute, int> stats;
        
        public int this[StatAttribute key]
        {
            get
            {
                if (stats.ContainsKey(key))
                {
                    return stats[key];
                } else
                {
                    return 0;
                }
            }            

            set
            {
                stats[key] = value;
            }
        }

        public Stats(List<IAction> actions)
        {
            this.stats = new Dictionary<StatAttribute, int>();
            this.Actions = actions; 
        }

        public static Stats GetUniformRandom(int averagePoints, int divergence, List<IAction> actions)
        {
            Stats stats = new Stats(actions);
            stats[StatAttribute.HP] = Game1.rnd.Next(averagePoints - divergence, averagePoints + divergence);
            stats[StatAttribute.ATTACK] = Game1.rnd.Next(averagePoints - divergence, averagePoints + divergence);
            stats[StatAttribute.DEFENCE] = Game1.rnd.Next(averagePoints - divergence, averagePoints + divergence);
            stats[StatAttribute.CHROMA] = Game1.rnd.Next(averagePoints - divergence, averagePoints + divergence);
            stats[StatAttribute.SPEED] = Game1.rnd.Next(averagePoints - divergence, averagePoints + divergence);
            
            return stats;
        }

        public Stats SetStat(StatAttribute attributes, int value)
        {
            stats[attributes] = value;
            return this;
        }
    }

    public enum StatAttribute
    {
        HP, ATTACK, DEFENCE, CHROMA, SPEED
    }
    //HP is for maximum hp 
    //Attack is for flat damage infliction
    //Defence is for flat damage absorbtion
    //Chroma is for weakness effectiveness/multiplier and maximum chroma
    //Speed is for turn speed and crit 
}
