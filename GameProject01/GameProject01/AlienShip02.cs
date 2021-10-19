using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AlienShip02 : Aliens
    {
        // Variant Alienship that follows a Sin-wave
        private float CenterYAxis { get; set; }
        private int SpriteIndexTimer { get; set; }
        public AlienShip02(Vector2 position, Texture2D texture) : base(position, texture)
        {
            ObjectSize = GameSettings.ALIENSHIP02SIZE;
            ObjectSpeed = GameSettings.ALIENSHIP02FORWARDSPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFALIEN02SPRITES;
            ExplosionStartSprite = GameSettings.ALIENSHIP02EXPLOSIONSPRITESTART;
            CenterYAxis = position.Y;

            if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                StartingSprite = 4;
                SpriteIndex = StartingSprite;
                ObjectSpeed += 2;
            }
        }
        public override void UpdateAliens(GameTime gameTime, Vector2 shipPosition)
        {
            FollowSinWave();
            CycleSpriteIndex();
            base.UpdateAliens(gameTime, shipPosition);
        }

        private void CycleSpriteIndex()
        {
            if(IsHit == false)
            SpriteIndexTimer++;
            if (SpriteIndexTimer >= GameSettings.TIMEBETWEENALIEN02SPRITECHANGE)
            {
                SpriteIndexTimer = 0;
                SpriteIndex++;
                if (SpriteIndex >= StartingSprite + 4)
                    SpriteIndex = StartingSprite;
            }
        }

        private void FollowSinWave()
        {
            ObjectPosition = new Vector2(ObjectPosition.X, (float)(CenterYAxis +
                GameSettings.SINWAVEAMPLITUDE * Math.Sin(ObjectPosition.X * GameSettings.SINWAVEFREQUENCY)));
        }
    }
}
