using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RotatedRectangleCollisions;
using FlappyBerg.Sprites;

namespace FlappyBerg.Metiers
{
    public class SpecialWall : Wall
    {
        public enum WallType { PowerUp, PowerDown};
        private WallType _type;
        public SpecialWall(Texture2D texture, int positionX, WallType type) : base(texture, positionX)
        {
            _type = type;
        }

        public override void CheckCollision(BirdSprite bird)
        {
            //  On utilise la classe RotatedRectangle pour prendre en compte la rotation du sprite
            RotatedRectangle birdRotatedHitBox = new RotatedRectangle(bird.HitBox, bird.SpriteRotation);
            if ((birdRotatedHitBox.Intersects(_topWall.HitBox) || birdRotatedHitBox.Intersects(_bottomWall.HitBox))
                && bird.CurrentState == BirdSprite.State.Alive)
            {
                bird.Die();
            }
            //  Si le bec de l'oiseau dépasse la moitié du mur, on ajoute un point.
            if ((bird.HitBox.X + bird.HitBox.Width) > (PositionX + Width / 2) && IsPassed == false)
            {
                IsPassed = true;
                bird.AddScore();
                if(_type == WallType.PowerUp)
                {
                    bird.PowerUp();
                }
                else
                {
                    bird.PowerDown();
                }
            }
        }
    }
}
