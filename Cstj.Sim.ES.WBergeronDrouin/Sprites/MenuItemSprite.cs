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
    public class MenuItemSprite : CustomAnimatedSprite
    {
        private Animation selectedAnimation;
        public string Name { get; private set; }
        public bool Active { get; set; }
        public MenuItemSprite(string name, Texture2D texture, int rows, int columns, int animationFrames, int staticFrames, Vector2 position)
            : base(texture,rows,columns,position,Vector2.Zero,staticFrames,1)
        {
            Name = name;
            originalAnimation.AnimationState = AnimationState.Inactive;
            selectedAnimation = new Animation(texture, texture.Width, texture.Height, rows, columns, animationFrames, 125);
            selectedAnimation.AnimationState = AnimationState.Active;
        }

        public void Update(GameTime gameTime)
        {
            this.Update(gameTime,Rectangle.Empty);
        }
        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            currentAnimation = selectedAnimation;
            currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            currentAnimation = originalAnimation;
        }
    }
}
