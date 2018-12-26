using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.Actors
{
    // The stats of an actor
    // Stats are like attack, defence or maybe even speed points.
    class Stats
    {
        public int MaxHealth;
        public List<IAction> Actions;
        private Dictionary<StatisticAttribute, int> stats;
        
        public int this[StatisticAttribute key, List<IAttribute> attributes]
        {
            get
            {
                if (stats.ContainsKey(key))
                {
                    var applicableAttributes = attributes.Where(x => x.getStatistic() == key);
                    float multiplied = applicableAttributes.Select(x => x.GetMultiplier()).Aggregate((float)stats[key], (acc, x) => acc * x);
                    float added = applicableAttributes.Select(x => x.GetAddition()).Sum();
                    return (int)(multiplied + added);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int this[StatisticAttribute key]
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

        public static Stats GetRandom(int health, int distributionPoints, List<IAction> actions)
        {
            Stats stats = new Stats(health, actions);
            Random rnd = new Random();
            new List<StatisticAttribute>(Enum.GetValues(typeof(StatisticAttribute)).Cast<StatisticAttribute>())
                .ForEach(x => stats[x] = rnd.Next(distributionPoints));

            return stats;
        }
        public Stats(int health, List<IAction> actions)
        {
            this.stats = new Dictionary<StatisticAttribute, int>();
            this.MaxHealth = health;
            this.Actions = actions;
        }

        public Stats SetStat(StatisticAttribute attributes, int value)
        {
            stats[attributes] = value;
            return this;
        }
    }

    enum StatisticAttribute
    {
        ATTACK, DEFENCE, SPEED, ATTACK_MAGIC, SUPPORT_MAGIC
    }
}
