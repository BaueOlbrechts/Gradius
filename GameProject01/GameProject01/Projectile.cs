using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class Projectile : GameObject
    {
        public Projectile (Vector2 position, Texture2D texture) : base(position, texture)
        {
            ObjectSize = GameSettings.PROJECTILESIZE;
            ObjectSpeed = GameSettings.PROJECTILESPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFPROJECTILESPRITES;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MoveRightOnScreen();
            DeactivateIfOfScreen();
        }

        private void MoveRightOnScreen()
        {
            ObjectPosition = new Vector2(ObjectPosition.X + ObjectSpeed, ObjectPosition.Y);
        }

        private void DeactivateIfOfScreen()
        {
            if (ObjectPosition.X > GameSettings.WINDOWWIDTH)
                IsActive = false;
        }
    }
}
