using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class BackgroundTile : GameObject
    {
        public int TileType { get; set; }
        public bool IsFlipped { get; set; }
        public BackgroundTile(Vector2 position, Texture2D texture, int tileType, bool isFlipped) : base(position, texture)
        {
            TileType = tileType;
            IsFlipped = isFlipped;
            ObjectSourceRectangle = GetTileTypeSourceRectangle();
            ObjectSize = new Vector2(ObjectSourceRectangle.Width, ObjectSourceRectangle.Height);
            ObjectSpeed = GameSettings.BACKGROUNDSPEED;
        }

        protected BackgroundTile(Vector2 position, Texture2D texture) : base(position,texture)
        {
        }

        private Rectangle GetTileTypeSourceRectangle()
        {
            Rectangle sourceRectangle;
            if (TileType == 1)
                sourceRectangle = GameSettings.BACKGROUND01SPRITERECTANLGE;
            else if (TileType == 2)
                sourceRectangle = GameSettings.BACKGROUND02SPRITERECTANLGE;
            else if (TileType == 3)
                sourceRectangle = GameSettings.BACKGROUND03SPRITERECTANLGE;
            else if (TileType == 4)
                sourceRectangle = GameSettings.BACKGROUND04SPRITERECTANLGE;
            else sourceRectangle = Rectangle.Empty;

            if (IsFlipped == true)
                sourceRectangle = new Rectangle(sourceRectangle.X, sourceRectangle.Height, sourceRectangle.Width, sourceRectangle.Height);

            return sourceRectangle;
        }

        public override void Update(GameTime gameTime)
        {
            MoveBackgroundLeft();
            GetDestinationRectanlge();
            DeactivateIfOffScreen();
        }

        private void MoveBackgroundLeft()
        {
            ObjectPosition = new Vector2(ObjectPosition.X - ObjectSpeed, ObjectPosition.Y);
        }

        private void DeactivateIfOffScreen()
        {
            if (ObjectPosition.X + ObjectSize.X <= 0)
                IsActive = false;
        }
    }
}
