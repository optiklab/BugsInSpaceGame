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
using Bugs.Sprites;

namespace Bugs.Display
{
    internal class ControlPanel : BasicSprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ControlPanel() : base()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="stringTexture"></param>
        public override void Load(ContentManager content)
        {
            SpriteTexture = content.Load<Texture2D>("TV");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteTexture, new Vector2(0, 0), Color.White);
        }

        #endregion
    }
}
