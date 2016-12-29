using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public enum AnimationState { Active, Inactive}
    public class Animation
    {
        public AnimationState AnimationState { get; set; }
        public bool DoOnce { get; set; }
        public bool AnimationDone { get; private set; }
        public Texture2D Texture { get; set; }
        private List<AnimationFrame> _frames;
        private int _timeSinceLastFrame, currentFrameIndex;
        public Animation(Texture2D texture, int textureWidth, int textureHeight, int rows, int columns, int totalframes, int duration)
        {
            DoOnce = false;
            Texture = texture;
            _frames = new List<AnimationFrame>();
            int frameWidth = textureWidth / columns;
            int frameHeight = textureHeight / rows;

            //for (int i = 0; i < totalframes; i++)
            //{
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        if((col+1)*(row+1) > totalframes)
                        {
                            return;
                        }
                        Rectangle newRectangle = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
                        AddFrame(newRectangle, duration);
                    }
              //  }
                
            }
        }
        /// <summary>
        /// Add a frame to the animation
        /// </summary>
        /// <param name="sourceRectangle">Source rectangle of the texture</param>
        /// <param name="frameDuration">Duration of the animation of the frame, in milliseconds</param>
        public void AddFrame(Rectangle sourceRectangle, int frameDuration)
        {
            AnimationFrame newFrame = new AnimationFrame(sourceRectangle,frameDuration);
            _frames.Add(newFrame);
        }
        
        public AnimationFrame GetNextFrame()
        {
            return _frames[currentFrameIndex];
        }

        public void Update(GameTime gameTime)
        {
            if(AnimationState == AnimationState.Active)
            {
                _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                if (_timeSinceLastFrame > _frames[currentFrameIndex].Duration)
                {
                    _timeSinceLastFrame -= _frames[currentFrameIndex].Duration;
                    currentFrameIndex++;
                    if (currentFrameIndex == _frames.Count)
                    {
                        currentFrameIndex = 0;
                        if (DoOnce)
                        {
                            AnimationDone = true;
                            AnimationState = AnimationState.Inactive;
                        }
                    }

                }
            }
        }

        public void Reset()
        {
            currentFrameIndex = 0;
            AnimationState = AnimationState.Active;
            AnimationDone = false;
        }

        public void End()
        {
            _timeSinceLastFrame = 0;
            currentFrameIndex = 0;
        }

        public void SetFrameDuration(int duration)
        {
            foreach (var frame in _frames)
            {
                frame.Duration = duration;
            }
        }
    }
}
