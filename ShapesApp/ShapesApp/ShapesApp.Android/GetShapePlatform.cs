using System.Net.Http;
using System.Collections.Generic;
using ShapesApp.Droid;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
/* Uncomment the line to use Approov SDK */
//using Approov;

[assembly: Xamarin.Forms.Dependency(
          typeof(GetShapePlatform))]
namespace ShapesApp.Droid
{
    public class GetShapePlatform : IGetShape
    {
        /* The endpoint version being used: v1 unprotected and v3 for Approov API protection */
        static string endpointVersion = "v1";
        /* The Shapes URL */
        string shapesURL = "https://shapes.approov.io/" + endpointVersion + "/shapes/";
        /* The Hello URL */
        string helloURL = "https://shapes.approov.io/" + endpointVersion + "/hello/";
        /* The secret key: REPLACE with shapes_api_key_placeholder if using SECRETS-PROTECTION */
        string shapes_api_key = "yXClypapWNHIifHUWmBIyPFAm";
        /* The http client */
        private static HttpClient httpClient;
        public GetShapePlatform()
        {
            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient();
            /* Uncomment the lines bellow to use Approov SDK */
            //ApproovService.Initialize("<enter-your-config-string-here>");
            //httpClient = ApproovService.CreateHttpClient();
            // Add substitution header: Uncomment if using SECRETS-PROTECTION
            //ApproovService.AddSubstitutionHeader("Api-Key", null);
            httpClient.DefaultRequestHeaders.Add("Api-Key", shapes_api_key);
        }

        public Dictionary<string, string> GetHello()
        {
            Task<Dictionary<string, string>> response = GetHelloAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetHelloAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(helloURL).ConfigureAwait(false);
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
