using oemLeads.Commands.Models;
using System.Text.Json;

namespace oemLeads.Queries
{
    internal class RwilUpdateLeadQuery: TRwilUpdateLeadQuery
    {
        public static bool RwilUpdated_ReadUpdateLead(ref JsonElement RwilAccess_Token, ref JsonElement Keyloop_Token)
        {
            bool bResultLoop = false,bUpdateFailed = false;
            string path = @"C:\Kerridge\", filename = "DataBase.txt";
            string GedaiServiceID = "", SABID = "", NextStage = "";
            string RoResponse = "";
            while (!bResultLoop)
            {
                // Temp call for token during test
                if (!RwilProcessLeadQuery.KeyloopGatewayOAuth(ref Keyloop_Token)) break;
                // TODO Mock Database needs to move to platform database
                try
                {
                    using StreamReader sr = File.OpenText(path + filename);
                    {
                        string? s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            bUpdateFailed = true;
                            GedaiDataRetrieve(s, ref GedaiServiceID, ref SABID, ref NextStage);
                            // Get the repair order async task needed using the SABID...
                            if (!Rwil_GetRepairOrderID(SABID, Keyloop_Token, ref RoResponse)) break;
                            if (!Rwil_PostTUpdates(NextStage, GedaiServiceID, RwilAccess_Token, RoResponse)) break;
                            bUpdateFailed = false;
                        }
                    }
                    if (bUpdateFailed) break;
                    bResultLoop = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    bResultLoop = false;
                }
            }

            return bResultLoop;
        }

        public static void GedaiDataRetrieve(string DataStored, ref string GedaiServiceID, ref string SABID, ref string NextStage, string Delimenator = ",")
        {
            // Init the variables passed through first
            GedaiServiceID = "";
            SABID = "";
            NextStage = "";
            // Split info string into the relevant fields
            string[] values = DataStored.Split(Delimenator);
            for (int i = 0; i < values.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        // Gedai Service ID
                        GedaiServiceID = values[i].Trim();
                        break;
                    case 1:
                        // SAB ID
                        SABID = values[i].Trim();
                        break;
                    case 2:
                        // Next Stage to run
                        NextStage = values[i].Trim();
                        break;
                    default:
                        break;
                }
            }
        }

        public static bool Rwil_GetRepairOrderID(string AppointmentID, JsonElement KeyloopToken, ref string ROResponse)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                // Validations needed upfront
                if (AppointmentID == "") break;

                var ROtask = RwilUpdateLead_GetRepairOrderIDAsync(KeyloopToken, AppointmentID);
                ROResponse = ROtask.GetAwaiter().GetResult();
                if (ROResponse == "-1") break;
                // TODO Need to build the model so we can perform the next Part 
                Console.WriteLine($"Repair Order: {ROResponse}");
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool Rwil_PostTUpdates(string Stage, string GedaiServiceID, JsonElement RwilToken, string ROResponse)
        {
            var bResultLoop = false;
            string path = @"C:\Kerridge\", filename = "DataBaseUpdate.txt";
            string updateStage = "";

            while (!bResultLoop)
            {
                var ROJasonResponse = JsonSerializer.Deserialize<RepairOrderResponse>(ROResponse);
                switch (Stage)
                {
                    case "T4":
                        Console.WriteLine("Do T4 update");
                        updateStage = "T5";
                        break;
                    case "T5":
                        updateStage = "T6";
                        break;
                    case "T6":
                        updateStage = "T7";
                        break;
                    case "T7":
                        updateStage = "END";
                        break;
                    default:
                        break;
                }
                // TODO need to replace this logic with update database from platform
                if (!File.Exists(path))
                {
                    using StreamWriter sw = File.CreateText(path+filename);
                }

                using (StreamWriter sw = File.AppendText(path+filename))
                {
                    if (updateStage != "END")
                    {
                        sw.WriteLine($"{GedaiServiceID},{ROJasonResponse!.RepairOrderId},{updateStage}");
                    }
                }
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT4(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken)
        {
            var bResultLoop = false;
            var RwilT4JsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilUpdateLead_RwilT4Request_Build(RepairOrderInfo, ref RwilT4JsonString, GedaiServiceID)) break;
                Console.WriteLine(RwilT4JsonString);
                // Async task needed here to perform the Rwil T4 Update, dont forget error handling
                var RwilT3task = RwilUpdateLead_RwilT4Async(RwilToken, RwilT4JsonString);
                var RwilT4RespJsonString = RwilT3task.GetAwaiter().GetResult();
                if (RwilT4RespJsonString == "-1") break;
                Console.WriteLine(RwilT4RespJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT4Request_Build(RepairOrderResponse? RepairOrderInfo, ref string RwilT4JsonString, string GedaiServiceID)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var rwilt4 = new RwilT4()
                {
                   Event = new T4Event()
                   {
                        EventType = "T4",
                        ServiceLeadRecordID = GedaiServiceID,
                        Timestamp = DateTime.Now,
                        // Need to figure this one
                        CustomerContactChannel = "",
                   },
                };

                RwilT4JsonString = JsonSerializer.Serialize(rwilt4);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool Rwil_CheckReplaceDataFile(string oldfile,string newfile,string backupfile)
        {
            var bResultLoop = false;
            
            while (!bResultLoop)
            {
                string path = newfile;
                string path2 = oldfile;
                try
                {
                    if (!File.Exists(path))
                    {      
                        using (FileStream fs = File.Create(path)) { }
                    }
                    // Ensure that the target does not exist.        
                    if (File.Exists(path2)) File.Delete(path2);
                    // Move the file.
                    File.Move(path, path2);
                    Console.WriteLine("{0} was moved to {1}.", path, path2);
                    // See if the original exists now.
                    if (File.Exists(path))
                    {
                        Console.WriteLine("The original file still exists, which is unexpected.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The original file no longer exists, which is expected.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                bResultLoop = true;
            }
            return bResultLoop;
        }
    }
}
