/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bugs.Sprites
{
    /// <summary>
    /// 
    /// </summary>
    internal class AnimatedSprite : Sprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public AnimatedSprite(int frameCount, int framesPerSecond) :
            base()
        {
            _frameCount = frameCount;
            _timeForFrame = 1f / framesPerSecond;
            _currentFrameNumber = 0;
            _timeElapsed = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void DrawSprite(SpriteBatch spriteBatch)
        {
            int frameWidth = SpriteTexture.Width / _frameCount;

            Rectangle rect = new Rectangle(
                frameWidth * _currentFrameNumber,
                0, frameWidth, SpriteTexture.Height);

            spriteBatch.Draw(SpriteTexture, SpritePosition, rect, Color.White);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        public void UpdateFrame(double elapsed)
        {
            _timeElapsed += elapsed;

            if (_timeElapsed > _timeForFrame)
            {
                // Show next frame.
                _currentFrameNumber++;
                _currentFrameNumber = _currentFrameNumber % (_frameCount - 1);
                _timeElapsed -= _timeForFrame;
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// 
        /// </summary>
        private int _frameCount;

        /// <summary>
        /// 
        /// </summary>
        private double _timeForFrame;

        /// <summary>
        /// 
        /// </summary>
        private int _currentFrameNumber;

        /// <summary>
        /// 
        /// </summary>
        private double _timeElapsed;

        #endregion
    }
}
