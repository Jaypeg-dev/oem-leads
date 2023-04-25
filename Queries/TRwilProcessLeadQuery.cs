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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/oauth2/token");

            var collection = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", $"{App.Rwil_client_id}"),
                    new("client_secret", $"{App.Rwil_client_secret}")
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/consumers/{App.Rwil_systemid}/{App.Rwil_consumername}");
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Keyloop_baseurl}/oauth/client_credential/accesstoken");
            var content = new StringContent($"grant_type=client_credentials&client_id={App.Keyloop_client_id}&client_secret={App.Keyloop_client_secret}", null, "application/x-www-form-urlencoded");
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{App.Rwil_host}/{App.Rwil_context}/consumers/{App.Rwil_systemid}/{App.Rwil_consumername}/serviceLead");
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Keyloop_baseurl}/appointment/{App.Keyloop_enteprise}/{App.Keyloop_store}/v1/appointments");
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

        public static async Task<string> RwilProcessLead_PatchRODetailsAsync(JsonElement KeyloopAccess_Token, string RODRequest,string SABID)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://{App.Keyloop_baseurl}/{App.Keyloop_enteprise}/{App.Keyloop_store}/v1/repair-orders/{SABID}/details");
            request.Headers.Add("x-on-behalf-of", "apiuser");
            request.Headers.Add("Authorization", "Bearer " + KeyloopAccess_Token);
            var content = new StringContent(RODRequest, null, "application/json");
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
                Console.WriteLine("\nException Caught! on patching repair order details");
                Console.WriteLine("\nMessage :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
                // return response.ToString();
            }
        }

        public static async Task<string> RwilProcessLead_RwilT0Async(JsonElement RwilToken, string RwilT0Request)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/producers/retail/{App.Rwil_systemid}/{App.Rwil_partnerkey}/serviceleadstatusupdate/created?targetSystem={App.Rwil_targetsystem}");
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.KeyloopLeads_URL}/api/v1/{App.KeyloopLeads_IntegrationKey}/{App.KeyloopLeads_DealerCode}/leads");

            request.Headers
                .Add("Authorization", "Basic " + RwilEncodeTo64($"{App.KeyloopLeads_User}:{App.KeyloopLeads_Password}"));

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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/consumers/{App.Rwil_systemid}/{App.Rwil_consumername}/offset");
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/producers/retail/{App.Rwil_systemid}/{App.Rwil_partnerkey}/serviceleadstatusupdate/created?targetSystem={App.Rwil_targetsystem}");
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
