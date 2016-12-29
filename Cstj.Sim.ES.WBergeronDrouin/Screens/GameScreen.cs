using FlappyBerg.Metiers;
using FlappyBerg.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Tools.Components;
using MonoGame.Tools.Screens;
using MonoGame.Tools.Services;
using MonoGame.Tools.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBerg.Screens
{
    public class GameScreen : Screen
    {
        #region Attributes

        private Camera camera;
        private BirdSprite bird;
        private List<Wall> walls;
        private List<GroundTileSprite> groundTiles;
        private string debugString;
        private const int DISTANCE_BETWEEN_WALLS = 300;
        private static Point CameraOffset { get; set; }
        private SpriteBatch staticSpriteBatch;

        private Texture2D texWall, texSpecialWallUp, texSpecialWallDown;
        private Texture2D background;
        private Texture2D leavingPrompt;
        private Texture2D scorePrompt;
        private Texture2D currentPrompt;
        private Texture2D texGroundTile;
        private Texture2D rec;
        private Texture2D texOui, texNon;
        private SpriteFont Arial8, ScoreFont, ScoreSmallFont;
        private Color titleRed;
        private Color titleLightBlue;

        private InGameMenuItem ouiMenuItem, nonMenuItem;
        #endregion

        #region Properties

        public static int HighScore { get; set; }
        #endregion
        public GameScreen(Game game)
            : base(game)
        {
            CameraOffset = new Point((int)(Game.GraphicsDevice.Viewport.Width * 0.5f), (int)(Game.GraphicsDevice.Viewport.Height * 0.5f - 200));

            camera = new Camera(Game.GraphicsDevice.Viewport, Vector2.Zero);
            camera.FocusPoint = new Point((int)CameraOffset.X, (int)CameraOffset.Y);
            staticSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void LoadContent()
        {
            //  Colors
            titleLightBlue = new Color(113, 222, 247);
            titleRed = new Color(235, 72, 72);

            walls = new List<Wall>();
            Texture2D texBird = LoadTexture(@"Sprites/BirdSpriteSheet");
            background = LoadTexture(@"Sprites/Background");

            //  Bird loading
            bird = new BirdSprite(texBird, 1, 4, new Vector2(0, 200), new Vector2(3, -5), 1, 1, 4, 4, 0.75f, 60);
            bird.Jumping = Load<SoundEffect>(@"Sounds/Jump2");
            bird.ScoreSound = Load<SoundEffect>(@"Sounds/Score");
            bird.DiyingSoundEffect = Load<SoundEffect>(@"Sounds/Death");
            bird.PunchSound = Load<SoundEffect>(@"Sounds/Punch");
            bird.PowerUpSound = Load<SoundEffect>(@"Sounds/PowerUp");
            bird.PowerDownSound = Load<SoundEffect>(@"Sounds/PowerDown");

            //  Pipe textures
            texWall = LoadTexture(@"Sprites/Pipe");
            texSpecialWallUp = LoadTexture(@"Sprites/Pipe_power-up");
            texSpecialWallDown = LoadTexture(@"Sprites/Pipe_power-down");


            //  In-Game menu
            leavingPrompt = LoadTexture(@"Sprites/LeaveGamePrompt");
            scorePrompt = LoadTexture(@"Sprites/PublishScorePrompt");
            currentPrompt = leavingPrompt;
            texOui = LoadTexture(@"Sprites/OuiSpriteSheet");
            texNon = LoadTexture(@"Sprites/NonSpriteSheet");
            ouiMenuItem = new InGameMenuItem(texOui, new Vector2(200, 300));
            nonMenuItem = new InGameMenuItem(texNon, new Vector2(550, 300));
            //  Par défault, le bouton "non" est sélectionné pour éviter les erreurs.
            nonMenuItem.Toggle();




            //  Font loading
            Arial8 = Load<SpriteFont>(@"Fonts\Arial8");
            ScoreFont = Load<SpriteFont>(@"Fonts\Score");
            ScoreSmallFont = Load<SpriteFont>(@"Fonts\ScoreSmall");

            //  Spawn des 4 premiers murs. Ils ne peuvent pas être spéciaux.
            for (int i = 1; i <= 4; i++)
            {
                walls.Add(new Wall(texWall, DISTANCE_BETWEEN_WALLS * i));
            }
            rec = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rec.SetData<Color>(new Color[] { Color.White });


            // Scrolling background
            groundTiles = new List<GroundTileSprite>();
            texGroundTile = LoadTexture(@"Sprites/Ground-tile");
            for (int i = 0; i < 4; i++)
            {
                groundTiles.Add(new GroundTileSprite(texGroundTile, new Vector2(texGroundTile.Width * i - 200, 650 - texGroundTile.Height)));
            }
            base.LoadContent();
        }
        public override void HandleInput()
        {
            if (State == ScreenState.Active)
            {
                if (KeyboardService.IsKeyDown(Keys.Space) && bird.CurrentState == BirdSprite.State.Dead)
                {
                    ScreenManager.AddScreen<GameScreen>();
                    Unload();
                }

                if (KeyboardService.IsKeyPressed(Keys.Escape))
                {
                    if (State == ScreenState.Active)
                    {
                        State = ScreenState.Pause;
                    }
                    else
                    {
                        State = ScreenState.Active;
                    }
                    
                }
            }
            else if(State == ScreenState.Pause)
            {

                if (KeyboardService.IsKeyPressed(Keys.Left) || KeyboardService.IsKeyPressed(Keys.Right))
                {
                    ouiMenuItem.Toggle();
                    nonMenuItem.Toggle();
                }
                if(KeyboardService.IsKeyPressed(Keys.Space) || KeyboardService.IsKeyPressed(Keys.Enter))
                {
                    if (currentPrompt == leavingPrompt)
                    {
                        //  Si l'utilisateur choisi de quitter le jeu, on le quitte
                        if (ouiMenuItem.State == InGameMenuItem.MenuItemState.Active)
                        {
                            //  On vérifie si le score est plus haut que 0
                            if (HighScore > 0)
                            {
                                currentPrompt = scorePrompt;
                                //  Le bouton reste "oui" par défaut, pour éviter d'annuler la sauvegarde de score par erreur
                            }
                            else
                            {
                                //  Sinon, on retourne directement au menu
                                ScreenManager.AddScreen<MainMenuScreen>();
                                Unload();
                            }
                        }
                        else    //  Sinon, on retourne en jeu.
                        {
                            this.State = ScreenState.Active;
                        }
                    }
                    else
                    {
                        if (ouiMenuItem.State == InGameMenuItem.MenuItemState.Active)
                        {
                            ScreenManager.AddScreen<ScoreHandlingScreen>();
                            Unload();
                        }
                        else
                        {
                            //  Sinon, on retourne directement au menu
                            ScreenManager.AddScreen<MainMenuScreen>();
                            Unload();
                        }
                    }
                    
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == ScreenState.Active)
            {

                if (bird.CurrentState == BirdSprite.State.Dying || bird.CurrentState == BirdSprite.State.Dead)
                {
                    if (HighScore < bird.Score)
                    {
                        HighScore = bird.Score;
                    }
                }
                if (bird.CurrentState != BirdSprite.State.Dead)
                {

                    bird.Update(gameTime, new Rectangle((int)(camera.Position.X), (int)(camera.Position.Y), (int)(camera.View.Bounds.Width + camera.Position.X),
                        (int)(camera.View.Bounds.Height + camera.Position.Y)));
                    camera.Position = new Vector2(bird.HitBox.X + 200, 150);

                    foreach (var wall in walls.ToList())
                    {
                        //  Si le mur est déjà passé et qu'on ne le voit plus, on le supprime.
                        if (wall.IsPassed && (wall.PositionX + wall.Width + 250) < camera.Position.X - 200)
                        {
                            walls.Remove(wall);
                        }
                        wall.CheckCollision(bird);
                    }
                    // Spawn de wall lorsqu'il y en a moins de 10 d'avances.
                    if (walls.Count < 10)
                    {
                        SpawnWall();
                    }

                    //  Scrolling ground
                    GroundTileSprite firstTile = groundTiles.First();
                    if (firstTile.HitBox.X + firstTile.HitBox.Width < camera.Position.X - 450)
                    {
                        groundTiles.Remove(firstTile);
                        groundTiles.Add(new GroundTileSprite(texGroundTile, new Vector2(groundTiles.Last().HitBox.X + groundTiles.Last().HitBox.Width, groundTiles.Last().HitBox.Y)));
                    }
                }
            }
        }

        private void SpawnWall()
        {
            int specialSpawnChance = ServicesHelper.RandomGenerator.Next(0, 15);
            if (specialSpawnChance == 10)
            {
                walls.Add(new SpecialWall(texSpecialWallUp, walls.Last().PositionX + DISTANCE_BETWEEN_WALLS, SpecialWall.WallType.PowerUp));
            }
            else if (specialSpawnChance == 11)
            {
                walls.Add(new SpecialWall(texSpecialWallDown, walls.Last().PositionX + DISTANCE_BETWEEN_WALLS, SpecialWall.WallType.PowerDown));
            }
            else
            {
                walls.Add(new Wall(texWall, walls.Last().PositionX + DISTANCE_BETWEEN_WALLS));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle fullScreen = new Rectangle(0, 0, 800, 600);
            #region Static Drawing
            staticSpriteBatch.Begin();
            staticSpriteBatch.Draw(background, fullScreen, Color.White);
            staticSpriteBatch.End();
            #endregion

            #region Camera drawing
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            //  On dessine le ground
            foreach (GroundTileSprite tile in groundTiles)
            {
                tile.Draw(SpriteBatch);
            }

            //  On dessine les murs (pipes)
            foreach (var wall in walls)
            {
                //  Si on est en déboguage, on affiche les carrés de hitbox pour les murs
                if (ScreenManager.IsDebugging)
                {
                    wall.DrawDebuggingBoxes(SpriteBatch, rec);
                }
                wall.Draw(SpriteBatch);
            }


            //  Si on est en déboguage, on affiche le carré de hitbox pour l'oiseau
            if (ScreenManager.IsDebugging)
            {
                SpriteBatch.Draw(rec, new Rectangle(bird.HitBox.X, bird.HitBox.Y, bird.HitBox.Width, bird.HitBox.Height), null,
                    Color.Red, bird.SpriteRotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
            bird.Draw(SpriteBatch);
            SpriteBatch.End();
            #endregion

            #region Second Static Drawing
            staticSpriteBatch.Begin();
            DrawText(staticSpriteBatch, ScoreFont, bird.Score.ToString(), Color.Black, Color.White, 1f,
                     new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2));
            //High score top left corner
            DrawText(staticSpriteBatch, ScoreSmallFont, String.Format("High Score: {0}", HighScore), Color.Black, titleRed, 1f,
                    Vector2.Zero);

            //  In-Game menu si le jeu est en pause
            if (this.State == ScreenState.Pause)
            {
                staticSpriteBatch.Draw(currentPrompt, fullScreen, Color.White);
                ouiMenuItem.Draw(staticSpriteBatch);
                nonMenuItem.Draw(staticSpriteBatch);
            }
            staticSpriteBatch.End();

            #endregion
        }

        public override void UnloadContent()
        {
            SpriteBatch.Dispose();
            staticSpriteBatch.Dispose();
        }
    }
}
