using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AlienShip03 : Aliens
    {
        private int SpriteIndexTimer { get; set; }

        public AlienShip03(Vector2 position, Texture2D texture) : base(position, texture)
        {
            ObjectSize = GameSettings.ALIENSHIP03SIZE;
            ObjectSpeed = GameSettings.ALIENSHIP03FORWARDSPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFALIEN03SPRITES;
            ExplosionStartSprite = GameSettings.ALIENSHIP03EXPLOSIONSPRITESTART;

            if(GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                ObjectSpeed += 2;
            }
        }

        public override void UpdateAliens(GameTime gameTime, Vector2 shipPosition)
        {
            CycleSpriteIndex();
            base.UpdateAliens(gameTime, shipPosition);
        }

        private void CycleSpriteIndex()
        {
            if (IsHit == false)
                SpriteIndexTimer++;
            if (SpriteIndexTimer >= GameSettings.TIMEBETWEENALIEN03SPRITECHANGE)
            {
                SpriteIndexTimer = 0;
                SpriteIndex++;
                if (SpriteIndex >= StartingSprite + 3)
                    SpriteIndex = StartingSprite;
            }
        }
    }
}
