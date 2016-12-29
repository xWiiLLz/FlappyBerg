using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Sprites
{
    public class GroundTileSprite : Sprite
    {
        public GroundTileSprite(Texture2D texture, Vector2 position)
            :base(texture,position)
        {
            
        }

        public Vector2 Position { get { return _position; } set { _position = value; } }
    }
}
