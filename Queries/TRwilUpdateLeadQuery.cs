using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace oemLeads.Queries
{
    internal class TRwilUpdateLeadQuery
    {
        public static async Task<string> RwilUpdateLead_GetRepairOrderIDAsync(JsonElement KeyloopAccess_Token, string RepairOrderID)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.eu-stage.keyloop.io/31905/44005860/v1/repair-orders/{RepairOrderID}");
            request.Headers.Add("Accept-Language", "ar");
            request.Headers.Add("Authorization", "Bearer " + KeyloopAccess_Token);
            var content = new StringContent("", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            Console.WriteLine(response);

            try
            {
                response.EnsureSuccessStatusCode();
                var actualcontent = await response.Content.ReadAsStringAsync();
                return actualcontent;
            }
            catch (HttpRequestException e)
            {
                //  Block of code to handle errors
                Console.WriteLine("\nException Caught while getting repair order details!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }
        public static async Task<string> RwilUpdateLead_RwilT4Async(JsonElement RwilToken, string RwilT4Request)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rwil-qa.volkswagenag.com/rwil/gedai/producers/retail/ZAF996-Autoline-DRIVE-DMS-Q/ZAF09002V/serviceleadstatusupdate/created");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(RwilT4Request, null, "application/json");
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
    }
}
