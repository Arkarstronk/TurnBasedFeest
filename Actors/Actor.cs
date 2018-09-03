using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TurnBasedFeest.Attributes;
using TurnBasedFeest.BattleEvents;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest.Actors
{
    abstract class ActorInfo
    {
        public virtual bool IsPlayer() => false;
    }

    class EnemyInfo : ActorInfo
    {
        private int experience;

        public EnemyInfo(int experience)
        {
            this.experience = experience;
        }
        public int GetXP()
        {            
            return this.experience;
        }
    }

    class PlayerInfo : ActorInfo
    {
        private LevelingScheme levelingScheme;

        public PlayerInfo(LevelingScheme levelingScheme)
        {
            this.levelingScheme = levelingScheme;
        }

        public override bool IsPlayer() => true;

        public LevelingScheme GetLevelingScheme()
        {
            return levelingScheme;    
        }
    }
    class Actor
    {
        public string Name;
        public Vector2 Position;
        public Health Health;
        public BattleEvent StartEvent { get; }        
        
        public List<GivenAttribute> HandedOutAttributes = new List<GivenAttribute>();
        public List<IAttribute> Attributes = new List<IAttribute>();        
        public bool HasTurn;
        //public bool IsPlayer;
        public ActorInfo Info;
        public Color Color;

        private Stats stats;
        private CustomSprite sprite;

        public Actor(string name, Color color, Stats stats, CustomSprite sprite, BattleEvent behaviourEvent, ActorInfo info)
        {
            this.Name = name;
            this.Color = color;
            this.stats = stats;
            this.Health = new Health(stats.MaxHealth);
            this.sprite = sprite;
            this.StartEvent = behaviourEvent;
            this.Info = info;
        }

        public void Initialize()
        {
            HasTurn = true;
        }

        // The actor starts its turn here
        public void OnTurnStart()
        {
            // Handle attributes expiration
            HandedOutAttributes.ForEach(x => x.Expiration--);
            HandedOutAttributes.FindAll(x => x.Expiration <= 0).ForEach(x => {
                x.receiver.Attributes.Remove(x.attribute);
            });
        }

        public void Update(GameTime gameTime)
        {
            Health.Update();   
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode offset size            
            spritebatch.DrawString(font, Name, Position + new Vector2(0,-90), Color.White);            
            Health.Draw(spritebatch, Position, font);

            for (int i = 0; i < Attributes.Count; i++)
            {
                Attributes[i].Draw(spritebatch, font, Position + new Vector2(i * 17, 0));
            }

            if (!IsAlive()) sprite.SetColor(Color.DarkSlateGray);

            sprite.Draw(spritebatch, Position.X, Position.Y);            
        }

        public bool IsAlive()
        {
            return Health.CurrentHealth > 0;
        }

        public List<IAction> GetActions()
        {
            return stats.Actions;
        }

        public Stats GetStats()
        {
            return this.stats;
        }

        public bool IsPlayer()
        {
            return Info.IsPlayer();
        }
    }
}
