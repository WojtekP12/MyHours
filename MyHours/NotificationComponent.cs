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
        public void RegisterNotification(int userID)
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
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");


                //re-register notification
                RegisterNotification(1);
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