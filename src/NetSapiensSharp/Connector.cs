using System;
using RestSharp;

namespace NetSapiensSharp
{
    public class Connector : IDisposable
    {
        private IRestClient _Client { get; set; }
        public IRestRequest _Request { get; set; }

        private string _ApiBaseUrl;
        private string _ClientId;
        private string _ClientSecret;
        private string _Username;
        private string _Password;
        private string _SessionToken;
        private string _RefreshToken;

        public Connector(string api_base_url, string client_id, string client_secret, string username, string password)
        {
            _ApiBaseUrl = api_base_url;
            _ClientId = client_id;
            _ClientSecret = client_secret;
            _Username = username;
            _Password = password;
        }

        private void AuthenticateUsernamePassword()
        {
            var r = Connector.Authenticate(_ApiBaseUrl, _ClientId, _ClientSecret, _Username, _Password);
            if (r.Data == null)
            {
                throw (r.ErrorException);
            }
            _SessionToken = r.Data.access_token;
            _RefreshToken = r.Data.refresh_token;
        }

        private void AuthenticateRefreshToken()
        {
            var r = Connector.Authenticate(_ApiBaseUrl, _ClientId, _ClientSecret, _RefreshToken);
            if (r.Data == null)
            {
                throw (r.ErrorException);
            }
            _SessionToken = r.Data.access_token;
            _RefreshToken = r.Data.refresh_token;
        }

        private void Authenticate()
        {
            if (_SessionToken != null)
            {
                return;
            }
            if (_RefreshToken != null)
            {
                try
                {
                    AuthenticateRefreshToken();
                }
                catch
                {
                }
            }
            if (_SessionToken == null)
            {
                AuthenticateUsernamePassword();
            }
        }

        public Request<Tresponse> CreateRequest<Tresponse>(string action_path) where Tresponse : class, new()
        {
            var myItem = new Request<Tresponse>()
            {
                _Client = new RestClient(_ApiBaseUrl),
                _Request = new RestRequest(action_path, Method.POST)
            };
            myItem._Client.AddHandler("text/html", new RestSharp.Deserializers.JsonDeserializer());
            myItem._Request.AddHeader("content-type", "application/json");
            myItem._Request.RequestFormat = DataFormat.Json;
            return myItem;
        }

        public IRestResponse<Tresponse> Send<Tresponse>(Request<Tresponse> request)
            where Tresponse : class, new()
        {
            Authenticate();
            request._Request.AddHeader("Authorization", "Bearer " + _SessionToken);
            var r = request._Client.Execute<Tresponse>(request._Request);
            if (r.StatusCode == System.Net.HttpStatusCode.OK && r.Data == null && typeof(Tresponse) == typeof(Common.OK))
            {
                r.Data = new Tresponse();
                r.ErrorException = null;
                r.ErrorMessage = null;
                r.ResponseStatus = ResponseStatus.Completed;
            }
            return r;
        }

        private class Authentication_Response
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
        }

        private static IRestResponse<Authentication_Response> Authenticate(string api_base_url, string client_id, string client_secret, string refresh_token)
        {
            var client = new RestClient(api_base_url);
            client.AddHandler("text/html", new RestSharp.Deserializers.JsonDeserializer());
            var request = new RestRequest("/oauth2/token/", Method.POST);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("refresh_token", refresh_token);
            return client.Execute<Authentication_Response>(request);
        }

        private static IRestResponse<Authentication_Response> Authenticate(string api_base_url, string client_id, string client_secret, string username, string password)
        {
            var client = new RestClient(api_base_url);
            client.AddHandler("text/html", new RestSharp.Deserializers.JsonDeserializer());
            var request = new RestRequest("/oauth2/token/", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            return client.Execute<Authentication_Response>(request);
        }

        public void Dispose()
        {
        }
    }
}
