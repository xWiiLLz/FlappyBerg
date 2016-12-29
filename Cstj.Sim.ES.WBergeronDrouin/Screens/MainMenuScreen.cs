using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FlappyBerg.Sprites;

namespace FlappyBerg.Screens
{
    public class MainMenuScreen : Screen
    {
        private Texture2D background;
        private List<MenuItemSprite> menuItems;
        private int selectedMenuItemIndex;
        public MainMenuScreen(Game game) : base(game)
        {
        }
        public override void LoadContent()
        {
            menuItems = new List<MenuItemSprite>();
            background = LoadTexture(@"Sprites/MainMenuBackground");
            Texture2D texMenuJouer = LoadTexture(@"Sprites/JouerSpriteSheet");
            Texture2D texMenuClassement = LoadTexture(@"Sprites/ClassementSpriteSheet");
            Texture2D texMenuAide = LoadTexture(@"Sprites/AideSpriteSheet");
            menuItems.Add(new MenuItemSprite("Jouer",texMenuJouer, 7, 1, 7, 1, 
                         new Vector2(Game.GraphicsDevice.Viewport.Width/2-texMenuJouer.Width/2, 200)));

            menuItems.Add(new MenuItemSprite("Classement", texMenuClassement, 7, 1, 7, 1,
                         new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - texMenuClassement.Width / 2, menuItems[0].HitBox.Y+menuItems[0].HitBox.Height)));

            menuItems.Add(new MenuItemSprite("Aide", texMenuAide, 7, 1, 7, 1,
                         new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - texMenuAide.Width / 2, menuItems[1].HitBox.Y+menuItems[1].HitBox.Height)));
            selectedMenuItemIndex = 0;
            base.LoadContent();
        }

        public override void HandleInput()
        {
            // Navigation menu
            if(KeyboardService.IsKeyPressed(Keys.Up))
            {
                selectedMenuItemIndex--;
                if(selectedMenuItemIndex<0)
                {
                    selectedMenuItemIndex = menuItems.Count-1;
                }
            }

            if(KeyboardService.IsKeyPressed(Keys.Down))
            {
                selectedMenuItemIndex++;
                if (selectedMenuItemIndex > menuItems.Count-1)
                {
                    selectedMenuItemIndex = 0;
                }
            }



            if (KeyboardService.IsKeyPressed(Keys.Space) || KeyboardService.IsKeyPressed(Keys.Enter))
            {
                switch(selectedMenuItemIndex)
                {
                    case 0:
                        {
                            ScreenManager.AddScreen<GameScreen>();
                            break;
                        }
                    case 1:
                        {
                            ScreenManager.AddScreen<LeaderboardsScreen>();
                            break;
                        }
                    case 2:
                        {
                            ScreenManager.AddScreen<HelpScreen>();
                            break;
                        }
                }
                Unload();
            }



            base.HandleInput();
            //  Si on appuie sur escape, le jeu se ferme.
            if (KeyboardService.IsKeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
        }

        public override void Update(GameTime gameTime)
        {
            menuItems[selectedMenuItemIndex].Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            foreach (var item in menuItems)
            {
                item.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }
        public override void UnloadContent()
        {
            SpriteBatch.Dispose();
        }
    }
}
