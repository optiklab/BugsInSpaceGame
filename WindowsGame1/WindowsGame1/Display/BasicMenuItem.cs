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
    internal class BasicMenuItem
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public BasicMenuItem(Vector2 position)
        {
            ItemPosition = position;
            _box = new BoundingBox();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Position of Item.
        /// </summary>
        public Vector2 ItemPosition
        { get; set; }

        /// <summary>
        /// Item Texture.
        /// </summary>
        public Texture2D ItemTexture
        { get; set; }

        public BoundingBox Box
        {
            get { return _box; }
            set { _box = value; }
        }

        #endregion

        #region Private fields

        private BoundingBox _box;

        #endregion
    }
}
