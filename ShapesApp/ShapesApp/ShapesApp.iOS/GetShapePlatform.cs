using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShapesApp.iOS;

[assembly: Xamarin.Forms.Dependency(
          typeof(GetShapePlatform))]
namespace ShapesApp.iOS
{
    public class GetShapePlatform : IGetShape
    {
        /* Comment out the line to use Approov SDK */
        private static HttpClient httpClient;
        /* Uncomment the line to use Approov SDK */
        //private static ApproovHttpClient httpClient;
        public GetShapePlatform()
        {
            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient();
            /* Uncomment the lines bellow to use Approov SDK */
            //var factory = new ApproovHttpClientFactory();
            //httpClient = factory.GetApproovHttpClient("<enter-your-config-string-here>")
            httpClient.BaseAddress = new Uri("https://shapes.approov.io");
        }

        public Dictionary<string, string> GetHello()
        {
            Task<Dictionary<string, string>> response = GetHelloAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetHelloAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/v1/hello").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(cont);
                // Add HTTP response code
                values["response"] = response.ReasonPhrase;
                return values;
            }
            Dictionary<string, string> errorValues = new Dictionary<string, string>();
            errorValues["response"] = response.ReasonPhrase;
            return errorValues;
        }

        public Dictionary<string, string> GetShape()
        {
            Task<Dictionary<string, string>> response = GetShapeAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetShapeAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/v2/shapes").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(cont);
                // Add HTTP response code
                values["response"] = response.ReasonPhrase;
                return values;
            }
            Dictionary<string, string> errorValues = new Dictionary<string, string>();
            errorValues["response"] = response.ReasonPhrase;
            return errorValues;
        }
    }
}
