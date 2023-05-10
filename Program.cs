// VW Rwil Adaptor Preperations
using System.Text.Json;
using oemLeads.Queries;

namespace RwillLeadAdaptorBuildV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bAdaptorRun = RwilLeadQuery();
            Console.WriteLine("Success of Rwil adaptor: " + bAdaptorRun);
        }

        public static bool RwilLeadQuery()
        {
            var bResultLoop = false;
            JsonElement RwilAccess_Token = default;
            JsonElement Keyloop_Token = default;
            int Timer = 7000, runTimes = 90;

            while (!bResultLoop)
            {
                // Simulate lambda scedular
                for (int i = 0; i < runTimes; i++)
                {
                    // Start by processing Rwil Leads
                    if (!RwilProcessLead(ref RwilAccess_Token, ref Keyloop_Token)) break;
                    // Next routine to now use the database and perform T4 T5 T6 T7 updates to Rwil using repair order
                    if (!RwilUpdateLead(ref RwilAccess_Token, ref Keyloop_Token)) break;
                    Thread.Sleep(Timer);
                }
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead(ref JsonElement RwilAccess_Token, ref JsonElement Keyloop_Token)
        {
            var bResultLoop = false;
            
            while (!bResultLoop)
            {
                if (!RwilProcessLeadQuery.RwilLeadGedaiAuth(ref RwilAccess_Token)) break;
                if (!RwilProcessLeadQuery.KeyloopGatewayOAuth(ref Keyloop_Token)) break;
                // Anything before can effect the update process, next routine can fail will try again later
                RwilProcessLeadQuery.RwilLeadCreateConsumer(RwilAccess_Token);
                RwilProcessLeadQuery.RwilGetServiceLeads(RwilAccess_Token, Keyloop_Token);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead(ref JsonElement RwilAccess_Token, ref JsonElement Keyloop_Token)
        {
            bool bResultLoop = false;
            string path = @"C:\Kerridge\", filename = "DataBase.txt", updatefile = "DataBaseUpdate.txt";

            while (!bResultLoop)
            {
                if (!RwilUpdateLeadQuery.RwilUpdated_ReadUpdateLead(ref RwilAccess_Token, ref Keyloop_Token)) break;
                // This would fall out when platform database is added this is temp to update current info
                if (!RwilUpdateLeadQuery.Rwil_CheckReplaceDataFile(path + filename, path + updatefile)) break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

    }
}