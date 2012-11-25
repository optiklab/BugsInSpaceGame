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
    internal class VolumeControl
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public VolumeControl(ContentManager content, Vector2 position)
        {
            ItemPosition = position;

            _textures[0] = content.Load<Texture2D>("00");
            _textures[1] = content.Load<Texture2D>("112");
            _textures[2] = content.Load<Texture2D>("22");
            _textures[3] = content.Load<Texture2D>("33");
            _textures[4] = content.Load<Texture2D>("44");
            _textures[5] = content.Load<Texture2D>("55");

            Boxes = new BoundingBox[6];

            for (int i = 0; i < 6; i++)
            {
                Boxes[i] = new BoundingBox();

                Boxes[i].Min = new Vector3(position.X + i * 24 + i * 8, position.Y, 0);
                Boxes[i].Max = new Vector3(position.X + (i + 1) * 24 + i * 8, position.Y + 24, 0);
            }

            _volumeLevel = 3;

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
        {
            get
            {
                return _textures[VolumeLevel];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BoundingBox[] Boxes
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
                return _volumeLevel;
            }
            set
            {
                _volumeLevel = value;

                if (_volumeLevel > 5)
                    _volumeLevel = 5;

                if (_volumeLevel < 0)
                    _volumeLevel = 0;
            }
        }

        #endregion

        #region Private fields

        private Texture2D[] _textures = new Texture2D[6];

        private int _volumeLevel;

        #endregion
    }
}
