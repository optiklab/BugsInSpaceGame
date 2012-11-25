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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Bugs.Sprites;

namespace Bugs.Display
{
    internal class MouseCursor
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public MouseCursor()
        {
            _mouse = new Sprite();

            _mouseBox = new BoundingBox();
            _centerBox = new BoundingBox();
            _cursorGame = new BoundingBox();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public BoundingBox MouseBox
        {
            get
            {
                return _mouseBox;
            }
            set
            {
                _mouseBox = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BoundingBox CenterBox
        {
            get
            {
                return _centerBox;
            }
            set
            {
                _centerBox = value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ContentManager content, GameWindow window)
        {
            _mouse.Load(content, "Pricel");
        }

        public void UpdateMouse(Menu menu)
        {
            _mouseState = Mouse.GetState();
            _mouse.SpritePosition.X = _mouseState.X;
            _mouse.SpritePosition.Y = _mouseState.Y;

            _mouseBox.Min = new Vector3(_mouse.SpritePosition.X, _mouse.SpritePosition.Y, 0);

            _mouseBox.Max = new Vector3(_mouse.SpritePosition.X + _mouse.SpriteTexture.Width,
                _mouse.SpritePosition.Y + _mouse.SpriteTexture.Height, 0);

            // Cursor is square
            float difX = (_mouseBox.Max.X - _mouseBox.Min.X) / 5;
            float difY = (_mouseBox.Max.Y - _mouseBox.Min.Y) / 5;

            // New box: this is small center part of all Cursor Square
            _centerBox.Min.X = _mouseBox.Min.X + 2 * difX;
            _centerBox.Max.X = _mouseBox.Max.X - 2 * difX;
            _centerBox.Min.Y = _mouseBox.Min.Y + 2 * difY;
            _centerBox.Max.Y = _mouseBox.Max.Y - 2 * difY;

            _cursorGame.Min = new Vector3(650, 400, 0);
            _cursorGame.Max = new Vector3(950, 500, 0);
        }

        public void DrawMouse(SpriteBatch spriteBatch)
        {
            _mouse.DrawSprite(spriteBatch);
        }

        #endregion

        #region Private fields

        private Sprite _mouse;

        private MouseState _mouseState;

        private BoundingBox _mouseBox;

        private BoundingBox _cursorGame;

        private BoundingBox _centerBox;

        #endregion
    }
}
