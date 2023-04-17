using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace oemLeads.Queries
{
    public class TRwilProcessLeadQuery
    {
        public static async Task<string> RwilLeadGedaiAuthAsync()
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static async Task<string> RwilLeadCreateConsumerAsync(JsonElement RwilToken)
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static async Task<string> KeyloopGatewayOAuthAsync()
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static async Task<string> RwilGetServiceLeadsAsync(JsonElement RwilToken)
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("\nMessage :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static async Task<string> RwilProcessLead_SABAsync(JsonElement KeyloopAccess_Token, string SABRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.eu-stage.keyloop.io/appointment/31905/44005860/v1/appointments");
            request.Headers.Add("Accept-Language", "ar");
            request.Headers.Add("Authorization", "Bearer " + KeyloopAccess_Token);
            var content = new StringContent(SABRequest, null, "application/json");
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught! on SAB Request");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
                // return response.ToString();
            }
        }

        public static async Task<string> RwilProcessLead_RwilT0Async(JsonElement RwilToken, string RwilT0Request)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/producers/retail/ZAF996-Autoline-DRIVE-DMS-Q/ZAF09002V/serviceleadstatusupdate/created");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(RwilT0Request, null, "application/json");
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
                // return response.ToString();
            }
        }

        public static async Task<string> RwilProcessLead_KeyloopLeadAsync(string KeyloopLeadRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://secure2.live-lead.com/api/v1/05537856-01b2-4f82-9e4e-afd500f002b1/octanevwsa/leads");

            request.Headers
                .Add("Authorization", "Basic " + RwilEncodeTo64("keyloopplatform.integration@keyloop.com:qFRy9Ce4hgpCJPcS"));

            var content = new StringContent(KeyloopLeadRequest, null, "application/json");
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static async Task<string> RwilProcessLead_RwilOffsetAsync(JsonElement RwilToken, string RwilOffsetRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/consumers/ZAF996-Autoline-DRIVE-DMS-Q/ZAF996-Autoline-DRIVE-DMS-Q/offset");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(RwilOffsetRequest, null, "application/json");
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");
                return "-1";
            }
        }

        public static async Task<string> RwilProcessLead_RwilT3Async(JsonElement RwilToken, string RwilT3Request)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/producers/retail/ZAF996-Autoline-DRIVE-DMS-Q/ZAF09002V/serviceleadstatusupdate/created");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(RwilT3Request, null, "application/json");
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
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }

        public static string RwilEncodeTo64(string toEncode)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string RwilDecodeFrom64(string encodedData)
        {
            var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return Encoding.ASCII.GetString(encodedDataAsBytes);
        }
    }
}
