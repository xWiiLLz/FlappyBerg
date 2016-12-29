using FlappyBerg.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Services;
using MonoGame.Tools.Sprites;
using RotatedRectangleCollisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Metiers
{
    public class Wall
    {
        protected WallSprite _topWall;
        protected WallSprite _bottomWall;
        public int PositionX { get; set; }
        public bool IsPassed { get; protected set; }
        private static int WALL_HOLE_HEIGHT = 216;
        public int Width { get; private set; }
        public Wall(Texture2D texture, int positionX)
        {
            Width = texture.Width;
            PositionX = positionX;
            _topWall = new WallSprite(texture, new Vector2(positionX, 0), ServicesHelper.RandomGenerator.Next(2,9), WallType.TopWall);
            _bottomWall = new WallSprite(texture, new Vector2(positionX, (_topWall.Height*texture.Height/2) + WALL_HOLE_HEIGHT), 9-_topWall.Height, WallType.BottomWall);
            IsPassed = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _topWall.Draw(spriteBatch);
            _bottomWall.Draw(spriteBatch);
        }
        public virtual void CheckCollision(BirdSprite bird)
        {
            //  On utilise la classe RotatedRectangle pour prendre en compte la rotation du sprite
            RotatedRectangle birdRotatedHitBox = new RotatedRectangle(bird.HitBox, bird.SpriteRotation);
            if((birdRotatedHitBox.Intersects(_topWall.HitBox) || birdRotatedHitBox.Intersects(_bottomWall.HitBox))
                && bird.CurrentState == BirdSprite.State.Alive)
            {
                bird.Die();
            }
            //  Si le bec de l'oiseau dépasse la moitié du mur, on ajoute un point.
            if( (bird.HitBox.X +bird.HitBox.Width) > (PositionX + Width/2) && IsPassed == false)
            {
                IsPassed = true;
                bird.AddScore();
            }
        }
        public void DrawDebuggingBoxes(SpriteBatch sp,Texture2D tex)
        {
            sp.Draw(tex, _topWall.HitBox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            sp.Draw(tex, _bottomWall.HitBox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
