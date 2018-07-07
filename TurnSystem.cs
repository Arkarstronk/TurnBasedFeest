using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Entities;

namespace TurnBasedFeest
{
    class TurnSystem
    {
        public bool ongoingBattle;
        List<Entity> entities = new List<Entity>();
        List<Entity>.Enumerator entityEnum;

        public void InitializeFight(List<Entity> entities)
        {
            ongoingBattle = true;
            this.entities = entities;
            entityEnum = this.entities.GetEnumerator();
            entityEnum.MoveNext();
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }

        public void Update(string command)
        {
            if (entityEnum.Current.moveRemaining)
            {
                // TODO: do not hardcode target
                Entity target = entities.Find(x => x.name != entityEnum.Current.name);

                if (command == "attack")
                {
                    target.entityCurrentHealth -= 10;
                    entityEnum.Current.moveRemaining = false;

                }
                if (command == "defend")
                {
                    entityEnum.Current.entityCurrentHealth += 10;
                    entityEnum.Current.moveRemaining = false;
                }
            }
            else if (!entityEnum.MoveNext())
            {
                foreach (Entity e in entities)
                {
                    e.moveRemaining = true;
                }

                entityEnum = entities.GetEnumerator();
                entityEnum.MoveNext();
            }

            foreach(Entity entity in entities)
            {
                entity.Update();

                if(entity.entityCurrentHealth <= 0)
                {
                    EndFight();
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Entity entity in entities)
            {
                entity.Draw(spritebatch, font);
            }
        }
    }
}
