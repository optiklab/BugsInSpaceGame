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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bugs.Sprites
{
    /// <summary>
    /// 
    /// </summary>
    internal class Sprite : BasicSprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public Sprite() : base()
        {
            Speed = new Vector2(0, 3);
            DefaultSpeed = Speed;
            MaxSpeed = new Vector2(0, 5);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public BoundingBox BoundingBox
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 SpritePosition;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Speed;

        public Vector2 DefaultSpeed;

        public Vector2 MaxSpeed;

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            SpriteTexture = content.Load<Texture2D>("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="stringTexture"></param>
        public void Load(ContentManager content, String stringTexture)
        {
            SpriteTexture = content.Load<Texture2D>(stringTexture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteTexture, SpritePosition, Color.White);
        }

        #endregion
    }
}
