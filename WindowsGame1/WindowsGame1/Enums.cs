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

namespace Bugs
{
    /// <summary>
    /// 
    /// </summary>
    internal enum State
    {
        /// <summary>
        /// 
        /// </summary>
        Game,
        /// <summary>
        /// 
        /// </summary>
        Menu,
        /// <summary>
        /// 
        /// </summary>
        Paused,
        /// <summary>
        /// 
        /// </summary>
        GameOver,
        /// <summary>
        /// 
        /// </summary>
        Hits,
        /// <summary>
        /// 
        /// </summary>
        UserInput
    };

    /// <summary>
    /// 
    /// </summary>
    internal enum MenuState : int
    {
        /// <summary>
        /// 
        /// </summary>
        Start = 0,
        /// <summary>
        /// 
        /// </summary>
        Hits = 1,
        /// <summary>
        /// 
        /// </summary>
        Exit = 2
    };
}
