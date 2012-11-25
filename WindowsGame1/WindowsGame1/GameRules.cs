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
    internal class GameRules
    {
        #region Constructor

        public GameRules()
        {
            LevelsPoints = new List<int>();
            LevelsPoints.Add(0);
            // Test third level
            //LevelsPoints.Add(50);
            //LevelsPoints.Add(100);
            //LevelsPoints.Add(200);
            //LevelsPoints.Add(300);

            // Front end
            LevelsPoints.Add(250);
            LevelsPoints.Add(1000);
            LevelsPoints.Add(2000);
            LevelsPoints.Add(3000);

            GameRules.DefaultPointsPerBonus = 20;
            GameRules.DefaultPointsPerEnemy = 10;
            GameRules.DefaultPointsPerMeteor = 1;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public List<int> LevelsPoints
        { get; set; }

        #endregion

        #region Game settings public properties

        public static uint DefaultPointsPerBonus
        {
            get;
            private set;
        }

        public static uint DefaultPointsPerEnemy
        {
            get;
            private set;
        }

        public static uint DefaultPointsPerMeteor
        {
            get;
            private set;
        }

        #endregion
    }
}
