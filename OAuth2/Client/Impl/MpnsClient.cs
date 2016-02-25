using System;
using Newtonsoft.Json.Linq;
using HL.OAuth2.Configuration;
using HL.OAuth2.Infrastructure;
using HL.OAuth2.Models;
using System.Collections.Specialized;
using RestSharp;

namespace HL.OAuth2.Client.Impl
{
    public class MpnsClient : OAuth2Client
    {
        public MpnsClient(IRequestFactory factory, IClientConfiguration configuration) : base(factory, configuration)
        {
        }

        #region MPNS-specific APIs

        /// <summary>
        /// TODO
        /// GET  /mpns/notifications         ## Get notifications list
        /// GET  /mpns/notifications/count   ## Get the notifications count
        /// POST /mpns/notifications         ## Create notification
        /// POST /mpns/notifications/push    ## Push a notification directly via the PNsService
        /// POST /mpns/devices/register      ## Register mobile device
        /// POST /mpns/devices/deregister    ## Deregister mobile device
        /// </summary>
        /// 

        private string _baseURI = null;

        private string BaseURI
        {
            get
            {
                return _baseURI ?? System.Configuration.ConfigurationManager.AppSettings["MpnsBaseURI"];
            }
        }

        #region API

        public string RegisterDevice(NameValueCollection parameters)
        {
            var response = QueryRegisterDevice(parameters);
            return string.Format("{0}, {1}", response.StatusCode, response.Content);
        }

        public string DeregisterDevice(NameValueCollection parameters)
        {
            var response = QueryDeregisterDevice(parameters);
            return string.Format("{0}, {1}", response.StatusCode, response.Content);
        }

        public string CreateNotification(NameValueCollection parameters)
        {
            var response = QueryCreateNotification(parameters);
            return string.Format("{0}, {1}", response.StatusCode, response.Content);
        }

        public string PushNotification(NameValueCollection parameters)
        {
            var response = QueryPushNotification(parameters);
            return string.Format("{0}, {1}", response.StatusCode, response.Content);
        }

        #endregion

        #region Private methods

        #region Endpoints

        private Endpoint RegisterDeviceEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/devices/register" }; }
        }

        private Endpoint DeregisterDeviceEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/devices/deregister" }; }
        }

        private Endpoint CreateNotificationEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/notifications" }; }
        }

        private Endpoint PushNotificationEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/notifications/push" }; }
        }

        #endregion

        #region API queries

        private IRestResponse QueryRegisterDevice(NameValueCollection parameters)
        {
            var client = _factory.CreateClient(RegisterDeviceEndpoint);
            var request = _factory.CreateRequest(RegisterDeviceEndpoint, Method.POST);

            var para = SimpleJson.SerializeObject(new RegisterDeviceRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("username"),
                parameters.Get("id"),
                parameters.Get("msg_service")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyRegisterEndpoint(request);
            return response;
        }

        private IRestResponse QueryDeregisterDevice(NameValueCollection parameters)
        {
            var client = _factory.CreateClient(DeregisterDeviceEndpoint);
            var request = _factory.CreateRequest(DeregisterDeviceEndpoint, Method.POST);

            var para = SimpleJson.SerializeObject(new DeregisterDeviceRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("username"),
                parameters.Get("id"),
                parameters.Get("msg_service")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyDeregisterEndpoint(request);
            return response;
        }

        private IRestResponse QueryCreateNotification(NameValueCollection parameters)
        {
            var client = _factory.CreateClient(CreateNotificationEndpoint);
            var request = _factory.CreateRequest(CreateNotificationEndpoint, Method.POST);

            var para = SimpleJson.SerializeObject(new CreateNotificationRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("notification_type"),
                parameters.Get("username"),
                parameters.Get("notifiable_type"),
                parameters.Get("notifiable_id")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyCreateNotificationEndpoint(request);
            return response;
        }

        private IRestResponse QueryPushNotification(NameValueCollection parameters)
        {
            var client = _factory.CreateClient(PushNotificationEndpoint);
            var request = _factory.CreateRequest(PushNotificationEndpoint, Method.POST);

            var para = SimpleJson.SerializeObject(new PushNotificationRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("username"),
                parameters.Get("url"),
                parameters.Get("message"),
                parameters.Get("notification_id"),
                parameters.Get("badge")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyCreateNotificationEndpoint(request);
            return response;
        }

        #endregion

        #endregion

        #endregion

        #region Overridden methods

        public override string Name
        {
            get
            {
                return "MPNS";
            }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = BaseURI,
                    Resource = "/mpns/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override UserInfo ParseUserInfo(string content)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}