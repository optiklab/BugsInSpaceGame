/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Bugs.Sprites;
using Bugs.Display;
using Bugs.Characters;

namespace Bugs
{
    class GameController
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public GameController(GameWindow window)
        {
            _window = window;

            GameState = new GameState();
            GameState.CurrentState = State.Menu;

            _gameOverTimer = new System.Timers.Timer(1000);
            _gameOverTimer.Elapsed += new ElapsedEventHandler(_OnTimedEvent);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public GameState GameState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGameFinished
        {
            get
            {
                return _isGameFinished;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes sprites positions.
        /// </summary>
        public void Initialize()
        {
            _model.InitializeCharacters(_window.ClientBounds);

            GameState.CurrentLevel = 0;
            GameState.CurrentScore = 0;
            GameState.IsGameOver = false;
            GameState.IsPausePressed = false;
            _isGameFinished = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            _model.LoadContent(content);

            _controlPanel.Load(content);

            _infoDisplay.LoadContent(content, _window.ClientBounds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboardState"></param>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (_isGameFinished)
                return;

            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            // User shoots.
            if (keyboardState.IsKeyDown(Keys.Space) && !_isShooting && !_model.Progress.IsOverheat)
            {
                _model.AddRocket();
                _shootsCount += 1;
                AudioPlayer.PlayShipSimpleShoot();
                _isShooting = true;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && _isShooting)
                _isShooting = false;

            _model.Update(elapsedTime, GameState);

            _MoveAllSprites();

            _MoveShip(keyboardState);

            _SolveCollisions();

            _ConsiderScoreStatement();

            _infoDisplay.UpdateInfo(_gameRules.LevelsPoints[(int)GameState.CurrentLevel], GameState.CurrentScore);

            _CalculateAmmoHeating(gameTime);

            _CalculateEnemiesRockets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void _CalculateAmmoHeating(GameTime gameTime)
        {
            double shootFreq = _shootsCount / gameTime.TotalGameTime.TotalSeconds;

            for (int i = 10; i >= 0; i--)
            {
                if (shootFreq > _model.Progress.LevelsArray[i])
                {
                    _model.Progress.SelectedItem = i;

                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _CalculateEnemiesRockets()
        {
            // Enemies shoots turns on only in 3rd level.
            if (GameState.CurrentLevel > 2)
            {
                // Default rocket position is 0,0. When its Y = 0, we generates new RANDOM Y poisition...
                if (_enemyRocketStartPos.Y == 0)
                {
                    _enemyRocketStartPos.Y = _GetRandomEnemyRocketYPosition();
                }
                else if (_enemyRocketStartPos.X == 0)
                {
                    // AFTER that, if X = 0: we're waiting for ANY enemy, who will visit the bounds of RANDOM Y...(+/- 5).
                    foreach (var enemy in _model.Enemies)
                    {
                        // When anyone from enemies met RANDOM Y position, shoot from that point (to looks like this enemy shoot).
                        if (enemy.SpritePosition.Y > _enemyRocketStartPos.Y - 5 &&
                            enemy.SpritePosition.Y < _enemyRocketStartPos.Y + 5)
                        {
                            // Remember X point of that enemy...
                            _enemyRocketStartPos.X = enemy.SpritePosition.X + enemy.SpriteTexture.Width / 10;

                            // Move Y a bit too... because Enemies Rockets should appear BEFORE enemy graphic.
                            _enemyRocketStartPos.Y = enemy.SpritePosition.Y + enemy.SpriteTexture.Height;

                            // And add this rocket.
                            _model.AddEnemyRocket(_enemyRocketStartPos.X, _enemyRocketStartPos.Y);
                            AudioPlayer.PlayEnemySimpleShoot();
                            break; // Work done.
                        }
                    }
                }
                else
                {
                    _SolveEnemiesRocketsCollisions();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _model.Draw(spriteBatch, GameState);

            if (_isGameFinished)
                _model.ShowWinnerSprite(spriteBatch);

            _controlPanel.DrawSprite(spriteBatch);

            _infoDisplay.DrawInfo(gameTime, spriteBatch, GameState);

            _model.DrawLives(spriteBatch);

            if (GameState.CurrentState != State.GameOver && GameState.CurrentState != State.Paused)
            {
                if (_model.Progress.IsOverheat)
                {
                    int secondsForColling = _model.Progress.GetTimeForCooling(gameTime.TotalGameTime.TotalSeconds, _shootsCount);
                    _infoDisplay.OverheatStatus(spriteBatch, secondsForColling);
                }
                else
                    _infoDisplay.NextLevelStatus(spriteBatch);
            }

            // Consider if we need to show hint... Hint is showing for some time (60 ticks) after changing the level.
            if (_currentLevelShowTimer > 0 && _currentLevelShowTimer <= 60)
            {
                // Show appripriate hint
                _infoDisplay.LevelCameUp(GameState.CurrentLevel, spriteBatch);
                _currentLevelShowTimer += 1;
            }
            else
            {
                _currentLevelShowTimer = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SpeedUp()
        {
            foreach (var enemy in _model.Enemies)
            {
                if (enemy.Speed != enemy.MaxSpeed)
                    enemy.Speed = enemy.MaxSpeed;
            }

            foreach (var meteor in _model.Meteors)
            {
                if (meteor.Speed != meteor.MaxSpeed)
                    meteor.Speed = meteor.MaxSpeed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SlowDown()
        {
            foreach (var enemy in _model.Enemies)
            {
                if (enemy.Speed != enemy.DefaultSpeed)
                    enemy.Speed = enemy.DefaultSpeed;
            }

            foreach (var meteor in _model.Meteors)
            {
                if (meteor.Speed != meteor.DefaultSpeed)
                    meteor.Speed = meteor.DefaultSpeed;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float _GetRandomEnemyRocketYPosition()
        {
            return _randomizer.Next(20, _window.ClientBounds.Height / 3);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _MoveAllSprites()
        {
            _MoveSprite(_model.Enemies, GameModel.XDistanceBetweenSprites,
                GameModel.XDistanceBetweenSprites, 4);

            _MoveSprite(_model.Stars, GameModel.XDistanceBetweenStars,
                GameModel.YDistanceBetweenBlueStars, 22);

            _MoveSprite(_model.FastStars, GameModel.XDistanceBetweenStars,
                GameModel.YDistanceBetweenSmallStars, 22);

            // GOOD GUY ROCKETS: Update positions and remove redundant elements.
            var oldRockets = new List<Rocket>();
            foreach (var rocket in _model.Rockets)
            {
                rocket.SpritePosition -= rocket.Speed;

                if (rocket.SpritePosition.Y < 0)
                    oldRockets.Add(rocket);
            }

            for (int i = 0; i < oldRockets.Count; i++)
                _model.Rockets.Remove(oldRockets[i]);

            // ENEMY ROCKETS: Update positions and remove redundant elements.
            var oldEnemyRockets = new List<Rocket>();
            foreach (var rocket in _model.EnemyRockets)
            {
                rocket.SpritePosition += rocket.Speed;

                if (rocket.SpritePosition.Y > _window.ClientBounds.Height)
                    oldEnemyRockets.Add(rocket);
            }

            for (int i = 0; i < oldEnemyRockets.Count; i++)
            {
                _model.EnemyRockets.Remove(oldEnemyRockets[i]);
                _enemyRocketStartPos.X = 0;
                _enemyRocketStartPos.Y = 0;
            }

            // LIVES BONUS:
            if (GameState.CurrentLevel > 2)
            {
                _model.LiveBonus.SpritePosition += _model.LiveBonus.Speed;

                if (_model.LiveBonus.SpritePosition.Y > _window.ClientBounds.Height)
                {
                    _model.LiveBonus.SpritePosition = new Vector2(
                        _randomizer.Next(GameModel.LeftDisplayBorder,
                            _window.ClientBounds.Width - GameModel.RightDisplayBorder),
                        -3000);
                }
            }

            // Meteors
            if (GameState.CurrentLevel > 1)
            {
                _MoveSprite(_model.Meteors, GameModel.XDistanceBetweenStars,
                    GameModel.YDistanceBetweenStars, 22);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="xDistanceBetweenSprites"></param>
        /// <param name="yDistanceBetweenSprites"></param>
        /// <param name="spriteFramesCount"></param>
        private void _MoveSprite(AnimatedSprite[] sprites, int xDistanceBetweenSprites,
            int yDistanceBetweenSprites, int spriteFramesCount)
        {
            var distance = _window.ClientBounds.Width - xDistanceBetweenSprites;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SpritePosition += sprites[i].Speed;

                // When sprite comes over the Screen, bring him back
                if (sprites[i].SpritePosition.Y > _window.ClientBounds.Height)
                {
                    sprites[i].SpritePosition = new Vector2(_randomizer.Next(
                        GameModel.LeftDisplayBorder,
                        distance - sprites[i].SpriteTexture.Width / spriteFramesCount),
                        -yDistanceBetweenSprites);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _MoveShip(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left))
                _model.SpaceShip.SpritePosition.X -= 10;
            else if (keyboardState.IsKeyDown(Keys.Right))
                _model.SpaceShip.SpritePosition.X += 10;

            var maxX = _window.ClientBounds.Width - GameModel.RightDisplayBorder;

            if (_model.SpaceShip.SpritePosition.X < GameModel.LeftDisplayBorder)
                _model.SpaceShip.SpritePosition.X = GameModel.LeftDisplayBorder;
            else if (_model.SpaceShip.SpritePosition.X > maxX)
                _model.SpaceShip.SpritePosition.X = maxX;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _RecalculateBoxes()
        {
            // Create Min (Left+Top corner) and Max (Right+Bottom corner) 2D (z = 0) borders for space ship.
            BoundingBox spaceShipBox = _model.SpaceShip.BoundingBox;

            spaceShipBox.Min = new Vector3(_model.SpaceShip.SpritePosition.X,
                _model.SpaceShip.SpritePosition.Y, 0);

            spaceShipBox.Max = new Vector3(_model.SpaceShip.SpritePosition.X + _model.SpaceShip.SpriteTexture.Width/5,
                _model.SpaceShip.SpritePosition.Y + _model.SpaceShip.SpriteTexture.Height, 0);

            _model.SpaceShip.BoundingBox = spaceShipBox;

            // Create Mins and Maxes 2D borders for bugs.
            foreach (var enemy in _model.Enemies)
            {
                var position = enemy.SpritePosition;
                var texture = enemy.SpriteTexture;

                BoundingBox box = enemy.BoundingBox;
                box.Min = new Vector3(position.X, position.Y, 0);
                box.Max = new Vector3(position.X + texture.Width / 4,
                                      position.Y + texture.Height, 0);

                enemy.BoundingBox = box;
            }

            // Create Mins and Maxes 2D borders for GOOD GUY ROCKETS.
            foreach (var rocket in _model.Rockets)
            {
                BoundingBox box = rocket.BoundingBox;
                var position = rocket.SpritePosition;
                var texture = rocket.SpriteTexture;

                box.Min = new Vector3(position.X, position.Y, 0);
                box.Max = new Vector3(
                    position.X + texture.Width / 3,
                    position.Y + texture.Height,
                    0);

                rocket.BoundingBox = box;
            }

            // LEVEL DEPENDEND UPDATES
            // Meteorits
            if (GameState.CurrentLevel > 1)
            {
                foreach (var meteor in _model.Meteors)
                {
                    var position = meteor.SpritePosition;
                    var texture = meteor.SpriteTexture;

                    BoundingBox box = meteor.BoundingBox;
                    box.Min = new Vector3(position.X, position.Y + texture.Height / 2, 0);
                    // Height / 2 => because we don't need to touch meteor in its tail
                    box.Max = new Vector3(position.X + texture.Width / 5, // 5 frames
                                          position.Y + texture.Height, 0);

                    meteor.BoundingBox = box;
                }
            }

            // Create Mins and Maxes 2D borders for ENEMIES ROCKETS.
            if (GameState.CurrentLevel > 2)
            {
                foreach (var rocket in _model.EnemyRockets)
                {
                    BoundingBox box = rocket.BoundingBox;
                    var position = rocket.SpritePosition;
                    var texture = rocket.SpriteTexture;

                    box.Min = new Vector3(position.X, position.Y, 0);
                    box.Max = new Vector3(
                        position.X + texture.Width / 3,
                        position.Y + texture.Height,
                        0);

                    rocket.BoundingBox = box;
                }

                // Live bonus
                BoundingBox liveBonusBox = _model.LiveBonus.BoundingBox;
                liveBonusBox.Min = new Vector3(_model.LiveBonus.SpritePosition.X, _model.LiveBonus.SpritePosition.Y, 0);
                liveBonusBox.Max = new Vector3(
                    _model.LiveBonus.SpritePosition.X + _model.LiveBonus.SpriteTexture.Width / 3,
                    _model.LiveBonus.SpritePosition.Y + _model.LiveBonus.SpriteTexture.Height,
                    0);
                _model.LiveBonus.BoundingBox = liveBonusBox;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _SolveCollisions()
        {
            _RecalculateBoxes();

            _SolveEnemiesCollisions(_model.Enemies, 4, GameRules.DefaultPointsPerEnemy);

            if (GameState.CurrentLevel > 1)
                _SolveEnemiesCollisions(_model.Meteors, 5, GameRules.DefaultPointsPerMeteor);

            if (GameState.CurrentLevel > 2)
                _SolveLiveBonusCollisions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="framesCount"></param>
        /// <param name="pointsForKill"></param>
        private void _SolveEnemiesCollisions(AnimatedSprite[] sprites, int framesCount,
            uint pointsForKill)
        {
            // Solve collisions between boxes.
            var distance = _window.ClientBounds.Width - GameModel.XDistanceBetweenSprites;

            foreach (var enemy in sprites)
            {
                var maxValue = distance - enemy.SpriteTexture.Width / 4;

                var rocket = new Rocket();
                if ((rocket = _IsRocketBitTheEnemy(enemy.BoundingBox)) != null)
                {
                    // Recalculate position of the bugs... (could be BOOOM! or POINTS++).
                    enemy.SpritePosition = new Vector2(
                        _randomizer.Next(GameModel.LeftDisplayBorder, maxValue),
                        -GameModel.YDistanceBetweenSprites);

                    _model.Rockets.Remove(rocket);

                    AudioPlayer.PlayEnemyDestroyed();

                    GameState.CurrentScore += pointsForKill;
                }

                if (_model.SpaceShip.BoundingBox.Intersects(enemy.BoundingBox))
                {
                    // Recalculate position of the bugs... (could be BOOOM! or POINTS++).
                    enemy.SpritePosition = new Vector2(
                        _randomizer.Next(GameModel.LeftDisplayBorder, maxValue),
                        -GameModel.YDistanceBetweenSprites);

                    if (_model.Lives.Count > 1)
                    {
                        _model.RemoveLive();
                        TimeSpan span = AudioPlayer.PlayEnemySuperShoot();
                    }
                    else
                        _GameOver();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _SolveLiveBonusCollisions()
        {
            if (_model.SpaceShip.BoundingBox.Intersects(_model.LiveBonus.BoundingBox))
            {
                _model.LiveBonus.SpritePosition = new Vector2(
                    _randomizer.Next(GameModel.LeftDisplayBorder,
                        _window.ClientBounds.Width - GameModel.RightDisplayBorder),
                    -3000);

                if (_model.Lives.Count < 5)
                {
                    _model.AddLive();
                    TimeSpan span = AudioPlayer.LiveBonus();
                }
                else
                {
                    GameState.CurrentScore += GameRules.DefaultPointsPerBonus;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _ConsiderScoreStatement()
        {
            if (GameState.CurrentScore >= _gameRules.LevelsPoints[0] &&
                GameState.CurrentScore <= _gameRules.LevelsPoints[1] &&
                GameState.CurrentLevel == 0)
            {
                GameState.CurrentLevel = 1;
                _currentLevelShowTimer += 1;
            }
            else if (GameState.CurrentScore >= _gameRules.LevelsPoints[1] &&
                GameState.CurrentScore <= _gameRules.LevelsPoints[2] &&
                GameState.CurrentLevel == 1)
            {
                GameState.CurrentLevel = 2;
                _currentLevelShowTimer += 1;
            }
            else if (GameState.CurrentScore >= _gameRules.LevelsPoints[2] &&
                GameState.CurrentScore < _gameRules.LevelsPoints[3] &&
                GameState.CurrentLevel == 2)
            {
                GameState.CurrentLevel = 3;
                _currentLevelShowTimer += 1;
            }
            else if (GameState.CurrentScore >= _gameRules.LevelsPoints[3] &&
                GameState.CurrentScore < _gameRules.LevelsPoints[4] &&
                GameState.CurrentLevel == 3)
            {
                GameState.CurrentLevel = 4;
                _currentLevelShowTimer += 1;

                _GameWin();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _SolveEnemiesRocketsCollisions()
        {
            Rocket killRocket = null;

            foreach (var rocket in _model.EnemyRockets)
                if (rocket.BoundingBox.Intersects(_model.SpaceShip.BoundingBox))
                {
                    killRocket = rocket;

                    break;
                }

            if (killRocket != null)
            {
                _model.EnemyRockets.Remove(killRocket);
                _enemyRocketStartPos.Y = 0;
                _enemyRocketStartPos.X = 0;

                if (_model.Lives.Count > 1)
                {
                    _model.RemoveLive();
                    TimeSpan span = AudioPlayer.PlayShipSuperShoot();
                }
                else
                    _GameOver();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _GameOver()
        {
            AudioPlayer.Stop();

            TimeSpan span = AudioPlayer.PlayBomb();

            // Show game over screen and stop the game
            GameState.CurrentState = State.GameOver;

            _gameOverTimer.Interval = span.TotalMilliseconds;
            _gameOverTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _GameWin()
        {
            AudioPlayer.Stop();

            TimeSpan span = AudioPlayer.PlayWin();

            _isGameFinished = true;

            _gameOverTimer.Interval = span.TotalMilliseconds;
            _gameOverTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enemyBox"></param>
        /// <returns></returns>
        private Rocket _IsRocketBitTheEnemy(BoundingBox enemyBox)
        {
            foreach (var rocket in _model.Rockets)
                if (rocket.BoundingBox.Intersects(enemyBox))
                    return rocket;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // Come into menu
            GameState.CurrentState = State.UserInput;//.Menu;
            AudioPlayer.PlayMenu();
            _gameOverTimer.Enabled = false;
        }

        #endregion

        #region Private fields
        
        private int _shootsCount = 0;

        private int _currentLevelShowTimer = 0;

        private bool _isShooting = false;

        private bool _isGameFinished = false;

        private Vector2 _enemyRocketStartPos = new Vector2(0, 0);

        private Timer _gameOverTimer;

        private Random _randomizer = new Random();

        private GameWindow _window;

        private ControlPanel _controlPanel = new ControlPanel();

        private GameModel _model = new GameModel();

        private InfoDisplay _infoDisplay = new InfoDisplay();

        private GameRules _gameRules = new GameRules();

        #endregion
    }
}
