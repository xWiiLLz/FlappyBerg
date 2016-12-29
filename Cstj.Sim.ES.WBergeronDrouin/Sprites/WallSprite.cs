using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Sprites;
using RotatedRectangleCollisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Sprites
{
    public enum WallType { TopWall, BottomWall}
    public class WallSprite : Sprite
    {
        public int Height { get; set; }
        private WallType _orientation;
        
        public WallSprite(Texture2D texture, Vector2 position, int height, WallType orientation)
            :base(texture,position)
        {
            Height = height;
            _orientation = orientation;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle wallTile = new Rectangle(0, 0, _texture.Width, _texture.Height / 2);
            Rectangle wallEndTile = new Rectangle(0, _texture.Height/2, _texture.Width, _texture.Height / 2);
            Rectangle tileType = wallTile;
            SpriteEffects spriteEffect = SpriteEffects.None;
            Vector2 tilePosition = new Vector2(_position.X, _position.Y);
            for (int i = 0; i < Height; i++)
            {
                tileType = wallTile;
                spriteEffect = SpriteEffects.None;
                tilePosition.Y = _position.Y + _texture.Height / 2 * i;
                if (i==0 && _orientation == WallType.BottomWall)
                {
                    tileType = wallEndTile;
                    spriteEffect = SpriteEffects.FlipVertically;
                }
                if (i == Height - 1 && _orientation == WallType.TopWall)
                {
                    tileType = wallEndTile;
                }
                spriteBatch.Draw(_texture, tilePosition, tileType, Color.White, 0f, Vector2.Zero, 1f,spriteEffect, 0.5f);
            }

        }
        public override Rectangle HitBox
        {
            get
            {
                //return new Rectangle((int)_position.X,(int)_position.Y,_texture.Width,_texture.Height/2*Height);
                return new Rectangle((int)_position.X,(int)_position.Y,_texture.Width,_texture.Height/2*Height);
            }
        }
    }
}
