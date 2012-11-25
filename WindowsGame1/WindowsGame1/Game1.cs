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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferMultiSampling = false;
            _graphics.IsFullScreen = false;

            _controller = new GameController(Window);

            _menu = new Menu();

            _mouseCursor = new MouseCursor();

            _hitList = new HitsList();
            _userInput = new UserInput();
        }

        #endregion

        #region Protected overrided methods from Microsoft.Xna.Framework.Game

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        public void NewGame()
        {
            Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _controller.LoadContent(this.Content);
            _menu.LoadContent(this.Content);
            _hitList.LoadContent(this.Content);
            _userInput.LoadContent(this.Content);
            _mouseCursor.LoadContent(this.Content, Window);

            AudioPlayer.Load(this.Content);

            if (!_isInited)
                _span = AudioPlayer.PlayMenuStart();

            _isInited = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _hitList.SerializeHistory();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            _keyboardState = Keyboard.GetState();

            // Catch keys in menu.
            if (_controller.GameState.CurrentState == State.Menu)
            {
                _UpdateMenu(gameTime);
                _gameSoudStarted = false;
            }
            else if (_controller.GameState.CurrentState == State.Game)
            {
                _UpdateGame(gameTime);

                if (_keyboardState.IsKeyDown(Keys.Escape) && !_controller.IsGameFinished)
                {
                    AudioPlayer.Stop();
                    AudioPlayer.PlayMenu();
                    _controller.GameState.CurrentState = State.UserInput;
                }
                else if (_keyboardState.IsKeyDown(Keys.Up))
                {
                    _controller.SpeedUp();
                }
                else if (_keyboardState.IsKeyUp(Keys.Up))
                {
                    _controller.SlowDown();
                }
            }
            else if (_controller.GameState.CurrentState == State.Paused)
                _PauseGameIfNeeded();

            else if (_controller.GameState.CurrentState == State.Hits)
            {
                _hitList.Update(_keyboardState);

                if (_keyboardState.IsKeyDown(Keys.Escape))
                    _controller.GameState.CurrentState = State.Menu;
            }
            else if (_controller.GameState.CurrentState == State.UserInput)
            {
                _userInput.Update(_keyboardState);

                if (_userInput.IsInputFinished)
                {
                    string userName = _userInput.UserName;
                    string score = _userInput.Score;

                    _hitList.AddUserData(new UserData() { Score = score, UserName = userName, CreationTime = DateTime.Now });

                    _controller.GameState.CurrentState = State.Hits;
                }
            }
            else
            {
                // Do nothing.
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw game background.
            GraphicsDevice.Clear(Color.Black);

            // Begin sprite batch to draw all game sprites.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Draw menu sprites.
            if (_controller.GameState.CurrentState == State.Menu)
            {
                _menu.Draw(_spriteBatch, gameTime);
                _mouseCursor.DrawMouse(_spriteBatch);
            }
            // Draw game sprites in any state.
            else if (_controller.GameState.CurrentState == State.Game ||
                _controller.GameState.CurrentState == State.Paused ||
                _controller.GameState.CurrentState == State.GameOver)
            {
                _controller.Draw(gameTime, _spriteBatch);
            }
            else if (_controller.GameState.CurrentState == State.Hits)
            {
                _hitList.Draw(_spriteBatch, gameTime);
            }
            else if (_controller.GameState.CurrentState == State.UserInput)
            {
                _userInput.Draw(_spriteBatch, gameTime, _controller.GameState.CurrentScore.ToString());
            }
            else
            {
                Debug.Assert(false);
            }

            // End batch.
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        private void _PauseGameIfNeeded()
        {
            if (_controller.IsGameFinished)
                return;

            if (_keyboardState.IsKeyDown(Keys.P))
            {
                _controller.GameState.IsPausePressed = true;
            }
            else if (_controller.GameState.IsPausePressed)
            {
                _controller.GameState.IsPausePressed = false;
                _controller.GameState.CurrentState = (_controller.GameState.CurrentState == State.Game) ? State.Paused : State.Game;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void _UpdateMenu(GameTime gameTime)
        {
            _mouseCursor.UpdateMouse(_menu);
            _menu.Update(_keyboardState, _mouseCursor);
            var _mouseState = Mouse.GetState();

            // Update keys.
            if (_keyboardState.IsKeyDown(Keys.Enter) ||
                (_mouseState.LeftButton == ButtonState.Pressed &&
                _menu.GetMenuItemIntersectedByMouse(_mouseCursor) != -1))
            {
                if (_menu.SelectedItem == (int)MenuState.Start)
                {
                    _controller.GameState.CurrentState = State.Game;
                    _controller.Initialize();
                    base.Initialize();
                }
                else if (_menu.SelectedItem == (int)MenuState.Exit)
                    this.Exit();
                else if (_menu.SelectedItem == (int)MenuState.Hits)
                    _controller.GameState.CurrentState = State.Hits;
            }
            else if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                int clickedLevel = _menu.GetVolumeItemIntersectedByMouse(_mouseCursor);

                if (clickedLevel > -1)
                {
                    _menu.VolumeLevel = clickedLevel;

                    AudioPlayer.Volume = clickedLevel * 0.2f;
                }
            }

            // Update audio for Start application (menu start): it should play only once,
            // so we need to find exact time where we can start.
            if (GameHelper.IsItNearValues(gameTime.TotalGameTime, _span) && !_menuSoudStarted)
            {
                AudioPlayer.Stop();
                AudioPlayer.PlayMenu();
                _menuSoudStarted = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void _UpdateGame(GameTime gameTime)
        {
            _PauseGameIfNeeded();

            _controller.Update(gameTime, _keyboardState);

            if (!_gameSoudStarted)
            {
                AudioPlayer.Stop();
                AudioPlayer.PlayGame();
            }

            _menuSoudStarted = false;
            _gameSoudStarted = true;
        }

        #endregion

        #region Private fields

        private GameController _controller;

        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        private KeyboardState _keyboardState;

        private Menu _menu;

        private HitsList _hitList;

        private UserInput _userInput;

        private MouseCursor _mouseCursor;

        TimeSpan _span;

        bool _menuSoudStarted = false;
        bool _gameSoudStarted = false;

        bool _isInited = false;

        #endregion
    }
}
