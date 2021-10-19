using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class Aliens : GameObject
    {
        protected int ExplosionStartSprite { get; set; }
        protected int StartingSprite { get; set; }
        public Aliens(Vector2 position, Texture2D texture) : base(position, texture)
        {
        }


        public override void UpdateAliens(GameTime gameTime, Vector2 ShipPosition)
        {
            MoveLeftOnScreen();
            base.Update(gameTime);
            DeactivateIfOffScreen();
            if (IsHit == true)
            {
                AlienExplosion();
            }
        }

        protected void DeactivateIfOffScreen()
        {
            if (ObjectPosition.X + ObjectSize.X <= 0)
                IsActive = false;
        }

        protected void MoveLeftOnScreen()
        {
            ObjectPosition = new Vector2(ObjectPosition.X - ObjectSpeed, ObjectPosition.Y);
        }

        protected void AlienExplosion()
        {
            float ExplosionDurationPercentage = (float)ExplosionCount / GameSettings.EXPLOSIONDURATION;

            if (ExplosionDurationPercentage >= 0.66)
            {
                SpriteIndex = ExplosionStartSprite;
            }
            else if (0.66 > ExplosionDurationPercentage && ExplosionDurationPercentage >= 0.33)
            {
                SpriteIndex = ExplosionStartSprite + 1;
            }
            else if (0.33 > ExplosionDurationPercentage && ExplosionDurationPercentage >= 0)
            {
                SpriteIndex = ExplosionStartSprite + 2;
            }

            ExplosionCount--;

            if (ExplosionCount <= 0)
            {
                IsActive = false;
            }
        }
    }
}
