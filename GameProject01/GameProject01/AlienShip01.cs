using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AlienShip01 : Aliens
    {
        // Variant Alienship that hones in on the ship
        private float BaseObjectSpeed { get; set; }
        public AlienShip01 (Vector2 position, Texture2D texture) : base(position, texture)
        {
            ObjectSize = GameSettings.ALIENSHIP01SIZE;
            BaseObjectSpeed = GameSettings.ALIENSHIP01FORWARDSPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFALIEN01SPRITES;
            ExplosionStartSprite = GameSettings.ALIENSHIP01EXPLOSIONSPRITESTART;
            if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                StartingSprite = 3;
                SpriteIndex = StartingSprite;
                BaseObjectSpeed += 2;
            }
            ObjectSpeed = BaseObjectSpeed;

        }

        public override void UpdateAliens(GameTime gameTime, Vector2 shipPosition)
        {
            MoveUpOrDownIfNecessary(shipPosition);
            base.UpdateAliens(gameTime, shipPosition);
        }

        private void MoveUpOrDownIfNecessary(Vector2 shipPosition)
        {
            if(IsHit == false)
            {
                if (ObjectPosition.X + ObjectSize.X >= shipPosition.X && ObjectPosition.X + ObjectSize.X * 2 <= GameSettings.WINDOWWIDTH)
                {
                    if (ObjectPosition.Y + GameSettings.ALIENSHIP01SIZE.Y / 2 >
                        shipPosition.Y + GameSettings.AVATARSHIPSIZE.Y / 2 + GameSettings.PLAYONHONING)
                    {
                        ObjectPosition = new Vector2(ObjectPosition.X, ObjectPosition.Y - GameSettings.ALIENSHIP01UPDOWNSPEED);
                        ObjectSpeed = BaseObjectSpeed / 1.5f;
                        SpriteIndex = StartingSprite + 2;
                    }

                    else if (ObjectPosition.Y + GameSettings.ALIENSHIP01SIZE.Y / 2 <
                        shipPosition.Y + GameSettings.AVATARSHIPSIZE.Y / 2 - GameSettings.PLAYONHONING)
                    {
                        ObjectPosition = new Vector2(ObjectPosition.X, ObjectPosition.Y + GameSettings.ALIENSHIP01UPDOWNSPEED);
                        ObjectSpeed = BaseObjectSpeed / 1.5f;
                        SpriteIndex = StartingSprite;
                    }

                    else
                    {
                        SpriteIndex = StartingSprite + 1;
                        ObjectSpeed = BaseObjectSpeed;
                    }
                }
                else
                {
                    SpriteIndex = StartingSprite + 1;
                    ObjectSpeed = BaseObjectSpeed;
                }
            }
        }
    }
}
