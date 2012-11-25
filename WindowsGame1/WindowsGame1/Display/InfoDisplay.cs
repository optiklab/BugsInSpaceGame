/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bugs.Display
{
    /// <summary>
    /// 
    /// </summary>
    internal class InfoDisplay
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public InfoDisplay()
        {
            _levelInfoString = new BasicInfoString("Level:");
            _pointsInfoString = new BasicInfoString("Score:");
            _pauseInfoString = new BasicInfoString("PAUSED");
            _gameOverInfoString = new BasicInfoString("GAME OVER");
            _helpInfoString = new BasicInfoString("Press 'P' for pause");
            _exitInfoString = new BasicInfoString("Press 'Esc' to exit");
            _livesInfoString = new BasicInfoString("Lives:");
            _overheatingInfoString = new BasicInfoString("OVERHEAT!");
            _statusOverheatingInfoString = new BasicInfoString(TIME_FOR_COOLING_TEXT);
            _nextLevelInfoString = new BasicInfoString(NEXT_LEVEL_STATUS_TEXT);

            _level1InfoString = new BasicInfoString("LEVEL 1");
            _level2InfoString = new BasicInfoString("LEVEL 2");
            _level3InfoString = new BasicInfoString("LEVEL 3");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ContentManager content, Rectangle clientBounds)
        {
            // Load font
            _myFont = content.Load<SpriteFont>("Arial");

            // Info show in right top corner.
            _levelInfoString.StringPosition = new Vector2(clientBounds.Width - RIGHT_DISPLAY_BORDER * 2, 25);
            _pointsInfoString.StringPosition = new Vector2(clientBounds.Width - RIGHT_DISPLAY_BORDER, 25);
            _exitInfoString.StringPosition = new Vector2(clientBounds.Width - 405, 15);
            _helpInfoString.StringPosition = new Vector2(clientBounds.Width - 400, 35);
            _livesInfoString.StringPosition = new Vector2(40, 25);

            // Pause type in center
            _pauseInfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);
            _gameOverInfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);

            _overheatingInfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);

            _statusOverheatingInfoString.StringPosition = new Vector2(
                clientBounds.Width - 160, clientBounds.Height - 35);

            _nextLevelInfoString.StringPosition = new Vector2(
                clientBounds.Width - 160, clientBounds.Height - 35);

            _level1InfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);
            _level2InfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);
            _level3InfoString.StringPosition = new Vector2(clientBounds.Width / 2,
                clientBounds.Height / 2);

            // Find center of the string
            _pauseInfoString.StringOrigin = _myFont.MeasureString(_pauseInfoString.OutString) / 2;
            _helpInfoString.StringOrigin = _myFont.MeasureString(_helpInfoString.OutString) / 2;
            _livesInfoString.StringOrigin = _myFont.MeasureString(_livesInfoString.OutString) / 2;
            _exitInfoString.StringOrigin = _myFont.MeasureString(_exitInfoString.OutString) / 2;
            _levelInfoString.StringOrigin = _myFont.MeasureString(_levelInfoString.OutString) / 2;
            _pointsInfoString.StringOrigin = _myFont.MeasureString(_pointsInfoString.OutString) / 2;
            _gameOverInfoString.StringOrigin = _myFont.MeasureString(_gameOverInfoString.OutString) / 2;
            _overheatingInfoString.StringOrigin = _myFont.MeasureString(_overheatingInfoString.OutString) / 2;
            _nextLevelInfoString.StringOrigin = _myFont.MeasureString(_nextLevelInfoString.OutString) / 2;
            _level1InfoString.StringOrigin = _myFont.MeasureString(_level1InfoString.OutString) / 2;
            _level2InfoString.StringOrigin = _myFont.MeasureString(_level2InfoString.OutString) / 2;
            _level3InfoString.StringOrigin = _myFont.MeasureString(_level3InfoString.OutString) / 2;
            _statusOverheatingInfoString.StringOrigin = _myFont.MeasureString(_statusOverheatingInfoString.OutString) / 2;
        }

        public void UpdateInfo(int nextLevelInfo, uint gotPoints)
        {
            _nextLevelInfoString.OutString = string.Format(NEXT_LEVEL_STATUS_TEXT, nextLevelInfo - gotPoints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="gameState"></param>
        public void DrawInfo(GameTime gameTime, SpriteBatch spriteBatch, GameState gameState)
        {
            // Show the strings.
            spriteBatch.DrawString(_myFont, _levelInfoString.OutString + " " + gameState.CurrentLevel, _levelInfoString.StringPosition, Color.White,
                0, _levelInfoString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_myFont, _pointsInfoString.OutString + " " + gameState.CurrentScore, _pointsInfoString.StringPosition, Color.White,
                0, _pointsInfoString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_myFont, _livesInfoString.OutString, _livesInfoString.StringPosition, Color.White,
                0, _livesInfoString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_myFont, _helpInfoString.OutString, _helpInfoString.StringPosition, Color.White,
                0, _helpInfoString.StringOrigin, 0.8f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_myFont, _exitInfoString.OutString, _exitInfoString.StringPosition, Color.White,
                0, _exitInfoString.StringOrigin, 0.8f, SpriteEffects.None, 0.5f);

            if (gameState.CurrentState == State.Paused)
                _Pause(spriteBatch);
            else if (gameState.CurrentState == State.GameOver)
                _GameOver(spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="secondsForCooling"></param>
        public void OverheatStatus(SpriteBatch spriteBatch, int secondsForCooling)
        {
            spriteBatch.DrawString(_myFont, _overheatingInfoString.OutString, _overheatingInfoString.StringPosition, Color.Yellow,
                0, _overheatingInfoString.StringOrigin, 3.0f, SpriteEffects.None, 0.5f);

            string toShow = string.Format(TIME_FOR_COOLING_TEXT, secondsForCooling);
            _statusOverheatingInfoString.OutString = toShow;
            spriteBatch.DrawString(_myFont, _statusOverheatingInfoString.OutString,
                _statusOverheatingInfoString.StringPosition, Color.White, 0, _statusOverheatingInfoString.StringOrigin,
                1.0f, SpriteEffects.None, 0.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void NextLevelStatus(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_myFont, _nextLevelInfoString.OutString, _nextLevelInfoString.StringPosition, Color.White,
                0, _nextLevelInfoString.StringOrigin, 0.8f, SpriteEffects.None, 0.5f);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="spriteBatch"></param>
        public void LevelCameUp(uint level, SpriteBatch spriteBatch)
        {
            if (level == 1)
            {
                spriteBatch.DrawString(_myFont, _level1InfoString.OutString, _level1InfoString.StringPosition, Color.Red,
                    0, _level1InfoString.StringOrigin, 3.0f, SpriteEffects.None, 0.5f);
            }
            else if (level == 2)
            {
                spriteBatch.DrawString(_myFont, _level2InfoString.OutString, _level2InfoString.StringPosition, Color.Red,
                    0, _level2InfoString.StringOrigin, 3.0f, SpriteEffects.None, 0.5f);
            }
            else if (level == 3)
            {
                spriteBatch.DrawString(_myFont, _level3InfoString.OutString, _level3InfoString.StringPosition, Color.Red,
                    0, _level3InfoString.StringOrigin, 3.0f, SpriteEffects.None, 0.5f);
            }
            else
            {
                // do nothing.
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// /
        /// </summary>
        private void _Pause(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_myFont, _pauseInfoString.OutString, _pauseInfoString.StringPosition, Color.Green,
                0, _pauseInfoString.StringOrigin, 4.0f, SpriteEffects.None, 0.5f);
        }

        private void _GameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_myFont, _gameOverInfoString.OutString, _gameOverInfoString.StringPosition, Color.Red,
                0, _gameOverInfoString.StringOrigin, 4.0f, SpriteEffects.None, 0.5f);
        }

        #endregion

        #region Private constants

        /// <summary>
        /// 
        /// </summary>
        private const int DISTANCE_BETWEEN_STRINGS = 50;

        /// <summary>
        /// 
        /// </summary>
        private const int TOP_DISPLAY_BORDER = 100;

        /// <summary>
        /// 
        /// </summary>
        private const int LEFT_DISPLAY_BORDER = 100;

        /// <summary>
        /// 
        /// </summary>
        private const int RIGHT_DISPLAY_BORDER = 100;

        /// <summary>
        /// 
        /// </summary>
        private const int BOTTOM_DISPLAY_BORDER = 140;

        private const string NEXT_LEVEL_STATUS_TEXT = "Take {0} points to get next level!";

        private const string TIME_FOR_COOLING_TEXT = "Wait {0} seconds for cooling!";

        #endregion

        #region Private fields

        // Font container.
        private SpriteFont _myFont;

        /// <summary>
        /// Strings.
        /// </summary>
        private BasicInfoString _livesInfoString;
        private BasicInfoString _levelInfoString;
        private BasicInfoString _pointsInfoString;
        private BasicInfoString _exitInfoString;
        private BasicInfoString _helpInfoString;
        private BasicInfoString _pauseInfoString;
        private BasicInfoString _gameOverInfoString;

        private BasicInfoString _overheatingInfoString;
        private BasicInfoString _statusOverheatingInfoString;
        private BasicInfoString _nextLevelInfoString;

        private BasicInfoString _level1InfoString;
        private BasicInfoString _level2InfoString;
        private BasicInfoString _level3InfoString;

        #endregion
    }
}
