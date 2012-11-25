/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Bugs.Display;


namespace Bugs.Display
{
    /// <summary>
    /// Hits list describes a page to show user scores history.
    /// Class should be public since it contains a collection of history to be saved.
    /// </summary>
    public class HitsList
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public HitsList()
        {
            _hitItem = new BasicMenuItem(HitItemPosition);
            _menuPosition = new Vector2(0, 0);
            _userData = new UserDataCollection();
            DeserializeHistory();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            Debug.Assert(content != null);

            // Load font
            _myFont = content.Load<SpriteFont>("Arial");

            _background = content.Load<Texture2D>("space_hd");
            _hitItem.ItemTexture = content.Load<Texture2D>("HitsItemWhite");

            _escHelpString.StringPosition = new Vector2(100, 30);
            _escHelpString.StringOrigin = _myFont.MeasureString(_escHelpString.OutString) / 2;

            _producedByString.StringPosition = new Vector2(512, 730);
            _producedByString.StringOrigin = _myFont.MeasureString(_producedByString.OutString) / 2;

            _questionsHelpString.StringPosition = new Vector2(512, 600);
            _questionsHelpString.StringOrigin = _myFont.MeasureString(_questionsHelpString.OutString) / 2;

            _webSiteHelpString.StringPosition = new Vector2(512, 630);
            _webSiteHelpString.StringOrigin = _myFont.MeasureString(_webSiteHelpString.OutString) / 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameState"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Debug.Assert(spriteBatch != null);
            Debug.Assert(gameTime != null);

            spriteBatch.Draw(_background, _menuPosition, Color.White);

            spriteBatch.Draw(_hitItem.ItemTexture, _hitItem.ItemPosition, Color.Yellow);

            spriteBatch.DrawString(_myFont, _escHelpString.OutString, _escHelpString.StringPosition, Color.White,
                0, _escHelpString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(_myFont, _webSiteHelpString.OutString, _webSiteHelpString.StringPosition, Color.Yellow,
                0, _webSiteHelpString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(_myFont, _questionsHelpString.OutString, _questionsHelpString.StringPosition, Color.White,
                0, _questionsHelpString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(_myFont, _producedByString.OutString, _producedByString.StringPosition, Color.White,
                0, _producedByString.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            foreach (var item in _visibleItems)
            {
                spriteBatch.DrawString(_myFont, item.OutString, item.StringPosition, Color.White,
                    0, item.StringOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }
        }

        /// <summary>
        /// Load content for Hits List one by one: header, items.
        /// </summary>
        /// <param name="keyboardState"></param>
        public void Update(KeyboardState keyboardState)
        {
            Debug.Assert(keyboardState != null);

            _visibleItems.Clear();
            int index = 0;
            foreach (var item in _userData.Collection)
            {
                string screenItem = _GetScreenItem(item);
                var newItem = new BasicInfoString(screenItem);
                newItem.StringPosition = new Vector2(550, 200 + index * 40);
                newItem.StringOrigin = _myFont.MeasureString(newItem.OutString) / 2;
                _visibleItems.Add(newItem);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SerializeHistory()
        {
            try
            {
                XmlSerializer l = new XmlSerializer(typeof(UserDataCollection));
                string path = System.Environment.CurrentDirectory + PATH;
                TextWriter k = new StreamWriter(path);
                l.Serialize(k, _userData);
                k.Close();
            }
            catch (Exception)
            {
                // To Logger.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        public void AddUserData(UserData userData)
        {
            _userData.Collection.Add(userData);

            _ValidateAndFixUserData();

            // Sort by Creation Time
            _userData.Collection.Sort(delegate(UserData u1, UserData u2)
            {
                int value1 = Convert.ToInt32(u1.Score);
                int value2 = Convert.ToInt32(u2.Score);
                return value2.CompareTo(value1);
            });
        }

        #endregion

        #region Private methods

        private void _ValidateAndFixUserData()
        {
            // Sort by Creation Time
            var collection = new List<UserData>();
            foreach (var item in _userData.Collection)
            {
                //var newItem = new UserData() { Score = item.Score, CreationTime = item.CreationTime, UserName = item.UserName };
                collection.Add(item);
            }

            collection.Sort(delegate(UserData u1, UserData u2)
            {
                return u1.CreationTime.CompareTo(u2.CreationTime);
            });

            // Take all except newest 10.
            var col = collection.Take(collection.Count - 10);

            // Remove others
            foreach (var item in col)
            {
                if (col.Contains(item))
                    _userData.Collection.Remove(item);
            }
            //int index = UserData.Collection.Count - 1; // Last one.
            //while (UserData.Collection.Count > 10)
            //    UserData.Collection.RemoveAt(index--);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        private string _GetScreenItem(UserData userData)
        {
            if (userData.UserName.Length > 30)
                userData.UserName = userData.UserName.Substring(0, 30);

            if (userData.Score.Length > 4)
                userData.Score = userData.Score.Substring(0, 4);

            // Add points to user name if name less than 30 symbols length, format: <NAME><points>
            string temp = string.Empty;
            int tempNameLenght = 30 - userData.UserName.Length;
            for (int i = 0; i < tempNameLenght; i++)
                temp += ".";
            string name = userData.UserName + temp;

            // Add points to score if it is less than 4 symbols length, format: <points><SCORE>
            temp = string.Empty;
            int tempScoreLenght = 4 - userData.Score.Length;
            for (int i = 0; i < tempScoreLenght; i++)
                temp += ".";
            string score = temp + userData.Score;

            // Add points between <NAME> and <SCORE>
            int tempLength = 50 - name.Length - score.Length;
            temp = string.Empty;
            for (int i = 0; i < 16 /* 50 - 30<FOR NAME> - 4<FOR SCORE>*/; i++)
                temp += ".";

            return (name + temp + score);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeserializeHistory()
        {
            string path = System.Environment.CurrentDirectory + PATH;

            try
            {
                FileInfo file = new FileInfo(path);

                // If file doesn't exists, create new empty one.
                if (!file.Exists)
                {
                    XmlSerializer l = new XmlSerializer(typeof(UserDataCollection));
                    TextWriter k = new StreamWriter(path);
                    l.Serialize(k, new UserDataCollection());
                    k.Close();
                }

                // Read file, if exsts (already exists =) ).
                if (file.Exists)
                {
                    XmlSerializer s = new XmlSerializer(typeof(UserDataCollection));
                    TextReader r = new StreamReader(path);
                    _userData = (UserDataCollection)s.Deserialize(r);
                    r.Close();
                    _ValidateAndFixUserData();
                }
            }
            catch (Exception)
            {
                // To logger
            }
        }

        #endregion

        #region Private constants

        /// <summary>
        /// 
        /// </summary>
        private readonly Vector2 HitItemPosition = new Vector2(370, 30);

        private const string PATH = "\\history.xml";

        #endregion

        #region Private fields

        // Font container.
        private SpriteFont _myFont;

        private Texture2D _background;
        private Vector2 _menuPosition;

        private BasicMenuItem _hitItem;

        private List<BasicInfoString> _visibleItems = new List<BasicInfoString>();

        private BasicInfoString _escHelpString = new BasicInfoString("'Esc' to main menu");
        private BasicInfoString _producedByString = new BasicInfoString("Game produced by OptikLab (c) 2011");
        private BasicInfoString _questionsHelpString = new BasicInfoString("You are welcome with your questions and suggestions on my web-site:");
        private BasicInfoString _webSiteHelpString = new BasicInfoString("http://www.ayarkov.com");

        private UserDataCollection _userData;

        #endregion
    }
}
