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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Bugs
{
    internal static class AudioPlayer
    {
        #region Public properties

        /// <summary>
        /// 
        /// </summary>
        public static float Volume
        {
            get
            {
                return MediaPlayer.Volume;
            }

            set
            {
                MediaPlayer.Volume = value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public static void Load(ContentManager content)
        {
            _startMenu = content.Load<SoundEffect>("start");
            _shipSimpleShoot = content.Load<SoundEffect>("Pow1");
            _enemySimpleShoot = content.Load<SoundEffect>("Pow2");
            _bomb = content.Load<SoundEffect>("Bomb");
            _enemyDestroyed = content.Load<SoundEffect>("EnemyDestroyed");
            _enemySuperShoot = content.Load<SoundEffect>("Pow3");
            _shipSuperShoot = content.Load<SoundEffect>("Pow4");
            _menuSong = content.Load<Song>("Untitled");
            _gameSong = content.Load<Song>("game");
            _win = content.Load<SoundEffect>("Win");
            _liveBonus = content.Load<SoundEffect>("LiveBonusSound");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayShipSimpleShoot()
        {
            _shipSimpleShoot.Play(Volume, 0, 0);
            return _shipSimpleShoot.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayEnemySimpleShoot()
        {
            _enemySimpleShoot.Play(Volume, 0, 0);

            return _enemySimpleShoot.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayBomb()
        {
            _bomb.Play(Volume, 0, 0);

            return _bomb.Duration;
        }

        public static TimeSpan PlayWin()
        {
            _win.Play(Volume, 0, 0);

            return _win.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayEnemyDestroyed()
        {
            _enemyDestroyed.Play(Volume, 0, 0);

            return _enemyDestroyed.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayShipSuperShoot()
        {
            _shipSuperShoot.Play(Volume, 0, 0);

            return _shipSuperShoot.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan PlayEnemySuperShoot()
        {
            _enemySuperShoot.Play(Volume, 0, 0);

            return _enemySuperShoot.Duration;
        }

        /// <summary>
        /// Play menu start effect.
        /// </summary>
        public static TimeSpan PlayMenuStart()
        {
            _startMenu.Play(Volume, 0, 0);

            return _startMenu.Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan LiveBonus()
        {
            _liveBonus.Play(Volume, 0, 0);

            return _liveBonus.Duration;
        }

        /// <summary>
        /// Play menu music.
        /// </summary>
        public static void PlayMenu()
        {
            MediaPlayer.Play(_menuSong);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Play game music.
        /// </summary>
        public static void PlayGame()
        {
            MediaPlayer.Play(_gameSong);
        }

        #endregion

        #region Private members

        private static SoundEffect _startMenu;

        private static SoundEffect _shipSimpleShoot;

        private static SoundEffect _enemySimpleShoot;

        private static SoundEffect _shipSuperShoot;

        private static SoundEffect _enemySuperShoot;

        private static SoundEffect _bomb;

        private static SoundEffect _win;

        private static SoundEffect _enemyDestroyed;

        private static SoundEffect _liveBonus;

        private static Song _menuSong;

        private static Song _gameSong;

        #endregion
    }
}
