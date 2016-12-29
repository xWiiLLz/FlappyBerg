using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class CustomAnimatedSprite : Sprite
    {
        protected readonly int _rows;
        protected readonly int _columns;

        protected int _millesecondsPerFrame = 35;
        protected int _timeSinceLastFrame = 0;
        protected int _currentFrame;

        protected readonly int _totalFrames;
        protected readonly int _frameWidth;
        protected readonly int _frameHeight;
        protected float Scale { get; set; }
        protected Animation originalAnimation, currentAnimation;

        #region Constructors
        public CustomAnimatedSprite(Texture2D texture, int rows, int columns)
            : this(texture, rows, columns, Vector2.Zero, Vector2.Zero)
        { }
        public CustomAnimatedSprite(Texture2D texture, int rows, int columns, Vector2 position)
            : this(texture, rows, columns, position, Vector2.Zero)
        { }
        public CustomAnimatedSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : this(texture, rows, columns, position, speed, rows * columns,1f)
        { }
        /// <summary>
        /// Custom Animated Sprite
        /// </summary>
        /// <param name="texture">Texture of original state</param>
        /// <param name="rows">Number of rows of original state texture</param>
        /// <param name="columns">Number of columns of original state texture</param>
        /// <param name="position">Position of the sprite</param>
        /// <param name="speed">Speed of the sprite</param>
        /// <param name="totalFrames">Total frames of the sprite</param>
        /// <param name="millisecondsPerFrame">Milliseconds to stay on each frame</param>
        public CustomAnimatedSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed,
                                int totalFrames, float scale, int millisecondsPerFrame = 35)
            : base(texture, position, speed)
        {
            originalAnimation = new Animation(texture, texture.Width,texture.Height,rows,columns,totalFrames,millisecondsPerFrame);
            currentAnimation = originalAnimation;
            _totalFrames = totalFrames;
            _rows = rows;
            _columns = columns;
            _frameWidth = texture.Width / columns;
            _frameHeight = texture.Height / rows;
            Scale = scale;
        }
        #endregion

        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            currentAnimation = originalAnimation;
            currentAnimation.Update(gameTime);

            base.Update(gameTime, clientBound);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Texture, _position, currentAnimation.GetNextFrame().SourceRectangle, Color.White,0f,Vector2.Zero,Scale,SpriteEffects.None,0f);
        }

        public override Rectangle HitBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, (int)(_frameWidth*Scale), (int)(_frameHeight*Scale)); }
        }
    }
}
