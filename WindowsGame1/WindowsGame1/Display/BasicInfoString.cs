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
    /// Basic class which presents one informational text block for the game (to show game info).
    /// </summary>
    internal class BasicInfoString
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public BasicInfoString()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outString"></param>
        public BasicInfoString(string outString)
        {
            OutString = outString;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Position of string showing.
        /// </summary>
        public Vector2 StringPosition
        { get; set; }

        /// <summary>
        /// Center of the string.
        /// </summary>
        public Vector2 StringOrigin
        { get; set; }

        /// <summary>
        /// String to show.
        /// </summary>
        public string OutString
        { get; set; }

        #endregion
    }
}
