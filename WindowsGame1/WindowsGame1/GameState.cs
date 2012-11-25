/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bugs
{
    internal class GameState
    {
        #region Constructor

        public GameState()
        {
            IsPausePressed = false;
            UserName = "Anonymous";
            CurrentLevel = 0;
            CurrentScore = 0;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Current game level.
        /// </summary>
        public uint CurrentLevel
        { get; set; }

        /// <summary>
        /// Score of the game.
        /// </summary>
        public uint CurrentScore
        { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string UserName
        { get; set; }

        /// <summary>
        /// Is pause pressed
        /// </summary>
        public bool IsPausePressed
        { get; set; }

        /// <summary>
        /// Is game over
        /// </summary>
        public bool IsGameOver
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public State CurrentState
        { get; set; }

        #endregion
    }
}
