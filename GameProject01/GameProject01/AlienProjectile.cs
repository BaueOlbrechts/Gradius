using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AlienProjectile : GameObject
    {
        private Vector2 Direction { get; set; }
        public AlienProjectile(Vector2 position, Texture2D texture, Vector2 direction) : base(position, texture)
        {
            ObjectSize = GameSettings.ALIENPROJECTILESIZE;
            ObjectSpeed = GameSettings.ALIENPROJECTILESPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFALIENPROJECTILESPRITES;
            Direction = direction;
            SpriteIndex = GetSpriteIndex();
            if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
            {
                ObjectSpeed += 2;
            }
        }

        private int GetSpriteIndex()
        {
            if (Direction.X >= 0)
                return 1;
            else
                return 0;
        }

        public override void Update(GameTime gameTime)
        {
            MoveInDirection();
            base.Update(gameTime);
            DeactivateIfOffScreen();
        }

        private void DeactivateIfOffScreen()
        {
            if (Rectangle.Intersect(ObjectDestinationRectangle, new Rectangle(0, 0, GameSettings.WINDOWWIDTH, GameSettings.WINDOWHEIGHT)) 
                == Rectangle.Empty)
                IsActive = false;
        }

        private void MoveInDirection()
        {
            ObjectPosition += Direction * ObjectSpeed;
        }
    }
}
