/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Bugs.Display;
using Bugs.Characters;

namespace Bugs.Display
{
    class UserInput
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public UserInput()
        {
            _hitItem = new BasicMenuItem(new Vector2(370, 30));
            _typeUserNameString = new BasicInfoString("Hey, space hero, type your name:");
            _scoreString = new BasicInfoString("Your score is ");

            _menuPosition = new Vector2(0, 0);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public bool IsInputFinished
        {
            get { return _isInputFinished; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return _currentInput; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Score
        {
            get { return _score; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            Debug.Assert(content != null);

            // Load font
            _myFont = content.Load<SpriteFont>("Arial");
            _background = content.Load<Texture2D>("space_hd");
            _hitItem.ItemTexture = content.Load<Texture2D>("HitsItemWhite");

            _typeUserNameString.StringPosition = new Vector2(512, 300);
            _typeUserNameString.StringOrigin = _myFont.MeasureString(_typeUserNameString.OutString) / 2;

            _scoreString.StringPosition = new Vector2(512, 200);
            _scoreString.StringOrigin = _myFont.MeasureString(_scoreString.OutString) / 2;

            _isInputFinished = false;

            _currentInput = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameState"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, string score)
        {
            Debug.Assert(spriteBatch != null);
            Debug.Assert(gameTime != null);

            spriteBatch.Draw(_background, _menuPosition, Color.White);

            spriteBatch.Draw(_hitItem.ItemTexture, _hitItem.ItemPosition, Color.Yellow);

            spriteBatch.DrawString(_myFont, _typeUserNameString.OutString, _typeUserNameString.StringPosition, Color.White,
                0, _typeUserNameString.StringOrigin, 1.5f, SpriteEffects.None, 0.5f);

            _score = score;
            string temp = _scoreString.OutString + score;
            spriteBatch.DrawString(_myFont, temp, _scoreString.StringPosition, Color.Yellow,
                0, _scoreString.StringOrigin, 1.8f, SpriteEffects.None, 0.5f);

            var newItem = new BasicInfoString(_currentInput);
            newItem.StringPosition = new Vector2(512, 400);
            newItem.StringOrigin = _myFont.MeasureString(newItem.OutString) / 2;
            spriteBatch.DrawString(_myFont, newItem.OutString, newItem.StringPosition, Color.White,
                0, newItem.StringOrigin, 2.0f, SpriteEffects.None, 0.5f);
        }

        /// <summary>
        /// Load content for Hits List one by one: header, items.
        /// </summary>
        /// <param name="keyboardState"></param>
        public void Update(KeyboardState keyboardState)
        {
            Debug.Assert(keyboardState != null);

            // Handle Enter
            if (keyboardState.IsKeyDown(Keys.Enter) && _currentInput.Length > 0)
            {
                _isInputFinished = true;
            }

            // Don't do anything if input finished.
            if (_isInputFinished)
                return;

            // Handle letters
            if (_isKeyLocked == 0 && _currentInput.Length < 20)
            {
                _isKeyLocked = 3;

                Keys[] keys = keyboardState.GetPressedKeys();

                if (keys.Length > 0)
                    _currentInput += KeysParser(keys);
            }
            else if (_isKeyLocked > 0)
            {
                _isKeyLocked--;
            }

            // Handle backspace
            if (keyboardState.IsKeyDown(Keys.Back)
                && _currentInput.Length > 0 && _isEscDown == 0)
            {
                _isEscDown = 3;
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            }
            else if (_isEscDown > 0)
            {
                _isEscDown--;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private string KeysParser(Keys[] keys)
        {
            string result = string.Empty;

            // Get only one symbol to do not type letters too fast =).
            if (keys.Length > 0)
            {
                string value = keys[0].ToString();

                if (value.Length == 1) // get only letters
                    result += value;
                else if (value == "Space") // or at least Space
                    result += " ";
            }

            return result;
        }

        #endregion

        #region Private fields

        private int _isKeyLocked = 0;
        private int _isEscDown = 0;
        private bool _isInputFinished = false;

        // Font container.
        private SpriteFont _myFont;

        private Texture2D _background;

        private Vector2 _menuPosition;

        private BasicMenuItem _hitItem;

        private BasicInfoString _typeUserNameString;
        private BasicInfoString _scoreString;
        private string _score;

        private string _currentInput = string.Empty;

        #endregion
    }
}
