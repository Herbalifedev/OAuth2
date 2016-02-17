using System;
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
{
    public class MpnsClient : OAuth2Client
    {
        public MpnsClient(IRequestFactory factory, IClientConfiguration configuration) : base(factory, configuration)
        {
        }

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
                    BaseUri = System.Configuration.ConfigurationManager.AppSettings["MpnsBaseURI"],
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
    }
}