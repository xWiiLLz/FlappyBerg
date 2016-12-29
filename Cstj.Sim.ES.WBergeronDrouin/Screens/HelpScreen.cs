using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Screens
{
    public class HelpScreen : Screen
    {
        private Texture2D background;
        public HelpScreen(Game game) : base(game)
        {

        }
        public override void HandleInput()
        {
            if(KeyboardService.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                ScreenManager.AddScreen<MainMenuScreen>();
                Unload();
            }
        }
        public override void LoadContent()
        {
            background = LoadTexture(@"Sprites/HelpScreenBackground");
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            SpriteBatch.End();
        }
    }
}
