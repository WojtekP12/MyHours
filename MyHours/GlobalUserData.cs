using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyHours
{
    public static class GlobalUserData
    {
        private static int notificationCount;
        public static int NotificationCount
        {
            get
            {
                return notificationCount;
            }
            set
            {
                notificationCount = value;
            }
        }

        public static void UpdateNotificationCount()
        {
            NotificationCount++;
        }
    }
}