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
using System.Xml;
using System.Xml.Serialization;

namespace Bugs
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("HitsList")]
    public class UserDataCollection
    {
        #region Constructor

        public UserDataCollection()
        {
            Collection = new List<UserData>();
        }

        #endregion

        #region Public properties

        [XmlElement("userdata")]
        public List<UserData> Collection;

        #endregion
    }
}
