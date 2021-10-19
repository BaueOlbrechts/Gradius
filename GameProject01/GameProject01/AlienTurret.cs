using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AlienTurret : Aliens
    {
        private bool IsFlipped { get; set; }
        public int ProjectileFireTimer { get; set; } = GameSettings.PROJECTILEFIRETIMER;
        public Vector2 PointToShip { get; set; }
        public AlienTurret(Vector2 position, Texture2D texture, bool isFlipped) : base(position, texture)
        {
            ObjectSize = GameSettings.ALIENTURRETSIZE;
            ObjectSpeed = GameSettings.ALIENTURRETFORWARDSPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFALIENTURRETSPRITES;
            IsFlipped = isFlipped;
            ExplosionStartSprite = GameSettings.ALIENTURRETEXPLOSIONSPRITESTART;

            if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                StartingSprite = 6;
                SpriteIndex = StartingSprite;
            }
        }

        public override void UpdateAliens(GameTime gameTime, Vector2 shipPosition)
        {
            DecreaseProjectileTimer();
            PointTowardsAvatarShip(shipPosition);
            if (IsHit == false)
            {
                UpdateSpriteIndex();
            }
            GetSourceRectanlge();
            MoveLeftOnScreen();
            GetDestinationRectanlge();
            DeactivateIfOffScreen();
            if (IsHit == true)
            {
                AlienExplosion();
            }
        }

        private void DecreaseProjectileTimer()
        {
            ProjectileFireTimer -= 1;
        }

        private void GetSourceRectanlge()
        {
            if(IsFlipped == false)
            {
                ObjectSourceRectangle = new Rectangle(ObjectTexture.Width / NumberOfSpriteCells * SpriteIndex, 0,
                ObjectTexture.Width / NumberOfSpriteCells, ObjectTexture.Height / 2);
            }
            else
            {
                ObjectSourceRectangle = new Rectangle(ObjectTexture.Width / NumberOfSpriteCells * SpriteIndex, ObjectTexture.Height / 2,
                ObjectTexture.Width / NumberOfSpriteCells, ObjectTexture.Height / 2);
            }
        }

        private void UpdateSpriteIndex()
        {
            if (PointToShip.X > Math.Cos(Math.PI / 6))
                SpriteIndex = StartingSprite + 5;
            else if (PointToShip.X > Math.Cos(Math.PI / 3))
                SpriteIndex = StartingSprite + 4;
            else if (PointToShip.X > Math.Cos(Math.PI / 2))
                SpriteIndex = StartingSprite + 3;
            else if (PointToShip.X > Math.Cos(Math.PI * 2 / 3))
                SpriteIndex = StartingSprite + 2;
            else if (PointToShip.X > Math.Cos(Math.PI * 5 / 6))
                SpriteIndex = StartingSprite + 1;
            else
                SpriteIndex = StartingSprite;
        }

        private void PointTowardsAvatarShip(Vector2 shipPosition)
        {
            Vector2 turretCenter = new Vector2(ObjectPosition.X + ObjectSize.X / 2, ObjectPosition.Y + ObjectSize.Y / 2);
            Vector2 shipCenter = new Vector2(shipPosition.X + GameSettings.AVATARSHIPSIZE.X / 2, shipPosition.Y + GameSettings.AVATARSHIPSIZE.Y / 2);
            Vector2 turretToShip = shipCenter - turretCenter;
            turretToShip.Normalize();
            PointToShip = turretToShip;
        }
    }
}
