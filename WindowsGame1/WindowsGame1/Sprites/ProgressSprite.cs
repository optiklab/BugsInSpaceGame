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
    internal class ProgressSprite
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ProgressSprite()
        {
            Textures = new List<Texture2D>();
            SelectedItem = 0;
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
        public List<Texture2D> Textures;

        /// <summary>
        /// 
        /// </summary>
        public int SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == 10)
                    IsOverheat = true;

                if (IsOverheat && value < 7)
                    IsOverheat = false;

                _selectedItem = value;
            }
        }

        public bool IsOverheat
        {
            get;
            private set;
        }

        public double[] LevelsArray
        {
            get
            {
                return levelsArray;
            }
        }

        #endregion

        #region Public methods

        public void Initialize(Vector2 position)
        {
            SpritePosition = position;
            IsOverheat = false;
            SelectedItem = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            for (int i = 0; i < 11; i++)
                Textures.Add(content.Load<Texture2D>((i + 1).ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawSprite(SpriteBatch spriteBatch)
        {
            if (SelectedItem >= 0 && SelectedItem < Textures.Count)
                spriteBatch.Draw(Textures[SelectedItem], SpritePosition, Color.White);
        }

        public int GetTimeForCooling(double totalSeconds, int totalShoots)
        {
            if (IsOverheat)
            {
                double seconds = totalShoots / 0.94;

                return (int)(seconds - totalSeconds);
            }

            return 0; // 0 seconds for cooling
        }

        #endregion

        #region Private fields

        private int _selectedItem;

        //                                    { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1};
        //                                    { 0.1, 0.25, 0.4, 0.55, 0.7, 0.85, 1.0, 1.15, 1.3, 1.45, 1.6 };
        double[] levelsArray = new double[11] { 0.1, 0.22, 0.34, 0.46, 0.58, 0.7, 0.82, 0.94, 1.06, 1.18, 1.3 };

        #endregion
    }
}
