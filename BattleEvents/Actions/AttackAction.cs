using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Attributes;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AttackAction : IAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;
        int beginHP;
        int targetHP;
        int damage;

        private string status;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            beginHP = (int)target.health.CurrentHealth;
            elapsedTime = 0;            

            int attack = source.GetStats()[StatAttribute.ATTACK];
            int defence = target.GetStats()[StatAttribute.DEFENCE];
            int sourceSpeed = source.GetStats()[StatAttribute.SPEED];
            int targetSpeed = target.GetStats()[StatAttribute.SPEED];


            foreach (IAttribute attribute in target.attributes.FindAll(x => x.GetAttributeType() == attributeType.INCOMING))
            {
                defence = (int) (defence * attribute.GetMultiplier()) + (int)attribute.GetAddition();
            }

            damage = (int) (10 * (attack / (float) defence));
            status = $"{source.name} used Attack on {target.name}";

            targetHP = (int) ((target.health.CurrentHealth - damage <= 0) ? 0 : (target.health.CurrentHealth - damage));
            target.health.color = Color.Yellow;
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            battle.battle.battleText = status;
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                target.health.color = Color.White;
                target.health.CurrentHealth = targetHP;

                //if this attack killed its target
                if (target.health.CurrentHealth == 0)
                {
                    battle.CurrentActor.battleEvents.Insert(battle.eventIndex + 1, new DeathEvent(target));
                }                

                battle.CurrentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;
                return true;
            }
            else
            {
                return false;
            }            
        }

        public string GetName()
        {
            return "Attack";
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, damage.ToString(), target.position + new Vector2(0, -(elapsedTime / (float) eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }

        public bool IsSupportive()
        {
            return false;
        }
    }
}
