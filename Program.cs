// VW Rwil Adaptor Preperations
using System.Net.Http.Headers;
using System.Text.Json;

namespace RwillLeadAdaptorBuildV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool bAdaptorRun = RwilLeadQuery();
            Console.WriteLine("Success of Rwil adaptor: " +  bAdaptorRun);
        }

        public static bool RwilLeadQuery()
        {
            bool bResultLoop = false;
            while (!bResultLoop)
            {
                JsonElement RwilAccess_Token = default;
                if (!RwilLeadGedaiAuth(ref RwilAccess_Token))
                {
                    // Error occured dont continue
                    break;
                }
                if (!RwilLeadCreateConsumer(RwilAccess_Token))
                {
                    // Error occured dont continue
                    break;
                }
                JsonElement Keyloop_Token = default;
                if (!KeyloopGatewayOAuth(ref Keyloop_Token))
                {
                    // Error occured dont continue
                    break;
                }
                if (!RwilGetServiceLeads(RwilAccess_Token))
                {
                    // Error occured dont continue
                    break;
                }
                // Only a single loop so we can break when we dont want to continue and return result
                bResultLoop = true;
            }
            return bResultLoop;
        }

        public static bool RwilLeadGedaiAuth(ref JsonElement RwilAccess_Token)
        {
            bool bResultLoop = false;
            while (!bResultLoop)
            {
                Task<string> Rwiltask = RwilLeadGedaiAuthAsync();
                string sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Rwil Gedai Auth = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                var RwilGedaiAuthObject = System.Text.Json.JsonDocument.Parse(sResponse);
                // retrieve the value
                RwilAccess_Token = RwilGedaiAuthObject.RootElement.GetProperty("access_token");
                // Only a single loop so we can break when we dont want to continue and return result
                bResultLoop = true;
            }
            return bResultLoop;
        }

        public static bool RwilLeadCreateConsumer(JsonElement RwilToken)
        {
            bool bResultLoop = false;
            while (!bResultLoop)
            {
                Task<string> Rwiltask = RwilLeadCreateConsumerAsync(RwilToken);
                string sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Rwil Create Consumer = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                bResultLoop = true;
            }
            return bResultLoop;
        }

        public static bool KeyloopGatewayOAuth(ref JsonElement KeyloopAccess_Token)
        {
            bool bResultLoop = false;
            while (!bResultLoop)
            {
                Task<string> Rwiltask = KeyloopGatewayOAuthAsync();
                string sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Keyloop Gateway Auth = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                var KeyloopAuthObject = System.Text.Json.JsonDocument.Parse(sResponse);
                // retrieve the value
                KeyloopAccess_Token = KeyloopAuthObject.RootElement.GetProperty("access_token");
                Console.WriteLine("Keyloop access token: " + KeyloopAccess_Token);
                // Only a single loop so we can break when we dont want to continue and return result
                bResultLoop = true;
            }
            return bResultLoop;
        }

        public static bool RwilGetServiceLeads(JsonElement RwilToken)
        {
            bool bResultLoop = false;
            while (!bResultLoop)
            {
                Task<string> Rwiltask = RwilGetServiceLeadsAsync(RwilToken);
                string sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Rwil Gedai Sevice Leads = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                bResultLoop = true;
            }
            return bResultLoop;
        }

        static async Task<string> RwilLeadGedaiAuthAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/oauth2/token");

            var collection = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", "ZAF996-Autoline-DRIVE-DMS-Q"),
                    new("client_secret", "ghVThqVlpUa83Pmkr2Ghs73yTZ69IyEI2d3g")
                };

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            Console.WriteLine(response);
            try
            {
                //  Block of code to try
                response.EnsureSuccessStatusCode();
                var actualcontent = await response.Content.ReadAsStringAsync();
                return actualcontent;
            }
            catch (Exception HttpRequestException)
            {
                //  Block of code to handle errors
                Console.WriteLine(HttpRequestException.HResult);

                return response.ToString();
            }
        }

        static async Task<string> RwilLeadCreateConsumerAsync(JsonElement RwilToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/consumers/ZAF996-Autoline-DRIVE-DMS-Q/ZAF996-Autoline-DRIVE-DMS-Q");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await client.SendAsync(request);
            Console.WriteLine(response);
            try
            {
                //  Block of code to try
                response.EnsureSuccessStatusCode();
                var actualcontent = await response.Content.ReadAsStringAsync();
                return actualcontent;
            }
            catch (Exception HttpRequestException)
            {
                //  Block of code to handle errors
                Console.WriteLine(HttpRequestException.HResult);

                return response.ToString();
            }
        }

        static async Task<string> KeyloopGatewayOAuthAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.eu-stage.keyloop.io/oauth/client_credential/accesstoken");
            var content = new StringContent("grant_type=client_credentials&client_id=l6IMyG2XJvAB6PjyBDt8peRkt8AExjvV&client_secret=GhRrgL3jZJATzaHj", null, "application/x-www-form-urlencoded");
            request.Content = content;
            var response = await client.SendAsync(request);
            Console.WriteLine(response);
            try
            {
                //  Block of code to try
                response.EnsureSuccessStatusCode();
                var actualcontent = await response.Content.ReadAsStringAsync();
                return actualcontent;
            }
            catch (Exception HttpRequestException)
            {
                //  Block of code to handle errors
                Console.WriteLine(HttpRequestException.HResult);

                return response.ToString();
            }
        }

        static async Task<string> RwilGetServiceLeadsAsync(JsonElement RwilToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://rwil-qa.volkswagenag.com/rwil/gedai/consumers/ZAF996-Autoline-DRIVE-DMS-Q/ZAF996-Autoline-DRIVE-DMS-Q/serviceLead");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(string.Empty);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            Console.WriteLine(response);
            try
            {
                //  Block of code to try
                response.EnsureSuccessStatusCode();
                var actualcontentleads = await response.Content.ReadAsStringAsync();
                return actualcontentleads;
            }
            catch (Exception HttpRequestException)
            {
                //  Block of code to handle errors
                Console.WriteLine(HttpRequestException.HResult);
                return response.ToString();
            }
        }

    }
}
