using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;
using FlappyBerg.ScoreLogic;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Services;

namespace FlappyBerg.Screens
{
    public class LeaderboardsScreen : Screen
    {
        private Leaderboards leaderboards;
        private SpriteFont Arial14, ScoreFont;
        private Texture2D background, failedBackground;
        private bool loadingFailed;
        public LeaderboardsScreen(Game game) : base(game)
        {

        }
        
        public override void LoadContent()
        {
            loadingFailed = false;
            leaderboards = new ScoreLogic.Leaderboards();
            
            Arial14 = Load<SpriteFont>(@"Fonts\Arial14");
            ScoreFont = Load<SpriteFont>(@"Fonts\Score");
            background = LoadTexture(@"Sprites/ClassementBackground");
            failedBackground = LoadTexture(@"Sprites/ConnectionError");
            LoadFromAPI();

            base.LoadContent();
        }

        private void LoadFromAPI()
        {
            try
            {
                leaderboards.LoadTop10();
            }
            catch (Exception e)
            {
                if (e.Message == "Server")
                {
                    loadingFailed = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            if(!loadingFailed)
            {
                for (int i = 0; i < leaderboards.Scores.Count; i++)
                {
                    if (leaderboards.Scores.ElementAtOrDefault(i) != null)
                    {
                        DrawText(SpriteBatch, ScoreFont, (i + 1).ToString(), Color.Black, Color.White, 0.7f, new Vector2(103, 115 + 42 * i));
                        SpriteBatch.DrawString(Arial14, leaderboards.Scores[i].Name, new Vector2(175, 123 + 42 * i), Color.Black);
                        SpriteBatch.DrawString(Arial14, leaderboards.Scores[i].Score.ToString(), new Vector2(405, 123 + 42 * i), Color.Black);
                        SpriteBatch.DrawString(Arial14, leaderboards.Scores[i].Date, new Vector2(520, 123 + 42 * i), Color.Black);
                    }
                    else
                    {
                        DrawText(SpriteBatch, ScoreFont, "Jouez pour voir votre score apparaître ici", Color.Black, Color.White, 0.7f, new Vector2(103, 115 + 42 * i));
                    }
                }
            }
            else
            {
                SpriteBatch.Draw(failedBackground, new Rectangle(0, 0, 800, 600), Color.White);
            }
            
            SpriteBatch.End();
        }
        public override void HandleInput()
        {
            if (KeyboardService.IsKeyPressed(Keys.F5))
            {
                LoadFromAPI();
            }

            if (KeyboardService.IsKeyPressed(Keys.Escape))
            {
                ScreenManager.AddScreen<MainMenuScreen>();
                Unload();
            }
        }
        public override void UnloadContent()
        {
            SpriteBatch.Dispose();
        }
    }
}
