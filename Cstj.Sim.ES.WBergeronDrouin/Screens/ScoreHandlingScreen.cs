using FlappyBerg.ScoreLogic;
using FlappyBerg.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Screens
{
    public class ScoreHandlingScreen : Screen
    {
        private Texture2D background, scoreHandlingBackground, scoreAlreadyPublishedBackground, scoreHandlingSuccess;
        private string _name;
        private string _message;
        private SpriteFont arial14;
        private bool _scoreAdded;
        public static int LastScorePublished { get; private set; }
        private Vector2 textPosition, messagePosition;
        private const int MAX_CHARACTERS = 25;
        private bool _canPublishScore, _scorePublished;
        public ScoreHandlingScreen(Game game) : base(game)
        {
            _name = "";
            _scorePublished = false;
            if (GameScreen.HighScore > LastScorePublished)
            {
                _canPublishScore = true;
            }
            else
            {
                _canPublishScore = false;
            }
        }

        public override void LoadContent()
        {
            scoreHandlingBackground = LoadTexture(@"Sprites/ScoreHandlingBackground");
            scoreAlreadyPublishedBackground = LoadTexture(@"Sprites/ScoreAlreadyPublished");
            scoreHandlingSuccess = LoadTexture(@"Sprites/ScoreHandlingSuccess");
            if (_canPublishScore)
            {
                background = scoreHandlingBackground;
            }
            else
            {
                background = scoreAlreadyPublishedBackground;
            }

            arial14 = Load<SpriteFont>(@"Fonts/Arial14");
            textPosition = new Vector2(245, 190);
            messagePosition = new Vector2(400, 300);
        }

        public override void HandleInput()
        {
            if(GameScreen.HighScore > LastScorePublished)
            {
                Keys[] keyPressed = KeyboardService.CurrentState.GetPressedKeys();
                foreach (var key in keyPressed)
                {

                    if (KeyboardService.PreviousState.IsKeyUp(key))
                    {
                        if (key == Keys.Back && _name.Length > 0)
                        {
                            //  Si on appuie sur back, on retire la dernière lettre
                            _name = _name.Remove(_name.Length - 1, 1);
                        }
                        else if (key == Keys.Space && _name.Length < MAX_CHARACTERS)
                        {
                            _name = _name.Insert(_name.Length, " ");
                        }
                        else if (key >= Keys.A && key <= Keys.Z && _name.Length < MAX_CHARACTERS)
                        {
                            if (KeyboardService.IsKeyDown(Keys.LeftShift))
                            {
                                _name += key.ToString().ToUpper();
                            }
                            else
                            {
                                _name += key.ToString().ToLower();
                            }
                        }

                        if (key == Keys.Enter)
                        {
                            if (_name.Length > 2 && _name.Length <= 25)
                            {
                                try
                                {
                                    _scoreAdded = Leaderboards.AddScoreEntry(new ScoreEntry(_name, GameScreen.HighScore, ""));
                                    if (_scoreAdded)
                                    {
                                        _message = String.Format("Votre score de {0} a bien été envoyé {1} !", GameScreen.HighScore, _name);
                                        LastScorePublished = GameScreen.HighScore;
                                        _canPublishScore = false;
                                        _scorePublished = true;
                                        background = scoreHandlingSuccess;
                                    }
                                }
                                catch (Exception e)
                                {
                                    _message = "Erreur lors de l'envoi. Veuillez vérifier votre connexion internet.";
                                }


                            }
                            else
                            {
                                _message = "Veuillez entrer un nom de 3 à 25 caractères.";
                            }
                        }
                    }

                }
            }
            if (KeyboardService.IsKeyPressed(Keys.Escape))
            {
                ScreenManager.AddScreen<MainMenuScreen>();
                Unload();
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            if(_canPublishScore || _scorePublished)
            {
                if(_canPublishScore)
                {
                    SpriteBatch.DrawString(arial14, _name, textPosition, Color.White);
                }
                if (!String.IsNullOrEmpty(_message))
                {
                    
                    messagePosition.X = 400 - arial14.MeasureString(_message).X / 2;
                    SpriteBatch.DrawString(arial14, _message, messagePosition, Color.White);
                }
            }
            SpriteBatch.End();
        }
    }
}
