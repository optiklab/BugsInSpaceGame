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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Bugs.Display;

namespace Bugs.Display
{
    /// <summary>
    /// 
    /// </summary>
    internal class Menu
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public Menu()
        {
            _menuPosition = new Vector2(0, 0);

            SelectedItem = 0;
            _items = new BasicMenuItem[3];
            _items[0] = new BasicMenuItem(new Vector2(400, 230));
            _items[1] = new BasicMenuItem(new Vector2(400, 330));
            _items[2] = new BasicMenuItem(new Vector2(400, 430));

            _lamps = new BasicMenuItem[3];
            _lamps[0] = new BasicMenuItem(new Vector2(300, 220));
            _lamps[1] = new BasicMenuItem(new Vector2(300, 320));
            _lamps[2] = new BasicMenuItem(new Vector2(300, 420));

            _lastSeconds = 0;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public int SelectedItem
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int VolumeLevel
        {
            get
            {
                return _volumeControl.VolumeLevel;
            }

            set
            {
                _volumeControl.VolumeLevel = value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("Backgr2");
            _items[0].ItemTexture = content.Load<Texture2D>("NewGameItemWhite");
            _items[1].ItemTexture = content.Load<Texture2D>("HitsItemWhite");
            _items[2].ItemTexture = content.Load<Texture2D>("ExitGameItemWhite");

            _lamps[0].ItemTexture = content.Load<Texture2D>("Button");
            _lamps[1].ItemTexture = content.Load<Texture2D>("Button");
            _lamps[2].ItemTexture = content.Load<Texture2D>("Button");

            _leftLight = content.Load<Texture2D>("Eagle2");
            _rightLight = content.Load<Texture2D>("RightBorder2");

            _volumeControl = new VolumeControl(content, new Vector2(780, 730));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameState"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw BACKGROUND
            spriteBatch.Draw(_background, _menuPosition, Color.White);

            if (gameTime.TotalGameTime.TotalSeconds > 1)
                spriteBatch.Draw(_leftLight, new Vector2(0, 0), Color.White);

            // Draw LAMPS ANIMATION
            _DrawLampsAnimation(spriteBatch, gameTime);

            for (int i = 0; i < _items.Length; i++)
            {
                if (i == SelectedItem)
                    spriteBatch.Draw(_items[i].ItemTexture, _items[i].ItemPosition, Color.Yellow);
                else
                    spriteBatch.Draw(_items[i].ItemTexture, _items[i].ItemPosition, Color.White);
            }

            // Draw LAMPS
            for (int i = 0; i < _lamps.Length; i++)
                spriteBatch.Draw(_lamps[i].ItemTexture, _lamps[i].ItemPosition, Color.White);

            // Draw VOLUME CONTROL
            spriteBatch.Draw(_volumeControl.ItemTexture, _volumeControl.ItemPosition, Color.Red);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(KeyboardState keyboardState, MouseCursor mouseCursor)
        {
            // 
            foreach (var item in _items)
            {
                BoundingBox box = new BoundingBox();
                box.Min = new Vector3(item.ItemPosition.X, item.ItemPosition.Y, 0);
                box.Max = new Vector3(item.ItemPosition.X + item.ItemTexture.Width,
                    item.ItemPosition.Y + item.ItemTexture.Height, 0);
                item.Box = box;
            }

            // Check mouse
            int index = GetMenuItemIntersectedByMouse(mouseCursor);
            if (index != -1 && _IsMousePositionChanged(mouseCursor))
            {
                SelectedItem = index;
                _latestCursorBox = mouseCursor.MouseBox;
                return;
            }

            // Check keyboard
            if (keyboardState.IsKeyDown(Keys.Up) && SelectedItem > 0 && _releasedUp)
            {
                --SelectedItem;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) &&
                SelectedItem < _items.Length - 1 &&
                _releasedDown)
            {
                ++SelectedItem;
            }

            _releasedDown = keyboardState.IsKeyUp(Keys.Down);
            _releasedUp = keyboardState.IsKeyUp(Keys.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseCursor"></param>
        /// <returns></returns>
        public int GetMenuItemIntersectedByMouse(MouseCursor mouseCursor)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Box.Intersects(mouseCursor.MouseBox))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseCursor"></param>
        /// <returns></returns>
        public int GetVolumeItemIntersectedByMouse(MouseCursor mouseCursor)
        {
            for (int i = 0; i < _volumeControl.Boxes.Length; i++)
            {
                if (_volumeControl.Boxes[i].Intersects(mouseCursor.CenterBox))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        private void _DrawLampsAnimation(SpriteBatch spriteBatch, GameTime gameTime)
        {
            double totalMilliSeconds = gameTime.TotalGameTime.TotalMilliseconds;
            if (totalMilliSeconds > _lastSeconds + 1500 &&
                totalMilliSeconds < _lastSeconds + 3000)
            {
                spriteBatch.Draw(_rightLight, new Vector2(511, 0), Color.White);
            }
            else if (totalMilliSeconds > _lastSeconds + 4000 &&
                totalMilliSeconds < _lastSeconds + 4200)
            {
                spriteBatch.Draw(_rightLight, new Vector2(511, 0), Color.White);
            }
            else if (totalMilliSeconds > _lastSeconds + 5000 &&
                totalMilliSeconds < _lastSeconds + 5200)
            {
                spriteBatch.Draw(_rightLight, new Vector2(511, 0), Color.White);
            }
            else if (totalMilliSeconds > _lastSeconds + 5200)
            {
                _lastSeconds = totalMilliSeconds;
            }
        }
        
        private bool _IsMousePositionChanged(MouseCursor mouseCursor)
        {
            return (mouseCursor.MouseBox.Max != _latestCursorBox.Max ||
                mouseCursor.MouseBox.Min != _latestCursorBox.Min);
        }

        #endregion

        #region Private fields

        private double _lastSeconds;

        private Texture2D _leftLight;
        private Texture2D _rightLight;

        private bool _releasedDown = true;
        private bool _releasedUp = true;

        private Texture2D _background;
        private Vector2 _menuPosition;

        private BasicMenuItem[] _items;

        private BasicMenuItem[] _lamps;

        private VolumeControl _volumeControl;

        // Remember latest cursor position to determine: if we need to LISTEN mouse changes,
        // or keyboard changes.
        private BoundingBox _latestCursorBox = new BoundingBox();

        #endregion
    }
}
