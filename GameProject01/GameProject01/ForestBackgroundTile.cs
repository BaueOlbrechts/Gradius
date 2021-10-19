using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject01
{
    class ForestBackgroundTile : BackgroundTile
    {
        public ForestBackgroundTile(Vector2 position, Texture2D texture) : base(position,texture)
        {
            ObjectSize = GameSettings.FORESTSIZE;
            ObjectSpeed = GameSettings.BACKGROUNDSPEED;
            ObjectSourceRectangle = new Rectangle(0, 0, (int)ObjectSize.X, (int)ObjectSize.Y);
        }
    }
}
