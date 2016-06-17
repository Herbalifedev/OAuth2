using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using HL.OAuth2.Configuration;
using HL.OAuth2.Infrastructure;
using HL.OAuth2.Models;
using RestSharp;

namespace HL.OAuth2.Client.Impl
{
    class MpmsClient : OAuth2Client
    {
        public MpmsClient(IRequestFactory factory, IClientConfiguration configuration) : base(factory, configuration)
        {
        }

        #region MPMS-specific APIs

        /// <summary>
        /// POST /mpms/images           ## Create/Upload image
        /// PUT /mpms/images/image_id   ## Update/Replace image
        /// </summary>

        #region Instance variables

        private string _baseURI = null;

        protected override string BaseURI
        {
            get
            {
                return _baseURI ?? System.Configuration.ConfigurationManager.AppSettings[BaseUriKey];
            }
        }

        protected override string BaseUriKey
        {
            get
            {
                return "MpmsBaseURI";
            }
        }

        #endregion

        #region API

        public IRestResponse UploadImage(NameValueCollection parameters)
        {
            PerformValidation();
            return QueryUploadImage(parameters);
        }

        public IRestResponse UpdateImage(NameValueCollection parameters)
        {
            PerformValidation();
            return QueryUpdateImage(parameters);
        }

        public IRestResponse RemoveImage(NameValueCollection parameters)
        {
            PerformValidation();
            return QueryRemoveImage(parameters);
        }

        #endregion

        #region Private methods

        #region Endpoints

        private Endpoint UploadImageEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpms/images" }; }
        }

        private Endpoint UpdateImageEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpms/images/{0}" }; }
        }

        private Endpoint RemoveImageEndpoint
        {
            get { return new Endpoint { BaseUri = BaseURI, Resource = "/mpms/images/{0}" }; }
        }

        #endregion

        #region API queries

        private IRestResponse QueryUploadImage(NameValueCollection parameters)
        {
            var client = _factory.CreateClient(UploadImageEndpoint);
            var request = _factory.CreateRequest(UploadImageEndpoint, Method.POST);

            var para = SimpleJson.SerializeObject(new UploadImageRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("username"),
                parameters.Get("img")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyUploadImageEndpoint(request);
            return response;
        }

        private IRestResponse QueryUpdateImage(NameValueCollection parameters)
        {
            var updatedEndpoint = UpdateImageEndpoint;
            updatedEndpoint.Resource = string.Format(updatedEndpoint.Resource, parameters.Get("id"));
            var client = _factory.CreateClient(updatedEndpoint);
            var request = _factory.CreateRequest(updatedEndpoint, Method.PUT);

            var para = SimpleJson.SerializeObject(new UpdateImageRequestInfo(
                parameters.Get("access_token"),
                parameters.Get("username"),
                parameters.Get("img")
            ));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyUpdateImageEndpoint(request);
            return response;
        }

        private IRestResponse QueryRemoveImage(NameValueCollection parameters)
        {
            var updatedEndpoint = RemoveImageEndpoint;
            updatedEndpoint.Resource = string.Format(updatedEndpoint.Resource, parameters.Get("id"));
            var client = _factory.CreateClient(updatedEndpoint);
            var request = _factory.CreateRequest(updatedEndpoint, Method.DELETE);

            var para = SimpleJson.SerializeObject(new RemoveImageRequestInfo(parameters.Get("access_token")));
            request.AddParameter("application/json", para, ParameterType.RequestBody);

            var response = client.ExecuteAndVerifyRemoveImageEndpoint(request);
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
                return "MPMS";
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
                    Resource = "/mpms/token"
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
