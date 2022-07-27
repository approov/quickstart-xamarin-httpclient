using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
/* Uncomment the line to use Approov SDK */
//using Approov;




namespace ShapesApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /* The endpoint version being used: v1 unprotected and v3 for Approov API protection */
        static string endpointVersion = "v1";
        /* The Shapes URL */
        string shapesURL = "https://shapes.approov.io/" + endpointVersion + "/shapes/";
        /* The Hello URL */
        string helloURL = "https://shapes.approov.io/" + endpointVersion + "/hello/";
        /* The secret key: REPLACE with shapes_api_key_placeholder if using SECRETS-PROTECTION */
        string shapes_api_key = "yXClypapWNHIifHUWmBIyPFAm";
        /* Comment out the line to use Approov SDK */
        private static HttpClient httpClient;
        
        // The Dictionary to hold the images we display
        private Dictionary<string, string> allImages = new Dictionary<string, string>();
        // The current image
        private ImageSource currentStatusImage;
        public ImageSource CurrentStatusImage
        {
            get 
            { 

                if (currentStatusImage == null) return ImageSource.FromResource(allImages["approov"], typeof(MainPage).GetTypeInfo().Assembly);
                return currentStatusImage; 
            }
            set
            {
                if (value == null) ImageSource.FromResource(allImages["approov"], typeof(MainPage).GetTypeInfo().Assembly);
                else currentStatusImage = value;
                OnPropertyChanged(nameof(CurrentStatusImage));
            }
        }

        private string statusLabelProperty;
        public string StatusLabelProperty
        {
            get { return statusLabelProperty; }
            set
            {
                statusLabelProperty = value;
                OnPropertyChanged(nameof(StatusLabelProperty));
            }
        }


        public MainPage()
        {
            InitializeComponent();
            // Load all images
            LoadAllImages();
            BindingContext = this;
            
            StatusLabelProperty = "Say Hello or Get Shape?";

            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient();
            /* Uncomment the lines bellow to use Approov SDK */
            //ApproovService.Initialize("<enter-your-config-string-here>");
            //httpClient = ApproovService.CreateHttpClient();
            // Add substitution header: Uncomment if using SECRETS-PROTECTION
            //ApproovService.AddSubstitutionHeader("Api-Key", null);
            httpClient.DefaultRequestHeaders.Add("Api-Key", shapes_api_key);
        }

        public void SetStatusImageFromString(string status)
        {
            CurrentStatusImage = ImageSource.FromResource(allImages[status], typeof(MainPage).GetTypeInfo().Assembly);
        }

        protected void LoadAllImages() {
            allImages.Add("approov", "ShapesApp.Images.approov.png");
            allImages.Add("circle", "ShapesApp.Images.circle.png");
            allImages.Add("confused", "ShapesApp.Images.confused.png");
            allImages.Add("hello", "ShapesApp.Images.hello.png");
            allImages.Add("rectangle", "ShapesApp.Images.rectangle.png");
            allImages.Add("square", "ShapesApp.Images.square.png");
            allImages.Add("triangle", "ShapesApp.Images.triangle.png");
        }
        // Hello button press event
        protected void HelloEvent(object sender, EventArgs args) 
        {
            try
            {
                Task<Dictionary<string, string>> response = GetHelloAsync();
                Dictionary<string, string> responseData = response.Result;
                if (!responseData.ContainsKey("text"))
                {
                    SetStatusImageFromString("confused");
                }
                else
                {
                    SetStatusImageFromString("hello");
                    StatusLabelProperty = responseData["text"];
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception during Hello: " + e.Message);
                SetStatusImageFromString("confused");
                StatusLabelProperty = e.Message;
            }
        }

        // Async network call to the hello endpoint
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
        // Shape event button press
        protected void ShapeEvent(object sender, EventArgs args) 
        {
            try
            {
                Task<Dictionary<string, string>> response = GetShapeAsync();
                Dictionary<string, string> responseData = response.Result;
                if (!responseData.ContainsKey("shape"))
                {
                    SetStatusImageFromString("confused");

                }
                else
                {
                    SetStatusImageFromString(responseData["shape"].ToLower());
                }
                StatusLabelProperty = responseData["response"];

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during Get Shape: " + e.Message);
                SetStatusImageFromString("confused");
                StatusLabelProperty = e.Message;
            }
        }
        // Async network call to the hello endpoint 
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

    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
