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
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{App.Keyloop_baseurl}/{App.Keyloop_enteprise}/{App.Keyloop_store}/v1/repair-orders/{RepairOrderID}");
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
        public static async Task<string> RwilUpdateLead_RwilTUpdateAsync(JsonElement RwilToken, string RwilTURequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{App.Rwil_host}/{App.Rwil_context}/producers/retail/{App.Rwil_systemid}/{App.Rwil_partnerkey}/serviceleadstatusupdate/created?targetSystem={App.Rwil_targetsystem}");
            request.Headers.Add("Authorization", "Bearer " + RwilToken);
            var content = new StringContent(RwilTURequest, null, "application/json");
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
                Console.WriteLine("\nException Caught in Rwil T Update!");
                Console.WriteLine("Message :{0} ", e.Message);
                var errorresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{errorresponse}");

                return "-1";
            }
        }
    }
}
