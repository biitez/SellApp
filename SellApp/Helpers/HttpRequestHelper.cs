using SellApp.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SellApp.Helpers
{
    public class HttpRequestHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _authorizationTokenBearer;

        public HttpRequestHelper(string AuthorizationTokenBearer, HttpClient httpClient = null)
        {
            _authorizationTokenBearer = AuthorizationTokenBearer ?? throw new UnauthorizedAccessException(nameof(AuthorizationTokenBearer));
            _httpClient = httpClient ?? throw new Exception("Bad HttpClient Instance");
        }

        public async Task<T> SendRequestJsonDeserializeAsync<T>(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Headers.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authorizationTokenBearer);

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead)
                .ConfigureAwait(false);

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Unauthorized)) // 401
            {
                throw new UnauthorizedAccessException("Unauthenticated");
            }

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Forbidden)) // 403
            {
                throw new UnauthorizedAccessException("Unauthorized Resource");
            }

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.NotFound)) // 404
            {
                throw new UnauthorizedAccessException("Resource NotFound");
            }
            
            return await httpResponseMessage.ResponseJsonDeserializeAsync<T>().ConfigureAwait(false);
        }
    }
}
