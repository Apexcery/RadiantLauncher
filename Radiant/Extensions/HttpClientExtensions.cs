using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Radiant.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken)
        {
            var contentAsString = await content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<T>(contentAsString);
        }
    }
}
