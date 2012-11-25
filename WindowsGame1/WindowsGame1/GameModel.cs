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
    /// <summary>
    /// 
    /// </summary>
    class GameModel
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public GameModel()
        {
            GameModel.XDistanceBetweenSprites = 100;
            GameModel.YDistanceBetweenSprites = 300;
            GameModel.YDistanceBetweenStars = 50;
            GameModel.YDistanceBetweenStars = 200;
            GameModel.YDistanceBetweenBlueStars = 50;
            GameModel.YDistanceBetweenSmallStars = 200;
            GameModel.LeftDisplayBorder = 50;
            GameModel.RightDisplayBorder = 200;
            GameModel.BottomDisplayBorder = 140;

            _background = new Background();

            _enemies[0] = new Enemy1(4, 3);
            _enemies[1] = new Enemy2(4, 3);
            _enemies[2] = new Enemy3(4, 3);
            _enemies[3] = new Enemy4(4, 3);

            _spaceShip = new AnimatedSprite(5, 4);

            for (int i = 0; i < _meteors.Length; i++)
                _meteors[i] = new Meteor(9, 8);

            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Star(15, 7);
                _stars[i].Speed = new Vector2(0, 0.5f);
            }

            for (int i = 0; i < _fastStars.Length; i++)
            {
                _fastStars[i] = new Star(15, 10);
                _fastStars[i].Speed = new Vector2(0, 1f);
            }

            _InitializeLives();

            _progress = new ProgressSprite();
        }

        #endregion

        #region Public properties

        public ProgressSprite Progress
        {
            get { return _progress; }
            private set { _progress = value; }
        }

        public AnimatedSprite SpaceShip
        {
            get { return _spaceShip; }
            private set { _spaceShip = value; }
        }

        public AnimatedSprite[] Enemies
        {
            get { return _enemies; }
            private set { _enemies = value; }
        }

        public List<Rocket> Rockets
        {
            get { return _rockets; }
            private set { _rockets = value; }
        }

        public List<Rocket> EnemyRockets
        {
            get { return _enemyRockets; }
            private set { _enemyRockets = value; }
        }

        public Meteor[] Meteors
        {
            get { return _meteors; }
            set { _meteors = value; }
        }

        public Star[] Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public Star[] FastStars
        {
            get { return _fastStars; }
            set { _fastStars = value; }
        }

        public LiveBonus LiveBonus
        {
            get { return _bonusLive; }

            set { _bonusLive = value; }
        }

        public List<Sprite> Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }

        #endregion

        #region Game settings public properties

        public static int YDistanceBetweenStars
        {
            get;
            private set;
        }

        public static int YDistanceBetweenBlueStars
        {
            get;
            private set;
        }

        public static int YDistanceBetweenSmallStars
        {
            get;
            private set;
        }

        public static int XDistanceBetweenStars
        {
            get;
            private set;
        }

        public static int YDistanceBetweenSprites
        {
            get;
            private set;
        }

        public static int XDistanceBetweenSprites
        {
            get;
            private set;
        }

        public static int LeftDisplayBorder
        {
            get;
            private set;
        }

        public static int RightDisplayBorder
        {
            get;
            private set;
        }

        public static int BottomDisplayBorder
        {
            get;
            private set;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientBounds"></param>
        public void InitializeCharacters(Rectangle clientBounds)
        {
            int clientWidth = clientBounds.Width;
            int temp = 0;
            for (int i = 0; i < _enemies.Length; i++)
            {
                _enemies[i].SpritePosition = new Vector2(_GetRandomXPosition(clientWidth),
                    temp = temp - YDistanceBetweenSprites);
            }

            temp = 0;
            for (int i = 0; i < _meteors.Length; i++)
                _meteors[i].SpritePosition = new Vector2(_GetRandomXPosition(clientWidth),
                    temp = temp - YDistanceBetweenStars);

            temp = clientBounds.Height;
            for (int i = 0; i < _stars.Length; i++)
                _stars[i].SpritePosition = new Vector2(_GetRandomXPosition(clientWidth),
                    temp = temp - YDistanceBetweenStars);

            temp = clientBounds.Height;
            for (int i = 0; i < _fastStars.Length; i++)
                _fastStars[i].SpritePosition = new Vector2(_GetRandomXPosition(clientWidth),
                    temp = temp - YDistanceBetweenStars);

            _spaceShip.SpritePosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height - BottomDisplayBorder);

            _progress.Initialize(new Vector2(10,
                clientBounds.Height - 235));

            _InitializeLives();

            _bonusLive.SpritePosition = new Vector2(_GetRandomXPosition(clientWidth), -1000);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            _rockets.Clear();
            _enemyRockets.Clear();

            _background.Load(content);

            _spaceShip.Load(content, "SpaceShipAnimated");

            for (int i = 0; i < _enemies.Length; i++)
                _enemies[i].Load(content);

            for (int i = 0; i < _meteors.Length; i++)
                _meteors[i].Load(content);

            for (int i = 0; i < _stars.Length; i++)
                _stars[i].Load(content);

            for (int i = 0; i < _fastStars.Length; i++)
                _fastStars[i].Load(content);

            _bonusLive.Load(content);

            _liveTexture = content.Load<Texture2D>("live");

            foreach (var live in _lives)
                live.SpriteTexture = _liveTexture;

            _rocketTexture = content.Load<Texture2D>("Pulka");

            _progress.Load(content);

            _winnerSprite.SpriteTexture = content.Load<Texture2D>("WinnerSprite");
            _winnerSprite.SpritePosition = new Vector2(200, 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            _background.DrawSprite(spriteBatch);

            // Draw all the sprites.
            for (int i = 0; i < _stars.Length; i++)
                _stars[i].DrawSprite(spriteBatch);

            for (int i = 0; i < _fastStars.Length; i++)
                _fastStars[i].DrawSprite(spriteBatch);

            if (gameState.CurrentLevel > 1)
                for (int i = 0; i < _meteors.Length; i++)
                    _meteors[i].DrawSprite(spriteBatch);

            for (int i = 0; i < _enemies.Length; i++)
                _enemies[i].DrawSprite(spriteBatch);

            foreach (var rocket in _rockets)
                rocket.DrawSprite(spriteBatch);

            foreach (var rocket in _enemyRockets)
                rocket.DrawSprite(spriteBatch);

            _spaceShip.DrawSprite(spriteBatch);

            _progress.DrawSprite(spriteBatch);

            _bonusLive.DrawSprite(spriteBatch);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(double elapsedTime, GameState gameState)
        {
            for (int i = 0; i < _enemies.Length; i++)
                _enemies[i].UpdateFrame(elapsedTime);

            if (gameState.CurrentLevel > 1)
                for (int i = 0; i < _meteors.Length; i++)
                    _meteors[i].UpdateFrame(elapsedTime);

            for (int i = 0; i < _fastStars.Length; i++)
                _fastStars[i].UpdateFrame(elapsedTime);

            for (int i = 0; i < _stars.Length; i++)
                _stars[i].UpdateFrame(elapsedTime);

            foreach (var rocket in _rockets)
                rocket.UpdateFrame(elapsedTime);

            foreach (var rocket in _enemyRockets)
                rocket.UpdateFrame(elapsedTime);

            _spaceShip.UpdateFrame(elapsedTime);

            _bonusLive.UpdateFrame(elapsedTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRocket()
        {
            var rocket = new Rocket();
            rocket.SpriteTexture = _rocketTexture;
            rocket.SpritePosition = new Vector2(
                _spaceShip.SpritePosition.X + _spaceShip.SpriteTexture.Width / 15,
                _spaceShip.SpritePosition.Y);
            _rockets.Add(rocket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddEnemyRocket(float x, float y)
        {
            var rocket = new Rocket();
            rocket.SpriteTexture = _rocketTexture;
            rocket.SpritePosition = new Vector2(x, y);
            _enemyRockets.Add(rocket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void ShowWinnerSprite(SpriteBatch spriteBatch)
        {
            _winnerSprite.DrawSprite(spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLives(SpriteBatch spriteBatch)
        {
            foreach (var live in _lives)
                live.DrawSprite(spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddLive()
        {
            _AddLive();
        }

        public void RemoveLive()
        {
            _lives.RemoveAt(_lives.Count - 1);
        }

        #endregion

        #region Private methods

        private void _InitializeLives()
        {
            _lives.Clear();

            for (int i = 0; i < 5; i++)
                _AddLive();
        }

        private void _AddLive()
        {
            var live = new Sprite();

            double xPosition = _lives.Count * 40 + 100;
            double yPosition = 10;

            live.SpritePosition = new Vector2((float)xPosition, (float)yPosition);
            live.SpriteTexture = _liveTexture;

            _lives.Add(live);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float _GetRandomXPosition(int clientBoundsWidth)
        {
            return _randomizer.Next(LeftDisplayBorder,
                    clientBoundsWidth - RightDisplayBorder);
        }

        #endregion

        #region Private fields

        private Random _randomizer = new Random();

        private Background _background;

        private AnimatedSprite[] _enemies = new AnimatedSprite[4];

        private List<Rocket> _rockets = new List<Rocket>();

        private List<Rocket> _enemyRockets = new List<Rocket>();

        private AnimatedSprite _spaceShip;

        private Meteor[] _meteors = new Meteor[3];

        private Star[] _stars = new Star[10];

        private Star[] _fastStars = new Star[10];

        private Texture2D _rocketTexture;

        private Sprite _winnerSprite = new Sprite();

        private ProgressSprite _progress;

        private LiveBonus _bonusLive = new LiveBonus(5, 4);

        private List<Sprite> _lives = new List<Sprite>();

        private Texture2D _liveTexture;

        #endregion
    }
}
