using HL.OAuth2.Client.Impl;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace HL.OAuth2.Client
{
    public class MpmsClientManager
    {
        private AuthorizationRoot _authorizationRoot;
        private const string ProviderName = "MPMS";
        private const string GrantType = "client_credentials";
        private MpmsClient _client;
        private static MpmsClientManager _mpmsClientManager;

        private AuthorizationRoot Root
        {
            get
            {
                return _authorizationRoot ?? new AuthorizationRoot();
            }
        }

        private static MpmsClientManager Instance
        {
            get
            {
                if (_mpmsClientManager == null)
                {
                    _mpmsClientManager = new MpmsClientManager();
                }
                return _mpmsClientManager;
            }
        }

        private MpmsClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = (MpmsClient)Root.Clients.Where(klient => klient.Name.Equals(ProviderName)).First();
                }

                if (_client == null)
                {
                    throw new Exception("MPMS client is not defined.");
                }

                if (string.IsNullOrEmpty(_client.AccessToken) || _client.ExpiresAt < DateTime.Now)
                {
                    RefreshToken(_client);
                }
                return _client;
            }
        }

        #region API

        public static IRestResponse UploadImage(string username, string base64EncodedImage)
        {
            var client = Instance.Client;
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["username"] = username;
            queryParams["img"] = base64EncodedImage;
            return client.UploadImage(queryParams);
        }

        public static IRestResponse UpdateImage(string imageId, string username, string base64EncodedImage)
        {
            var client = Instance.Client;
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["id"] = imageId;
            queryParams["username"] = username;
            queryParams["img"] = base64EncodedImage;
            return client.UpdateImage(queryParams);
        }

        public static IRestResponse RemoveImage(string imageId)
        {
            var client = Instance.Client;
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["id"] = imageId;
            return client.RemoveImage(queryParams);
        }

        public static IRestResponse RemoveImages(string[] imageIds)
        {
            var client = Instance.Client;
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["access_token"] = client.AccessToken;
            queryParams["image_list"] = string.Join(",", imageIds);
            return client.RemoveImages(queryParams);
        }

        #endregion

        #region Private methods

        private void RefreshToken(MpmsClient client)
        {
            var queryParams = new NameValueCollection();
            queryParams["code"] = "code";
            queryParams["grant_type"] = GrantType;
            client.GetToken(queryParams);
        }

        #endregion
    }
}
