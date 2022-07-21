using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShapesApp.iOS;
/* Uncomment the line to use Approov SDK */
//using Approov;

[assembly: Xamarin.Forms.Dependency(
          typeof(GetShapePlatform))]
namespace ShapesApp.iOS
{
    public class GetShapePlatform : IGetShape
    {
        /* The Shapes URL */
        string shapesURL = "https://shapes.approov.io/v1/shapes/";
        /* The secret key: REPLACE with shapes_api_key_placeholder if using SECRET-PROTECTION */
        string shapes_api_key = "yXClypapWNHIifHUWmBIyPFAm";
        /* Comment out the line to use Approov SDK */
        private static HttpClient httpClient;
        /* Uncomment the line to use Approov SDK */
        //private static ApproovHttpClient httpClient;
        public GetShapePlatform()
        {
            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Key",shapes_api_key);
            /* Uncomment the lines bellow to use Approov SDK */
            //var factory = new ApproovHttpClientFactory();
            //httpClient = factory.GetApproovHttpClient("<enter-your-config-string-here>");
            // Add substitution header: Uncomment if using SECRET-PROTECTION
            //IosApproovHttpClient.AddSubstitutionHeader("Api-Key", null);
        }

        public Dictionary<string, string> GetHello()
        {
            Task<Dictionary<string, string>> response = GetHelloAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetHelloAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://shapes.approov.io/v1/hello").ConfigureAwait(false);
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
            HttpResponseMessage response = await httpClient.GetAsync(shapesURL).ConfigureAwait(false);
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
