using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GameProject01
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect _projectileSFX, _alienExplosionSFX, _shipExplosionSFX, _gameStartSFX;
        Song _song1, _song2, _currentSong;
        Score _score;
        SpriteFont _font;
        AvatarShip _avatarShip;
        Texture2D _avatarTexture, _alienShip01Texture, _alienShip02Texture, _alienShip03Texture, _alienTurretTexture,
            _alienProjectileTexture, _projectileTexture, _startScreenTexture, _endScreenTexture, _backgroundTexture, _forestTexture;
        List<Aliens> _aliens = new List<Aliens>();
        List<Projectile> _projectiles = new List<Projectile>();
        List<AlienProjectile> _alienProjectiles = new List<AlienProjectile>();
        List<BackgroundTile> _backgroundTiles = new List<BackgroundTile>();
        List<ForestBackgroundTile> _forestTiles = new List<ForestBackgroundTile>();
        int _waveTimer = GameSettings.TIMEBETWEENWAVES;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GameSettings.WINDOWWIDTH;
            graphics.PreferredBackBufferHeight = GameSettings.WINDOWHEIGHT;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            MediaPlayer.IsRepeating = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Graphics load
            _avatarTexture = Content.Load<Texture2D>("Graphics/AvatarShip");
            _alienShip01Texture = Content.Load<Texture2D>("Graphics/AlienShip1Texture");
            _alienShip02Texture = Content.Load<Texture2D>("Graphics/AlienShip2Texture");
            _alienShip03Texture = Content.Load<Texture2D>("Graphics/AlienShip3Texture");
            _alienTurretTexture = Content.Load<Texture2D>("Graphics/AlienTurretTexture");
            _alienProjectileTexture = Content.Load<Texture2D>("Graphics/AlienProjectileTexture");
            _projectileTexture = Content.Load<Texture2D>("Graphics/ProjectileTexture");
            _startScreenTexture = Content.Load<Texture2D>("Graphics/StartScreen");
            _endScreenTexture = Content.Load<Texture2D>("Graphics/EndScreen");
            _backgroundTexture = Content.Load<Texture2D>("Graphics/BackGroundTexture");
            _forestTexture = Content.Load<Texture2D>("Graphics/ForestTexture");
            _font = Content.Load<SpriteFont>("ScoreFont");

            //Sounds load
            _projectileSFX = Content.Load<SoundEffect>("Sounds/ProjectileShot");
            _alienExplosionSFX = Content.Load<SoundEffect>("Sounds/AlienExplosion");
            _shipExplosionSFX = Content.Load<SoundEffect>("Sounds/ShipExplosion");
            _gameStartSFX = Content.Load<SoundEffect>("Sounds/StartSound");
            _song1 = Content.Load<Song>("Sounds/Stage1");
            _song2 = Content.Load<Song>("Sounds/Stage2");

            _currentSong = _song1;
            _score = new Score(_font);
            _score.GetHighScoreFromTxt();
        }

        private void CreateNewShip()
        {
            _avatarShip = new AvatarShip(new Vector2(graphics.PreferredBackBufferWidth / 2 - GameSettings.AVATARSHIPSIZE.X / 2,
                            graphics.PreferredBackBufferHeight / 2 - GameSettings.AVATARSHIPSIZE.Y / 2), _avatarTexture);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            GameSettings.SetCurrentStates();

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.Playing)
            {
                // Begin update setup events
                SpawnNewWaveIfNecessary();

                // Update object and events
                UpdateBackgroundTiles(gameTime);
                _avatarShip.Update(gameTime);
                UpdateProjectiles(gameTime);
                UpdateAliens(gameTime);
                CreateNewBackgroundTileIfNecessary();
                TurretFireProjectileIfNecessary();
                FireProjectileIfNecessary();
                CheckProjectileCollision();
                CheckShipCollision();

                // Finish update events
                RemoveInactiveObjects();
                _score.UpdateScores();
            }

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.ControlScreen)
            {
                SetDifficulty();
                PressSpaceToPlay();
            }

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.GameOver)
            {
                RestartGameIfNecessary();
            }

            GameSettings.SetPreviousStates();
            base.Update(gameTime);
        }

        private void TurretFireProjectileIfNecessary()
        {

            foreach(Aliens currentAlien in _aliens)
            {
                if(currentAlien is AlienTurret)
                {
                    AlienTurret currentAlienTurret = (AlienTurret)currentAlien;
                    if (currentAlienTurret.ProjectileFireTimer <= 0 && currentAlienTurret.IsHit == false)
                    {
                        currentAlienTurret.ProjectileFireTimer = GameSettings.PROJECTILEFIRETIMER;
                        if (GameSettings.CURRENTDIFFICULTY == GameSettings.Difficulty.Hard)
                        {
                            currentAlienTurret.ProjectileFireTimer = GameSettings.PROJECTILEFIRETIMER / 2;
                        }
                        AlienProjectile newAlienProjectile = new AlienProjectile(currentAlienTurret.ObjectPosition + currentAlienTurret.ObjectSize / 2
                            - GameSettings.ALIENPROJECTILESIZE / 2, _alienProjectileTexture, currentAlienTurret.PointToShip);
                        _alienProjectiles.Add(newAlienProjectile);
                    }
                }
            }
        }

        private void SetDifficulty()
        {
            if (GameSettings.CurrentKeyboardState.IsKeyDown(Keys.D1) && GameSettings.PreviousKeyboardState.IsKeyUp(Keys.D1))
            {
                _currentSong = _song1;
                GameSettings.CURRENTDIFFICULTY = GameSettings.Difficulty.Normal;
                MediaPlayer.Play(_currentSong);
            }

            if (GameSettings.CurrentKeyboardState.IsKeyDown(Keys.D2) && GameSettings.PreviousKeyboardState.IsKeyUp(Keys.D2))
            {
                _currentSong = _song2;
                GameSettings.CURRENTDIFFICULTY = GameSettings.Difficulty.Hard;
                MediaPlayer.Play(_currentSong);
            }
        }

        private void CreateNewBackgroundTileIfNecessary()
        {
            for (int i = _backgroundTiles.Count; i > 0; i--)
            {
                BackgroundTile currentBackgroundTile = _backgroundTiles[i - 1];

                if (currentBackgroundTile.ObjectPosition.X <= GameSettings.WINDOWWIDTH + GameSettings.BACKGROUNDSPEED &&
                    currentBackgroundTile.ObjectPosition.X > GameSettings.WINDOWWIDTH)
                {
                    BackgroundTile newBackgroundTile = new BackgroundTile(new Vector2(currentBackgroundTile.ObjectPosition.X +
                        currentBackgroundTile.ObjectDestinationRectangle.Width, currentBackgroundTile.ObjectPosition.Y),
                        _backgroundTexture, GetRandomTileType(), currentBackgroundTile.IsFlipped);
                    _backgroundTiles.Add(newBackgroundTile);
                    AddPossibleAlienTurret(newBackgroundTile);
                }
            }
            for (int i = _forestTiles.Count; i > 0; i--)
            {
                ForestBackgroundTile currentForestBackgroundTile = _forestTiles[i - 1];
                if (currentForestBackgroundTile.ObjectPosition.X <= GameSettings.WINDOWWIDTH + GameSettings.BACKGROUNDSPEED &&
                    currentForestBackgroundTile.ObjectPosition.X > GameSettings.WINDOWWIDTH)
                {
                    float randomDouble = (float)GameSettings.Random.NextDouble();
                    if(randomDouble <= 0.70)
                    {
                        ForestBackgroundTile newForestBackgroundTile = new ForestBackgroundTile(new Vector2(currentForestBackgroundTile.ObjectPosition.X +
                        GameSettings.FORESTSIZE.X, currentForestBackgroundTile.ObjectPosition.Y), _forestTexture);
                        _forestTiles.Add(newForestBackgroundTile);
                    }
                    else
                    {
                        ForestBackgroundTile newForestBackgroundTile = new ForestBackgroundTile(new Vector2(currentForestBackgroundTile.ObjectPosition.X +
                        randomDouble * 1000, currentForestBackgroundTile.ObjectPosition.Y), _forestTexture);
                        _forestTiles.Add(newForestBackgroundTile);
                    }
                }
            }
        }

        private void AddPossibleAlienTurret(BackgroundTile newBackgroundTile)
        {
            if (newBackgroundTile.TileType == 2 || newBackgroundTile.TileType == 3)
            {
                float randomDouble = (float)GameSettings.Random.NextDouble();
                if (randomDouble >= 0.90)
                {
                    if (newBackgroundTile.IsFlipped == false)
                    {
                        AlienTurret newAlienTurret = new AlienTurret(new Vector2(newBackgroundTile.ObjectPosition.X - newBackgroundTile.ObjectSize.X / 2
                            + GameSettings.ALIENTURRETSIZE.X / 2, newBackgroundTile.ObjectPosition.Y + newBackgroundTile.ObjectSize.Y / 2),
                            _alienTurretTexture, false);
                        _aliens.Add(newAlienTurret);
                    }
                    else
                    {
                        AlienTurret newAlienTurret = new AlienTurret(new Vector2(newBackgroundTile.ObjectPosition.X - newBackgroundTile.ObjectSize.X / 2
                           + GameSettings.ALIENTURRETSIZE.X / 2, newBackgroundTile.ObjectPosition.Y + newBackgroundTile.ObjectSize.Y / 2 
                           - GameSettings.ALIENTURRETSIZE.Y), _alienTurretTexture, true);
                        _aliens.Add(newAlienTurret);
                    }
                }
            }
        }

        private int GetRandomTileType()
        {
            float randomDouble = (float)GameSettings.Random.NextDouble();
            if (randomDouble <= 0.48)
                return 2;
            else if (randomDouble <= 0.96)
                return 3;
            else return 4;
        }

        private void UpdateBackgroundTiles(GameTime gameTime)
        {
            for (int i = 0; i < _backgroundTiles.Count; i++)
            {
                BackgroundTile currentBackgroundTile = _backgroundTiles[i];
                currentBackgroundTile.Update(gameTime);
            }

            for (int i = 0; i < _forestTiles.Count; i++)
            {
                ForestBackgroundTile currentForestBackgroundTile = _forestTiles[i];
                currentForestBackgroundTile.Update(gameTime);
            }
        }

        private void RestartGameIfNecessary()
        {
            if (GameSettings.IsSpaceDownFirstFrame())
            {
                ClearLists();
                _score.ResetScore();
                GameSettings.SetGameStateToControlScreen();
            }
        }

        private void ClearLists()
        {
            for (int i = _aliens.Count; i > 0; i--)
            {
                Aliens currentAlien = _aliens[i - 1];
                _aliens.Remove(currentAlien);
            }

            for (int i = _projectiles.Count; i > 0; i--)
            {
                Projectile currentProjectile = _projectiles[i - 1];
                _projectiles.Remove(currentProjectile);
            }

            for (int i = _alienProjectiles.Count; i > 0; i--)
            {
                AlienProjectile currentAlienProjectile = _alienProjectiles[i - 1];
                _alienProjectiles.Remove(currentAlienProjectile);
            }

            for (int i = _backgroundTiles.Count; i > 0; i--)
            {
                BackgroundTile currentBackgroundTile = _backgroundTiles[i - 1];
                _backgroundTiles.Remove(currentBackgroundTile);
            }

            for (int i = _forestTiles.Count; i > 0; i--)
            {
                ForestBackgroundTile currentForestBackgroundTile = _forestTiles[i - 1];
                _forestTiles.Remove(currentForestBackgroundTile);
            }
        }

        private void PressSpaceToPlay()
        {
            if (GameSettings.IsSpaceDownFirstFrame())
            {
                GameSettings.SetGameStateToPlaying();
                _gameStartSFX.Play();
                CreateNewShip();
                CreateStartingBackgroundTiles();
                MediaPlayer.Play(_currentSong);
                
                _waveTimer = GameSettings.TIMEBETWEENWAVES;
            }
        }

        private void CreateStartingBackgroundTiles()
        {
            BackgroundTile newTopBackgroundTile = new BackgroundTile(new Vector2(GameSettings.BACKGROUNDSTARTINGDISTANCE, 0), _backgroundTexture, 1, true);
            BackgroundTile newBottomBackgroundTile = new BackgroundTile(new Vector2(GameSettings.BACKGROUNDSTARTINGDISTANCE, GameSettings.WINDOWHEIGHT - _backgroundTexture.Height / 2),
                _backgroundTexture, 1, false);
            ForestBackgroundTile newForestBackgroundTile = new ForestBackgroundTile(new Vector2(GameSettings.FORESTSTARTINGDISTANCE, 
                newBottomBackgroundTile.ObjectPosition.Y * 6 / 5), _forestTexture);

            _backgroundTiles.Add(newTopBackgroundTile);
            _backgroundTiles.Add(newBottomBackgroundTile);
            _forestTiles.Add(newForestBackgroundTile);
        }

        private void CheckShipCollision()
        {
            for (int k = _aliens.Count; k > 0; k--)
            {
                Aliens currentAlien = _aliens[k - 1];
                if (Rectangle.Intersect(_avatarShip.ObjectDestinationRectangle, currentAlien.ObjectDestinationRectangle) != Rectangle.Empty &&
                    currentAlien.IsHit == false && _avatarShip.IsHit == false)
                {
                    EndCurrentGame();
                }
            }

            for (int i = _alienProjectiles.Count; i > 0; i--)
            {
                AlienProjectile currentAlienProjectile = _alienProjectiles[i - 1];
                if (Rectangle.Intersect(_avatarShip.ObjectDestinationRectangle, currentAlienProjectile.ObjectDestinationRectangle) != Rectangle.Empty &&
                   currentAlienProjectile.IsActive == true && _avatarShip.IsHit == false)
                {
                    EndCurrentGame();
                }
            }
        }

        private void EndCurrentGame()
        {
            _score.SetNewHighScoreIfNecessary();
            _avatarShip.IsHit = true;
            _shipExplosionSFX.Play();
            MediaPlayer.Stop();
        }

        private void CheckProjectileCollision()
        {
            for (int i = _projectiles.Count; i > 0; i--)
            {
                Projectile currentProjectile = _projectiles[i - 1];
                for (int k = _aliens.Count; k > 0; k--)
                {
                    Aliens currentAlien = _aliens[k - 1];
                    if (Rectangle.Intersect(currentProjectile.ObjectDestinationRectangle,currentAlien.ObjectDestinationRectangle) != Rectangle.Empty &&
                        currentAlien.IsHit == false)
                    {
                        currentAlien.IsHit = true;
                        currentProjectile.IsActive = false;
                        _score.IncreaseScore();
                        _alienExplosionSFX.Play();
                    }
                }
            }
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            for(int i = 0; i < _projectiles.Count; i++)
            {
                Projectile currentProjectile = _projectiles[i];
                currentProjectile.Update(gameTime);
            }

            for (int i = 0; i < _alienProjectiles.Count; i++)
            {
                AlienProjectile currentAlienProjectile = _alienProjectiles[i];
                currentAlienProjectile.Update(gameTime);
            }
        }

        private void FireProjectileIfNecessary()
        {
            if (GameSettings.IsSpaceDownFirstFrame())
            {
                Projectile newProjectile = new Projectile(new Vector2(_avatarShip.ObjectPosition.X + GameSettings.AVATARSHIPSIZE.X,
                    _avatarShip.ObjectPosition.Y + GameSettings.AVATARSHIPSIZE.Y / 2 - GameSettings.PROJECTILESIZE.Y / 2), _projectileTexture);
                _projectiles.Add(newProjectile);
                _projectileSFX.Play();
            }
        }

        private void UpdateAliens(GameTime gameTime)
        {
            for (int i = 0; i < _aliens.Count; i++)
            {
                Aliens currentAlien = _aliens[i];
                currentAlien.UpdateAliens(gameTime, _avatarShip.ObjectPosition);
            }
        }

        private void SpawnNewWaveIfNecessary()
        {
            _waveTimer--;
            if (_waveTimer <= 0)
            {
                SpawnRandomWave();
            }
        }

        private void SpawnRandomWave()
        {
            int alienWaveType = GameSettings.Random.Next(1, 4);

            if(alienWaveType == 1)
            {
                _waveTimer = GameSettings.TIMEBETWEENWAVES;
                SpawnAlienType01Wave();
            }
            else if (alienWaveType == 2)
            {
                _waveTimer = GameSettings.TIMEBETWEENWAVES * 2 / 3;
                SpawnAlienType02Wave();
            }
            else if (alienWaveType == 3)
            {
                _waveTimer = GameSettings.TIMEBETWEENWAVES / 3;
                SpawnAlienType03Wave();
            }
        }

        private void SpawnAlienType03Wave()
        {
            float horizontalDisctanceInbetWeen = GameSettings.WINDOWWIDTH / GameSettings.DISTANCEBETWEENALIENSINWAVE / 2;
            float alien03YPosition = GameSettings.Random.Next(GameSettings.WINDOWHEIGHT / 10, 
                GameSettings.WINDOWHEIGHT * 9 / 10 - (int)GameSettings.ALIENSHIP03SIZE.Y);

            for (int alienNr = 1; alienNr <= GameSettings.NUMBEROFALIEN03PERROW; alienNr++)
            {
                Vector2 newAlien03Position = new Vector2(GameSettings.WINDOWWIDTH + horizontalDisctanceInbetWeen * alienNr,
                    alien03YPosition);

                AlienShip03 newAlien03 = new AlienShip03(newAlien03Position, _alienShip03Texture);
                _aliens.Add(newAlien03);
            }
        }

        private void SpawnAlienType02Wave()
        {
            float horizontalDisctanceInbetWeen = GameSettings.WINDOWWIDTH / GameSettings.DISTANCEBETWEENALIENSINWAVE;

            for (int col = 1; col <= GameSettings.NUMBEROFALIEN02COLSINWAVE; col++)
            {
                for (int row = 1; row <= GameSettings.NUMBEROFALIEN02PERCOL; row++)
                {
                    float verticalDistanceInbetween = GameSettings.WINDOWHEIGHT / (GameSettings.NUMBEROFALIEN02PERCOL +1);
                    Vector2 newAlien02Position = new Vector2(GameSettings.WINDOWWIDTH + horizontalDisctanceInbetWeen * col,
                        verticalDistanceInbetween * row - GameSettings.ALIENSHIP02SIZE.Y / 2);

                    AlienShip02 newAlien02 = new AlienShip02(newAlien02Position, _alienShip02Texture);
                    _aliens.Add(newAlien02);
                }
            }
        }

        private void SpawnAlienType01Wave()
        {
            float horizontalDisctanceInbetWeen = GameSettings.WINDOWWIDTH / GameSettings.DISTANCEBETWEENALIENSINWAVE;

            for (int col = 1; col <= GameSettings.NUMBEROFALIEN01COLSINWAVE; col++)
            {
                for(int row = 1; row <= col; row++)
                {
                    float verticalDistanceInbetween = GameSettings.WINDOWHEIGHT / (col + 1);
                    Vector2 newAlien01Position = new Vector2(GameSettings.WINDOWWIDTH + horizontalDisctanceInbetWeen * col,
                        verticalDistanceInbetween * row - GameSettings.ALIENSHIP01SIZE.Y / 2);

                    AlienShip01 newAlien01 = new AlienShip01(newAlien01Position, _alienShip01Texture);
                    _aliens.Add(newAlien01);
                }
            }
        }

        private void RemoveInactiveObjects()
        {
            for (int i = _aliens.Count; i > 0; i--)
            {
                Aliens currentAlien = _aliens[i - 1];
                if (currentAlien.IsActive == false)
                    _aliens.Remove(currentAlien);
            }

            for (int i = _projectiles.Count; i > 0; i--)
            {
                Projectile currentProjectile = _projectiles[i - 1];
                if (currentProjectile.IsActive == false)
                    _projectiles.Remove(currentProjectile);
            }

            for (int i = _alienProjectiles.Count; i > 0; i--)
            {
                AlienProjectile currentalienProjectile = _alienProjectiles[i - 1];
                if (currentalienProjectile.IsActive == false)
                    _alienProjectiles.Remove(currentalienProjectile);
            }

            for (int i = _backgroundTiles.Count; i > 0; i--)
            {
                BackgroundTile currentBackgroundTile = _backgroundTiles[i - 1];
                if(currentBackgroundTile.IsActive == false)
                    _backgroundTiles.Remove(currentBackgroundTile);
            }

            for (int i = _forestTiles.Count; i > 0; i--)
            {
                ForestBackgroundTile currentForestBackgroundTile = _forestTiles[i - 1];
                if (currentForestBackgroundTile.IsActive == false)
                    _forestTiles.Remove(currentForestBackgroundTile);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.Playing)
            {
                DrawBackgroundTiles(spriteBatch);
                _avatarShip.Draw(spriteBatch);
                DrawProjectiles(spriteBatch);
                DrawAliens(spriteBatch);
                _score.DrawPlayingScreenScore(spriteBatch);
            }

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.ControlScreen)
            {
                DrawStartScreen(spriteBatch);
                _score.DrawControlScreenScore(spriteBatch);
            }

            if (GameSettings.CURRENTGAMESTATE == GameSettings.GameState.GameOver)
            {
                DrawEndScreen(spriteBatch);
                _score.DrawGameOverScreenScore(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBackgroundTiles(SpriteBatch spriteBatch)
        {
            foreach (ForestBackgroundTile currentForestTile in _forestTiles)
            {
                currentForestTile.Draw(spriteBatch);
            }

            foreach (BackgroundTile currentBackgroundTile in _backgroundTiles)
            {
                currentBackgroundTile.Draw(spriteBatch);
            }
        }

        private void DrawEndScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_endScreenTexture, new Rectangle(0, 0, GameSettings.WINDOWWIDTH, GameSettings.WINDOWHEIGHT), Color.White);
        }

        private void DrawStartScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_startScreenTexture, new Rectangle(0, 0, GameSettings.WINDOWWIDTH, GameSettings.WINDOWHEIGHT), Color.White);
        }

        private void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile currentProjectile in _projectiles)
            {
                currentProjectile.Draw(spriteBatch);
            }

            foreach (AlienProjectile currentAlienProjectile in _alienProjectiles)
            {
                currentAlienProjectile.Draw(spriteBatch);
            }
        }

        private void DrawAliens(SpriteBatch spriteBatch)
        {
            foreach (Aliens currentAlien in _aliens)
            {
                currentAlien.Draw(spriteBatch);
            }
        }
    }
}
