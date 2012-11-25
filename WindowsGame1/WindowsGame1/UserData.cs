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
using System.Xml;
using System.Xml.Serialization;

namespace Bugs
{
    /// <summary>
    /// Class container for user data, which needs to be saved.
    /// </summary>
    public class UserData
    {
        [XmlAttribute("UserName")]
        public string UserName
        {
            get;
            set;
        }

        [XmlAttribute("Score")]
        public string Score
        {
            get;
            set;
        }

        [XmlAttribute("CreationTime")]
        public DateTime CreationTime
        {
            get;
            set;
        }
    }
}
