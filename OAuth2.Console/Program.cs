using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL.OAuth2;
using HL.OAuth2.Client;
using HL.OAuth2.Client.Impl;

namespace HL.OAuth2.Console
{
    class Program
    {
        static string token = null;

        static void Main(string[] args)
        {
            try
            {
                var p = new Program();
                token = p.TestGetToken();
                p.TestRegisterDevice();
                p.TestCreateNotification();
                p.TestDeregisterDevice();
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

            var client = (MpnsClient) authorizationRoot.Clients.Where(klient => klient.Name.Equals("MPNS")).First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["grant_type"] = "client_credentials";
            var accessToken = client.GetToken(queryParams);

            DateTime now = DateTime.Now;

            System.Console.WriteLine("TestGetToken");
            System.Console.WriteLine("Token: " + accessToken);
            System.Console.WriteLine(string.Format("Time now: {0}, Expires at: {1}, Expiring in {2} second(s)", now, client.ExpiresAt, (client.ExpiresAt - now).TotalSeconds));

            return accessToken;
        }

        public void TestRegisterDevice()
        {
            var authorizationRoot = new AuthorizationRoot();

            var client = (MpnsClient)authorizationRoot.Clients.Where(klient => klient.Name.Equals("MPNS")).First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = token;
            queryParams["username"] = "mpns1";
            queryParams["id"] = "token1";
            queryParams["msg_service"] = "apns";
            var results = client.RegisterDevice(queryParams);
            System.Console.WriteLine(string.Format("TestRegisterDevice::Response : {0}", results));
        }

        public void TestDeregisterDevice()
        {
            var authorizationRoot = new AuthorizationRoot();

            var client = (MpnsClient)authorizationRoot.Clients.Where(klient => klient.Name.Equals("MPNS")).First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = token;
            queryParams["username"] = "mpns1";
            queryParams["id"] = "token1";
            queryParams["msg_service"] = "apns";
            var results = client.DeregisterDevice(queryParams);
            System.Console.WriteLine(string.Format("TestDeregisterDevice::Response : {0}", results));
        }

        public void TestCreateNotification()
        {
            var authorizationRoot = new AuthorizationRoot();

            var client = (MpnsClient)authorizationRoot.Clients.Where(klient => klient.Name.Equals("MPNS")).First();
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = token;
            queryParams["username"] = "mpns1";
            queryParams["notification_type"] = "SetReminderNotification";
            queryParams["contact"] = "Win10x64DotNetDev";
            var results = client.CreateNotification(queryParams);
            System.Console.WriteLine(string.Format("TestDeregisterDevice::Response : {0}", results));
        }
    }
}
