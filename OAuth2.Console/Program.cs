using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var p = new Program();
                p.TestLogin();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:" + ex.Message + "\n" + ex.StackTrace);
            }
            System.Console.WriteLine("\nDone.");
            System.Console.ReadKey();
        }

        public void TestLogin()
        {
            var authorizationRoot = new AuthorizationRoot();

            var client = (Client.OAuth2Client) authorizationRoot.Clients.First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["grant_type"] = "client_credentials";
            var token = client.GetToken(queryParams);

            System.Console.WriteLine("Token: " + token);
            System.Console.WriteLine("Expires at: " + client.ExpiresAt);
        }
    }
}
