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
    internal static class GameHelper
    {
        public static bool IsItNearValues(TimeSpan span1, TimeSpan span2)
        {
            bool result = true;
            TimeSpan default1 = new TimeSpan(0, 0, 1);

            if (span2 > span1)
            {
                result = (span2 - span1) <= default1;
            }
            else if (span2 < span1)
            {
                result = (span1 - span2) <= default1;
            }

            return result;
        }
    }
}
