using HL.OAuth2.Client.Impl;
using System;
using System.Linq;
using System.Collections.Specialized;

namespace HL.OAuth2.Client
{
    public class MpnsClientManager
    {
        private AuthorizationRoot _authorizationRoot;
        private const string ProviderName = "MPNS";
        private const string GrantType = "client_credentials";
        private MpnsClient _client;
        private static MpnsClientManager _mpnsClientManager;

        private AuthorizationRoot Root
        {
            get
            {
                return _authorizationRoot ?? new AuthorizationRoot();
            }
        }

        private static MpnsClientManager Instance
        {
            get
            {
                return _mpnsClientManager ?? (_mpnsClientManager = new MpnsClientManager());
            }
        }

        private MpnsClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = (MpnsClient) Root.Clients.Where(klient => klient.Name.Equals(ProviderName)).First();
                }

                if (_client == null)
                {
                    throw new Exception("MPNS client is not defined");
                }

                if (string.IsNullOrEmpty(_client.AccessToken) || _client.ExpiresAt < DateTime.Now)
                {
                    RefreshToken(_client);
                }
                return _client;
            }
        }

        public static void RegisterDevice(string username, string deviceToken, string messageService)
        {
            var client = Instance.Client;
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["username"] = username;
            queryParams["id"] = deviceToken;
            queryParams["msg_service"] = messageService;
            client.RegisterDevice(queryParams);
        }

        public static void DeregisterDevice(string username, string deviceToken, string messageService)
        {
            var client = Instance.Client;
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["username"] = username;
            queryParams["id"] = deviceToken;
            queryParams["msg_service"] = messageService;
            client.DeregisterDevice(queryParams);
        }

        public static void CreateSetReminderNotification(string receiverUsername, string contactId, string contactName)
        {
            var client = Instance.Client;
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["username"] = receiverUsername;
            queryParams["notification_type"] = "SetReminderNotification";
            queryParams["notifiable_type"] = contactName;
            queryParams["notifiable_id"] = contactId;
            client.CreateNotification(queryParams);
        }

        public static void PushNotification(string receiverUsername, string url, string message, string notification_id, string badgeCount)
        {
            var client = Instance.Client;
            NameValueCollection queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["username"] = receiverUsername;
            queryParams["url"] = url;
            queryParams["message"] = message;
            queryParams["notification_id"] = notification_id;
            queryParams["badge"] = badgeCount;
            client.PushNotification(queryParams);
        }

        #region private methods

        private void RefreshToken(MpnsClient client)
        {
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["grant_type"] = GrantType;
            client.GetToken(queryParams);
        }

        #endregion
    }
}