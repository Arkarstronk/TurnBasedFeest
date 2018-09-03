using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.BattleEvents.Gorilla;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest.Actors.npcs
{
    abstract class Enemy
    {
        protected abstract string GetName(int id);
        protected abstract Stats GetStats();
        protected abstract CustomSprite GetSprite();
        protected abstract int GetXP();

        protected virtual BattleEvent GetBehaviour() => new BattleEventAI();

        public Actor Create(int id)
        {
            return new Actor(GetName(id), Color.White, GetStats(), GetSprite(), GetBehaviour(), new EnemyInfo(GetXP()));
        }
    }
    // Level 1 enemies?
    class Platipus_1 : Enemy
    {
        protected override string GetName(int id) => $"Very Angry Platipus {id}";
        protected override CustomSprite GetSprite() => CustomSprite.GetSprite("monkey", SpriteDirection.LEFT);
        protected override int GetXP() => 10;
        protected override Stats GetStats()
        {
            return new Stats(3, new List<BattleEvents.Actions.IAction>() { new AttackAction() })
                .SetStat(StatisticAttribute.ATTACK, 5)                
                .SetStat(StatisticAttribute.DEFENCE, 3)
                .SetStat(StatisticAttribute.SPEED, 3);
        }

    }

    class BattleMonkey_1 : Enemy
    {
        protected override string GetName(int id) => $"Battle Monkey {id}";
        protected override CustomSprite GetSprite() => CustomSprite.GetSprite("monkey", SpriteDirection.LEFT);
        protected override int GetXP() => 11;

        protected override Stats GetStats()
        {
            return new Stats(4, new List<BattleEvents.Actions.IAction>() { new AttackAction() })
                .SetStat(StatisticAttribute.ATTACK, 7)
                .SetStat(StatisticAttribute.DEFENCE, 3)
                .SetStat(StatisticAttribute.SPEED, 3);
        }
    }

    class BattleGorilla : Enemy
    {
        protected override string GetName(int id) => $"Battle Monkey {id}";

        protected override CustomSprite GetSprite() {
            var enemySprite = CustomSprite.GetSprite("monkey", SpriteDirection.LEFT);
            enemySprite.Scale(4, 5);

            return enemySprite;
        }
        protected override int GetXP() => 200;
        protected override BattleEvent GetBehaviour() => new GorillaAI();
        
        protected override Stats GetStats()
        {
            return new Stats(100, new List<IAction> { new AttackAction(), new DefendAction(), new HealAction() })
                .SetStat(StatisticAttribute.ATTACK, 11)
                .SetStat(StatisticAttribute.DEFENCE, 5)
                .SetStat(StatisticAttribute.SUPPORT_MAGIC, 2)
                .SetStat(StatisticAttribute.ATTACK_MAGIC, 2)
                .SetStat(StatisticAttribute.SPEED, 2);
        }
    }
}
