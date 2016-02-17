﻿using System;
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;
using System.Collections.Specialized;
using RestSharp;

namespace OAuth2.Client.Impl
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

        public string RegisterDevice(NameValueCollection parameters)
        {
            var response = QueryRegisterDevice(parameters);
            return string.Format("{0}, {1}", response.StatusCode, response.Content);
        }

        public bool DeregisterDevice(NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }

        private Endpoint RegisterDeviceEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/devices/register" }; }
        }

        private Endpoint DeregisterDeviceEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpns/devices/deregister" }; }
        }

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

            var response = client.ExecuteAndVerify(request);
            return response;
        }

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