using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace CreateListItemWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            var siteUrl = ConfigurationManager.AppSettings["siteUrl"];
            var listName = ConfigurationManager.AppSettings["listName"];

            var securePassword = new SecureString();
            foreach (var c in password.ToCharArray()) securePassword.AppendChar(c);

            var clientContext = new ClientContext(siteUrl);
            clientContext.Credentials = new SharePointOnlineCredentials(username, securePassword);

            var scheduleList = clientContext.Web.Lists.GetByTitle(listName);
            clientContext.Load(scheduleList);

            var listItemCreationInfo = new ListItemCreationInformation();
            var listItem = scheduleList.AddItem(listItemCreationInfo);
            
            listItem["Title"] = "WebJob " + now;

            listItem.Update();
            clientContext.ExecuteQuery();

        }
    }
}
