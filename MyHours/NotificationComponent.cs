using Microsoft.AspNet.SignalR;
using MyHours.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyHours
{
    public class NotificationComponent
    {
        public void RegisterNotification()
        {
            string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlCommand = @"SELECT [SenderID],[UserID],[Name],[Description],[StatusID],[Date] from [dbo].[USER_NOTIFICATION]"; //where [UserID] = @userID";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                //cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }

                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += SqlDep_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        private void SqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= SqlDep_OnChange;

                //from here we will send notification message to client
                NotifyClients(e.Info);

                //re-register notification
                RegisterNotification();
            }
        }

        private void NotifyClients(SqlNotificationInfo info)
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            switch (info)
            {
                case SqlNotificationInfo.Insert:
                    {
                        notificationHub.Clients.All.notify("added");
                        break;
                    }
                case SqlNotificationInfo.Delete:
                    {
                        notificationHub.Clients.All.notify("deleted");
                        break;
                    }
                default:
                    {
                        notificationHub.Clients.All.notify("other");
                        break;
                    }
            }
        }

        public List<USER_NOTIFICATION> GetNotifications(int jobId)
        {
            using (TAM_DBEntities dc = new TAM_DBEntities())
            {
                //return dc.USER_NOTIFICATION.Where(x => x.AddedOn > afterDate && x.ContactName == "A").OrderByDescending(x => x.AddedOn).ToList();
                return dc.USER_NOTIFICATION.ToList();
            }
        }
    }
}