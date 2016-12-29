using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Sprites
{
    public class MouseSprite : AnimatedSprite
    {
        public MouseSprite(Texture2D texture, int rows, int columns)
            : base(texture, rows, columns)
        { }

        public MouseSprite(Texture2D texture, int rows, int columns, Vector2 position)
            : base(texture, rows, columns, position, Vector2.Zero)
        { }

        public MouseSprite(Texture2D texture, int rows, int columns, Vector2 position, Vector2 speed)
            : base(texture, rows, columns, position, speed)
        { }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            MouseService mouseService = ServicesHelper.GetService<MouseService>();
            Vector2 currentMousePosition = mouseService.CurrentPosition;
            Vector2 previousMousePosition = mouseService.PreviousPosition;

            if (currentMousePosition.X != previousMousePosition.X || currentMousePosition.Y != previousMousePosition.Y)
            {
                _position = currentMousePosition;
            }

            //Vérification de sortie de la fenêtre
            HandleOutOfBound(clientBounds);

            base.Update(gameTime, clientBounds);

        }
    }
}
