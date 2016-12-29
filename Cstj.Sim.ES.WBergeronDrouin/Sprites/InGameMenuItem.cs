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
    public class InGameMenuItem
    {
        private Texture2D _texture;
        private Vector2 _position;
        public enum MenuItemState { Active, Inactive};
        public MenuItemState State { get; private set; }

        public InGameMenuItem(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            State = MenuItemState.Inactive;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle;
            if(State == MenuItemState.Active)
            {
                sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height / 2);
            }
            else
            {
                sourceRectangle = new Rectangle(0, _texture.Height / 2, _texture.Width, _texture.Height / 2);
            }
            spriteBatch.Draw(_texture, _position, sourceRectangle, Color.White);
        }

        public void Toggle()
        {
            if (State == MenuItemState.Active)
            {
                State = MenuItemState.Inactive;
            }
            else
            {
                State = MenuItemState.Active;
            }
        }
    }
}
