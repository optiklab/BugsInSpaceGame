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
using Microsoft.Xna.Framework.Input;
using Bugs.Sprites;

namespace Bugs.Characters
{
    class Rocket : AnimatedSprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public Rocket() : this(3, 2)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Rocket(int frameCount, int framesPerSecond) :
            base(frameCount, framesPerSecond)
        {
            Speed = new Vector2(0, 10);
            DefaultSpeed = Speed;
            MaxSpeed = new Vector2(0, 10);
            BoundingBox = new BoundingBox();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            SpriteTexture = content.Load<Texture2D>("Pulka");
        }

        #endregion
    }
}
