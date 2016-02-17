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
                var token = p.TestGetToken();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:" + ex.Message + "\n" + ex.StackTrace);
            }
            System.Console.WriteLine("\nDone.");
            System.Console.ReadKey();
        }

        public string TestGetToken()
        {
            var authorizationRoot = new AuthorizationRoot();

            var client = (Client.OAuth2Client) authorizationRoot.Clients.Where(klient => klient.Name.Equals("MPNS")).First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["grant_type"] = "client_credentials";
            var token = client.GetToken(queryParams);

            DateTime now = DateTime.Now;

            System.Console.WriteLine("TestGetToken");
            System.Console.WriteLine("Token: " + token);
            System.Console.WriteLine(string.Format("Time now: {0}, Expires at: {1}, Expiring in {2} second(s)", now, client.ExpiresAt, (client.ExpiresAt - now).TotalSeconds));

            return token;
        }
    }
}
