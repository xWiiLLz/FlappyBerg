using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Services;
using MonoGame.Tools.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Sprites
{
    public class BirdSprite : CustomAnimatedSprite
    {
        public enum State { Alive, Dying, Dead };
        #region Private Attributes

        private Animation _flyingAnimation;
        private SpriteEffects spriteEffect;
        private float _ACCELERATION = 0.30f;
        public float SpriteRotation { get; set; }
        private Vector2 _originalSpeed;
        #endregion

        #region Public Properties
        public int Score { get; set; }
        public SoundEffect Jumping { get; set; }
        public SoundEffect DiyingSoundEffect { get; set; }
        public SoundEffect ScoreSound { get; set; }
        public SoundEffect PowerUpSound { get; set; }
        public SoundEffect PowerDownSound { get; set; }
        public SoundEffect PunchSound { get; set; }
        public State CurrentState { get; set; }
        #endregion

        #region Constructors


        public BirdSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed, int totalFrame,
             int flyingTextureRows, int flyingTextureColumns, int flyingTotalFrames, float scale, int millisecondsPerFrame = 35)
            : base(texture, rows, columns, position, speed, totalFrame, scale, millisecondsPerFrame)
        {
            _speed = speed;
            _originalSpeed = speed;
            _flyingAnimation = new Animation(texture, texture.Width, texture.Height, flyingTextureRows, flyingTextureColumns, flyingTotalFrames, millisecondsPerFrame);
            _flyingAnimation.DoOnce = true;
            spriteEffect = SpriteEffects.None;
            Score = 0;
            CurrentState = State.Alive;
            SpriteRotation = 0.1f;
        }
        #endregion

        /// <summary>
        /// Sets the jumping animation sprite
        /// </summary>
        /// <param name="texture">Texture of the jumping sprite</param>
        /// <param name="rows">Number of rows in the given texture</param>
        /// <param name="columns">Number of columns in the given texture</param>
        /// <param name="totalFrames">Number of total frames</param>
        /// <param name="millisecondsPerFrame">Time of animation for each frame</param>


        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            KeyboardService keyboardService = ServicesHelper.GetService<KeyboardService>();

            //  Lower bound
            if (_position.Y + HitBox.Height > clientBound.Height - clientBound.Y)
            {
                if (CurrentState != State.Dying)
                {
                    PlaySoundEffect(PunchSound);
                }
                CurrentState = State.Dead;
                _position.Y = clientBound.Height - 100 - _frameHeight;
            }

            //  Higher bound
            if (_position.Y < clientBound.Y - 100)
            {
                Die();

            }

            if (CurrentState != State.Dead)
            {
                if (currentAnimation.AnimationDone)
                {
                    currentAnimation = originalAnimation;
                }


                if (keyboardService.IsKeyPressed(Keys.Space) && CurrentState == State.Alive)
                {
                    currentAnimation = _flyingAnimation;
                    _flyingAnimation.Reset();
                    _speed.Y = -8f;
                    PlaySoundEffect(Jumping);
                    SpriteRotation = 0.1f;
                }

                _speed.Y = _speed.Y + _ACCELERATION;
                _position += _speed;
                SpriteRotation += SpriteRotation * 0.03f;
                if (SpriteRotation > Math.PI / 2)
                {
                    SpriteRotation = (float)Math.PI / 2;
                }

                currentAnimation.Update(gameTime);
            }


        }


        #region SoundEffects



        /// <summary>
        /// Plays a given sound effect, if it's set
        /// </summary>
        private void PlaySoundEffect(SoundEffect effect)
        {
            if (effect != null)
            {
                effect.Play();
            }
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle position = new Rectangle((int)_position.X, (int)_position.Y, currentAnimation.GetNextFrame().SourceRectangle.Width, currentAnimation.GetNextFrame().SourceRectangle.Height);
            Rectangle source = currentAnimation.GetNextFrame().SourceRectangle;
            Texture2D texture = currentAnimation.Texture;
            spriteBatch.Draw(texture, _position, source, Color.White, SpriteRotation, Vector2.Zero, Scale, spriteEffect, 0f);
        }

        public void Die()
        {
            _speed.X = 0;
            if (_speed.Y < 0)
            {
                _speed.Y = 0;
            }
            CurrentState = State.Dying;
            PlaySoundEffect(DiyingSoundEffect);

        }

        public void AddScore()
        {
            Score += 1;
            PlaySoundEffect(ScoreSound);
        }

        #region Speed Modifiers
        public void PowerUp()
        {
            if (_ACCELERATION < 0.40f)
            {
                _ACCELERATION += .05f;
                _speed.X++;
            }
            PlaySoundEffect(PowerUpSound);
        }
        public void PowerDown()
        {
            // Accélération ne peut pas descendre en bas de 0.30f
            if (_ACCELERATION > 0.30f)
            {
                _ACCELERATION -= .05f;
            }
            //  On ne peut pas avoir plus que 2 power downs... 
            if (_speed.X > _originalSpeed.X - 2)
            {
                _speed.X--;
            }
            PlaySoundEffect(PowerDownSound);
        }
        #endregion
    }
}
