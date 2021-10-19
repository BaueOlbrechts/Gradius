using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class AvatarShip : GameObject
    {
        public AvatarShip(Vector2 position, Texture2D texture) : base(position, texture)
        {
            ObjectSize = GameSettings.AVATARSHIPSIZE;
            ObjectSpeed = GameSettings.AVATARSHIPSPEED;
            NumberOfSpriteCells = GameSettings.NUMBEROFSHIPSPRITES;
        }

        public void MoveShipIfNecessary(KeyboardState keyboardState)
        {
            if(IsHit == false)
            {
                if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                {
                    if (ObjectPosition.Y >= 0)
                    {
                        ObjectPosition = new Vector2(ObjectPosition.X, ObjectPosition.Y - ObjectSpeed);
                        SpriteIndex = 2;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                {
                    if (ObjectPosition.Y + ObjectSize.Y <= GameSettings.WINDOWHEIGHT)
                    {
                        ObjectPosition = new Vector2(ObjectPosition.X, ObjectPosition.Y + ObjectSpeed);
                        SpriteIndex = 1;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                    if (ObjectPosition.X + ObjectSize.X <= GameSettings.WINDOWWIDTH)
                        ObjectPosition = new Vector2(ObjectPosition.X + ObjectSpeed, ObjectPosition.Y);


                if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                    if(ObjectPosition.X >= 0)
                        ObjectPosition = new Vector2(ObjectPosition.X - ObjectSpeed, ObjectPosition.Y);

                if (keyboardState.IsKeyUp(Keys.W) && keyboardState.IsKeyUp(Keys.S) &&
                    keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.Down))
                    SpriteIndex = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MoveShipIfNecessary(GameSettings.CurrentKeyboardState);
            if (IsHit == true)
            {
                ShipExplosion();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void ShipExplosion()
        {
            float ExplosionDurationPercentage = (float)ExplosionCount / GameSettings.EXPLOSIONDURATION;

            if (ExplosionDurationPercentage >= 0.75)
            {
                SpriteIndex = 3;
            }
            else if (0.75 > ExplosionDurationPercentage && ExplosionDurationPercentage >= 0.5)
            {
                SpriteIndex = 4;
            }
            else if (0.5 > ExplosionDurationPercentage && ExplosionDurationPercentage >= 0.25)
            {
                SpriteIndex = 5;
            }
            else if (0.25 > ExplosionDurationPercentage && ExplosionDurationPercentage >= 0)
            {
                SpriteIndex = 6;
            }

            ExplosionCount--;


            if (ExplosionCount <= 0)
            {
                IsActive = false;
                GameSettings.SetGameStateTogameOver();
            }
        }
    }
}
