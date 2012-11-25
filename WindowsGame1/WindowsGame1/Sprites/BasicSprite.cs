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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bugs.Sprites
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class BasicSprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public BasicSprite()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public Texture2D SpriteTexture;

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="stringTexture"></param>
        public abstract void Load(ContentManager content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void DrawSprite(SpriteBatch spriteBatch);

        #endregion
    }
}
