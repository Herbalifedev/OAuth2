using System.Net;
using HL.OAuth2.Client;
using RestSharp;

namespace HL.OAuth2.Infrastructure
{
    public static class RestClientExtensions
    {
        public static IRestResponse ExecuteAndVerify(this IRestClient client, IRestRequest request)
        {
            var response = client.Execute(request);
            if (response.Content.IsEmpty() ||
                (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
            {
                throw new UnexpectedResponseException(response);
            }
            return response;
        }

        public static IRestResponse ExecuteAndVerifyRegisterEndpoint(this IRestClient client, IRestRequest request)
        {
            var response = client.Execute(request);
            if (response.Content.IsEmpty() ||
                response.StatusCode != HttpStatusCode.Created)
            {
                throw new UnexpectedResponseException(response);
            }
            return response;
        }

        public static IRestResponse ExecuteAndVerifyDeregisterEndpoint(this IRestClient client, IRestRequest request)
        {
            var response = client.Execute(request);
            if (!response.Content.IsEmpty() &&
                (int)response.StatusCode != 204)
            {
                throw new UnexpectedResponseException(response);
            }
            return response;
        }

        public static IRestResponse ExecuteAndVerifyCreateNotificationEndpoint(this IRestClient client, IRestRequest request)
        {
            var response = client.Execute(request);
            if (response.Content.IsEmpty() ||
                response.StatusCode != HttpStatusCode.Created)
            {
                throw new UnexpectedResponseException(response);
            }
            return response;
        }
    }
}