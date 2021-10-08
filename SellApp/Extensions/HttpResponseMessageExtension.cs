using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SellApp.Extensions
{
    internal static class HttpRequestMessageExtensions
    {
        public static async Task<T> ResponseJsonDeserializeAsync<T>(this HttpResponseMessage httpResponseMessage)
            => JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync()
                .ConfigureAwait(false));
    }
}
