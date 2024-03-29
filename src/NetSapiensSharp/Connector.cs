﻿using System;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace NetSapiensSharp
{
    public class Connector : IDisposable
    {
        private string _ApiBaseUrl;
        private string _ClientId;
        private string _ClientSecret;
        private string _Username;
        private string _Password;
        private string _SessionToken;
        private string _RefreshToken;
        private int _ExpiresInSeconds;
        private DateTime _ExpirationStartTime = DateTime.UtcNow;
        private const int _UnauthorizedRetryLimit = 5;

        public Connector(string api_base_url, string client_id, string client_secret, string username, string password)
        {
            _ApiBaseUrl = api_base_url.TrimEnd('/') + '/';
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
            _ExpiresInSeconds = r.Data.expires_in;
            _ExpirationStartTime = DateTime.UtcNow;
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
            _ExpiresInSeconds = r.Data.expires_in;
            _ExpirationStartTime = DateTime.UtcNow;
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

        private bool IsAccessTokenExpired()
        {
            var myElapsedSeconds = Math.Ceiling((DateTime.UtcNow - _ExpirationStartTime).TotalSeconds);
            return myElapsedSeconds >= (double)_ExpiresInSeconds;
        }

        public Request<Tresponse> CreateRequest<Tresponse>(string action_path) where Tresponse : class, new()
        {
            var myItem = new Request<Tresponse>()
            {
                _Client = new RestClient(_ApiBaseUrl).UseNewtonsoftJson(),
                _Request = new RestRequest(action_path, Method.POST, DataFormat.Json)
            };
            myItem._Client.AddHandler("text/html", () => new JsonNetSerializer());
            myItem._Request.AddHeader("content-type", "application/json");
            return myItem;
        }

        public IRestResponse<Tresponse> Send<Tresponse>(Request<Tresponse> request)
            where Tresponse : class, new()
        {
            IRestResponse<Tresponse> myResponse = null;
            int myRetryCount = 0;
            if (IsAccessTokenExpired())
            {
                _SessionToken = null;
            }
            do
            {
                Authenticate();
                request._Request.AddOrUpdateParameter("Authorization", "Bearer " + _SessionToken, ParameterType.HttpHeader);
                myResponse = request._Client.Execute<Tresponse>(request._Request);

                if (myResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _SessionToken = null;
                    myRetryCount++;
                }
                else if (myResponse.StatusCode == System.Net.HttpStatusCode.OK && myResponse.Data == null && typeof(Tresponse) == typeof(Common.OK))
                {
                    myResponse.Data = new Tresponse();
                    myResponse.ErrorException = null;
                    myResponse.ErrorMessage = null;
                    myResponse.ResponseStatus = ResponseStatus.Completed;
                    break;
                }
            }
            while (_SessionToken == null && myRetryCount < _UnauthorizedRetryLimit);

            if (myRetryCount == _UnauthorizedRetryLimit)
            {
                myResponse.Data = null;
                myResponse.ErrorException = null;
                myResponse.ErrorMessage = $"Failed to authorize. Retry limit of {_UnauthorizedRetryLimit} has been reached.";
                myResponse.ResponseStatus = ResponseStatus.Aborted;
            }
            return myResponse;
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
            var client = new RestClient(api_base_url).UseNewtonsoftJson();
            client.AddHandler("text/html", () => new JsonNetSerializer());
            var request = new RestRequest("/oauth2/token/", Method.POST);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("refresh_token", refresh_token);
            return client.Execute<Authentication_Response>(request);
        }

        private static IRestResponse<Authentication_Response> Authenticate(string api_base_url, string client_id, string client_secret, string username, string password)
        {
            var client = new RestClient(api_base_url).UseNewtonsoftJson();
            client.AddHandler("text/html", () => new JsonNetSerializer());
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
