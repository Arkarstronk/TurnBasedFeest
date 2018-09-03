using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedFeest.Actors
{
    // A class that tells when enough xp is gained for a level up.
    class LevelingScheme
    {

        
        // When xp is reached,
        List<LevelUp> levelList = new List<LevelUp>();        

        public LevelUp this[int level]
        {
            get
            {
                if(level >= levelList.Count)
                {
                    return levelList.Last();
                } else
                {
                    return levelList[level];
                }
            }
        }

        public LevelingScheme(Dictionary<StatisticAttribute, double> ariDict)
        {
            levelList = initList(ariDict);
        }

        private List<LevelUp> initList(Dictionary<StatisticAttribute, double> probabilities)
        {
            List<LevelUp> list = new List<LevelUp>();   

            for (int i = 0; i < 50; i++)
            {
                list.Add(new LevelUp(11 * i * i, probabilities));
            }
            for (int i = 50; i < 100; i++)
            {
                list.Add(new LevelUp(11 * 50 * 50 + i, probabilities));
            }

            return list;
        }
    }

    class LevelUp
    {
        public int XP;
        public Dictionary<StatisticAttribute, double> LevelUpProbabilities;

        public LevelUp(int xp, Dictionary<StatisticAttribute, double> levelUpProbabilities)
        {
            this.XP = xp;
            this.LevelUpProbabilities = levelUpProbabilities;
        }
    }
}
