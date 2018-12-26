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
        public int Level;
        public int ExperiencePoints;
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

        // Add experience to the player, and if enough experience is gained level up.
        public ExperienceResult AddExperience(int experience, LevelingScheme levelingSlate)
        {
            ExperienceResult result = new ExperienceResult(experience, Level);
            // Add experience points
            ExperiencePoints += experience;
                        

            // If enough experience is gained
            while(ExperiencePoints >= levelingSlate[Level + 1].XP)
            {
                // Level up
                Level++;

                // Subtract experience points to reset, otherwise you would level up too quickly
                ExperiencePoints -= levelingSlate[Level + 1].XP;

                // For each stat attribute there is a probability it increases
                levelingSlate[Level + 1].LevelUpProbabilities.ToList().ForEach(pair => {
                    StatisticAttribute attribute = pair.Key;
                    double probability = pair.Value;

                    if (Game1.rnd.NextDouble() <= probability)
                    {
                        int oldStat = this[attribute];                        
                        SetStat(attribute, oldStat + 1);

                        if (result.statups.ContainsKey(attribute))
                        {
                            result.statups[attribute] = new Tuple<int, int>(result.statups[attribute].Item1, result.statups[attribute].Item2 + 1);
                        } else
                        {
                            result.statups.Add(attribute, new Tuple<int, int>(oldStat, oldStat + 1));
                        }
                    }
                });

                // Increase the max health with each level up.
                MaxHealth += Game1.rnd.Next(5, 8);

                // Notify the result that we leveled up
                result.LeveledUp = true;
                result.NewLevel = Level;
            }

            return result;
        }
    }

    public enum StatisticAttribute
    {
        ATTACK, DEFENCE, SPEED, ATTACK_MAGIC, SUPPORT_MAGIC
    }

    public class ExperienceResult
    {
        public int Level;
        public int NewLevel;
        public int Experience;
        public bool LeveledUp;

        public Dictionary<StatisticAttribute, Tuple<int, int>> statups;

        public ExperienceResult(int experience, int level)
        {
            this.Level = level;
            this.NewLevel = Level;
            this.Experience = experience;
            this.LeveledUp = false;

            statups = new Dictionary<StatisticAttribute, Tuple<int, int>>();
        }
    }
}
