using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class GameObject
    {
        public Vector2 ObjectPosition { get; protected set; } = Vector2.Zero;
        public Vector2 ObjectSize { get; set; } = Vector2.Zero;
        public Rectangle ObjectDestinationRectangle { get; set; } = Rectangle.Empty;
        protected Rectangle ObjectSourceRectangle { get; set; } = Rectangle.Empty;
        protected float ObjectSpeed { get; set; } = 0;
        protected Texture2D ObjectTexture { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsHit { get; set; } = false;
        protected int ExplosionCount { get; set; } = GameSettings.EXPLOSIONDURATION;
        protected int SpriteIndex { get; set; } = 0;
        protected int NumberOfSpriteCells { get; set; } = 0;

        public GameObject(Vector2 position, Texture2D texture)
        {
            ObjectPosition = position;
            ObjectTexture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            GetDestinationRectanlge();
            GetSourceRectangle();
        }

        public virtual void UpdateAliens(GameTime gameTime, Vector2 shipPosition)
        {
            this.Update(gameTime);
        }

        protected void GetDestinationRectanlge()
        {
            ObjectDestinationRectangle = new Rectangle((int)ObjectPosition.X, (int)ObjectPosition.Y, (int)ObjectSize.X, (int)ObjectSize.Y);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive == true)
            {
                spriteBatch.Draw(ObjectTexture, ObjectDestinationRectangle, ObjectSourceRectangle, Color.White);
            }
        }
        private void GetSourceRectangle()
        {
            ObjectSourceRectangle = new Rectangle(ObjectTexture.Width / NumberOfSpriteCells * SpriteIndex, 0, 
                ObjectTexture.Width / NumberOfSpriteCells, ObjectTexture.Height);
        }
    }
}
