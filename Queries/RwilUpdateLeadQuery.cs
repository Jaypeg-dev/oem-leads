using oemLeads.Commands.Models;
using System.Text.Json;

namespace oemLeads.Queries
{
    internal class RwilUpdateLeadQuery : TRwilUpdateLeadQuery
    {
        public static bool RwilUpdated_ReadUpdateLead(ref JsonElement RwilAccess_Token, ref JsonElement Keyloop_Token)
        {
            bool bResultLoop = false, bUpdateFailed = false;
            string path = @"C:\Kerridge\", filename = "DataBase.txt";
            string GedaiServiceID = "", SABID = "", NextStage = "", sAppDAte = "";
            var RoResponse = "";

            while (!bResultLoop)
            {
                // Perform Auths for updates
                if (!RwilProcessLeadQuery.KeyloopGatewayOAuth(ref Keyloop_Token)) break;
                if (!RwilProcessLeadQuery.RwilLeadGedaiAuth(ref RwilAccess_Token)) break;
                // TODO Mock Database needs to move to platform database
                try
                {
                    using var sr = File.OpenText(path + filename);

                    {
                        var s = "";

                        while ((s = sr.ReadLine()) != null)
                        {
                            bUpdateFailed = true;
                            GedaiDataRetrieve(s, ref GedaiServiceID, ref SABID, ref sAppDAte, ref NextStage);
                            // Get the repair order async task needed using the SABID...
                            if (!Rwil_GetRepairOrderID(SABID, Keyloop_Token, ref RoResponse)) break;
                            if (!Rwil_PostTUpdates(NextStage, GedaiServiceID, sAppDAte, RwilAccess_Token, RoResponse)) break;
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

        public static void GedaiDataRetrieve(string DataStored, ref string GedaiServiceID, ref string SABID, ref string sAppointmentDate, ref string NextStage, string Delimenator = ",")
        {
            // Init the variables passed through first
            GedaiServiceID = "";
            SABID = "";
            NextStage = "";
            sAppointmentDate = "";
            // Split info string into the relevant fields
            var values = DataStored.Split(Delimenator);

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
                        sAppointmentDate = values[i].Trim();
                        break;
                    case 3:
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

        public static bool Rwil_PostTUpdates(string Stage, string GedaiServiceID, string sAppDate, JsonElement RwilToken, string ROResponse)
        {
            var bResultLoop = false;
            string path = @"C:\Kerridge\", filename = "DataBaseUpdate.txt";
            var updateStage = Stage;

            while (!bResultLoop)
            {
                    var ROJasonResponse = JsonSerializer.Deserialize<RepairOrderResponse>(ROResponse);

                    switch (Stage)
                    {
                        case "T4":
                            RwilUpdateLead_RwilT4(GedaiServiceID, ROJasonResponse, RwilToken, ref updateStage);
                            break;
                        case "T5":
                            RwilUpdateLead_RwilT5(GedaiServiceID, ROJasonResponse, RwilToken, ref updateStage);
                            break;
                        case "TBA":
                            // Monitor check T6, T7 and Repair Order Header Status - Closed Work Completed update 
                            RwilUpdateLead_RwilTBA(GedaiServiceID, ref sAppDate, ROJasonResponse, RwilToken, ref updateStage);
                            break;
                        default:
                            break;
                    }

                    // TODO need to replace this logic with update database from platform
                    if (!File.Exists(path + filename))
                    {
                        using var sw = File.CreateText(path + filename);
                    }

                    using (StreamWriter sw = File.AppendText(path + filename))
                    {
                        if (updateStage != "END")
                        {
                            sw.WriteLine($"{GedaiServiceID},{ROJasonResponse!.RepairOrderId},{ROJasonResponse?.Appointment.DueInDateTime},{updateStage}");
                        }
                    }
                    Thread.Sleep(2000);
                    bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT4(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken, ref string updateStage)
        {
            var bResultLoop = false;
            var RwilT4JsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilUpdateLead_RwilT4Request_Build(RepairOrderInfo, ref RwilT4JsonString, GedaiServiceID)) break;
                Console.WriteLine(RwilT4JsonString);
                // Async task needed here to perform the Rwil T4 Update, dont forget error handling
                var RwilT4task = RwilUpdateLead_RwilTUpdateAsync(RwilToken, RwilT4JsonString);
                var RwilT4RespJsonString = RwilT4task.GetAwaiter().GetResult();
                if (RwilT4RespJsonString == "-1") break;
                Console.WriteLine(RwilT4RespJsonString);
                bResultLoop = true;
            }

            if (bResultLoop) updateStage = "T5";
            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT4Request_Build(RepairOrderResponse? RepairOrderInfo, ref string RwilT4JsonString, string GedaiServiceID)
        {
            var bResultLoop = false;
            var sContactType = "";
            var Reason = "";

            while (!bResultLoop)
            {
                // TODO Repair order details notes to check Contacted Customer
                if (RepairOrderInfo?.Details.Notes == null) break;
                if (!RwilUpdateLead_CheckContactedNotes(RepairOrderInfo?.Details.Notes, ref sContactType, ref Reason)) break;

                var rwilt4 = new RwilT4()
                {
                    Event = new T4Event()
                    {
                        EventType = "T4",
                        ServiceLeadRecordID = GedaiServiceID,
                        Timestamp = DateTime.Now,
                        CustomerContactChannel = sContactType,
                    },
                };

                RwilT4JsonString = JsonSerializer.Serialize(rwilt4);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_CheckContactedNotes(string sNotes, ref string sContactType,ref string sReason)
        {
            var bResultLoop = false;
            var cancelType = "";

            while (!bResultLoop)
            {
                sContactType = "";
                if (!RwilUpdateLead_GetDetailsFromNotes(sNotes, ref sContactType, ref cancelType, ref sReason)) break;
                if (sContactType == "") break;

                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_GetDetailsFromNotes(string sNotes, ref string sContactType, ref string cancelType,ref string sReason, string Delimenator = ",", string fieldSeperator = ":")
        {
            var values = sNotes.Split(Delimenator);
            sContactType = "";
            cancelType = "";

            for (int i = 0; i < values.Length; i++)
            {
                if (i == 0)
                {
                    // Will always be Customer Contacted should be fixed not correct no details
                    var something = values[i].Trim();
                    if (!something.Contains("Customer Contacted")) return false;
                    if (something.Contains("Phone/Email")) { sContactType = ""; } else if (something.Contains("Phone")) { sContactType = "Phone"; } else if (something.Contains("Email")) { sContactType = "Email"; }
                }
                else
                {
                    // Cancellation types
                    var something = values[i].Trim();
                    var fieldCancellation = something.Split(fieldSeperator);
                    // Only 3 fields - type description and decision first check decision
                    if (fieldCancellation[2].Trim() == "Y" && fieldCancellation[2].Trim() != "Y/N") cancelType = fieldCancellation[0].Trim();
                    // Strip out the reason in the []
                    if (cancelType == "T5_C_21")
                    {
                        int Start, End;
                        Start = something.IndexOf('[', 1); End = something.IndexOf(']');
                        sReason = something.Substring(Start, End - Start);
                    };

                }
            }

            if (cancelType == "") cancelType = "T5_C_14";
            return true;
        }

        public static bool RwilUpdateLead_RwilT5(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken, ref string updateStage)
        {
            var bResultLoop = false;
            var bIndividualCheck = false;

            while (!bResultLoop)
            {
                switch (RepairOrderInfo?.Status)
                {
                    case "CREATED":
                        bIndividualCheck = RwilUpdateLead_RwilT5A(GedaiServiceID, RepairOrderInfo, RwilToken);
                        if (!bIndividualCheck)
                        {
                            if (!RwilUpdateLead_CheckedNoteItemsExist(RepairOrderInfo))
                            {
                                // Rwil notes has been deleted perform T5C check
                                bIndividualCheck = RwilUpdateLead_RwilT5C(GedaiServiceID, RepairOrderInfo, RwilToken);
                                if (bIndividualCheck) updateStage = "END";
                            }
                        }

                        break;
                    default:
                        if (!RwilUpdateLead_CheckedNoteItemsExist(RepairOrderInfo))
                        {
                            // Rwil notes has been deleted perform T5C check
                            bIndividualCheck = RwilUpdateLead_RwilT5C(GedaiServiceID, RepairOrderInfo, RwilToken);
                            if (bIndividualCheck) updateStage = "END";
                        }

                        break;
                }

                if (!bIndividualCheck) break;
                bResultLoop = true;
            }

            if (bResultLoop && updateStage != "END") updateStage = "TBA";
            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT5A(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken)
        {
            var bResultLoop = false;
            var RwilT5AJsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilUpdateLead_RwilT5ARequest_Build(RepairOrderInfo, ref RwilT5AJsonString, GedaiServiceID)) break;
                Console.WriteLine(RwilT5AJsonString);
                // Async task needed here to perform the Rwil T4 Update, dont forget error handling
                var RwilT5Atask = RwilUpdateLead_RwilTUpdateAsync(RwilToken, RwilT5AJsonString);
                var RwilT5ARespJsonString = RwilT5Atask.GetAwaiter().GetResult();
                if (RwilT5ARespJsonString == "-1") break;
                Console.WriteLine(RwilT5ARespJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_RwilT5ARequest_Build(RepairOrderResponse? RepairOrderInfo, ref string RwilT5AJsonString, string GedaiServiceID)
        {
            var bResultLoop = false;
            var sContactType = "";
            var Reason = "";

            while (!bResultLoop)
            {
                // Create routine to loop through job items 
                if (!RwilUpdateLead_CheckedJobItemsBooked(RepairOrderInfo)) break;
                RwilUpdateLead_CheckContactedNotes(RepairOrderInfo?.Details.Notes, ref sContactType, ref Reason);

                var t5a_rwilt5a = new T5A_RwilT5A()
                {
                    Event = new T5A_Event()
                    {
                        EventType = "T5_A",
                        ServiceLeadRecordID = GedaiServiceID,
                        Timestamp = DateTime.Now,
                        CustomerContactChannel = sContactType,
                        AppointmentDate = RwilUpdateLead_ConvertToTimeZone(RepairOrderInfo?.Appointment.DueInDateTime),
                        ServiceAdvisor = new T5A_ServiceAdvisor()
                        {
                            TelephoneNumber = "",
                            Name = RepairOrderInfo?.Resources.AssignedAdvisor.Name,
                            OrgGroup = "",
                        },
                        OptionalText = "",
                    },
                };

                RwilT5AJsonString = JsonSerializer.Serialize(t5a_rwilt5a);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_CheckedJobItemsBooked(RepairOrderResponse? RepairOrderInfo)
        {
            var bResultLoop = false;
            var bBooked = false;

            while (!bResultLoop)
            {
                // Create routine to loop through job items
                if (RepairOrderInfo?.Jobs is not null)
                {
                    foreach (var RepairOrderJob in RepairOrderInfo?.Jobs)
                    {
                        if (RepairOrderJob?.Labor is not null)
                        {
                            foreach (var ROLabor in RepairOrderJob?.Labor)
                            {
                                if (ROLabor.Status == "BOOKED")
                                {
                                    bBooked = true;
                                    break;
                                }
                            }
                        }

                        // loop through menu's as well
                        if (RepairOrderJob?.Menus is not null)
                        {
                            foreach (var ROMenus in RepairOrderJob?.Menus)
                            {
                                if (ROMenus?.Labor is not null)
                                {
                                    foreach (var ROMenusLab in ROMenus?.Labor)
                                    {
                                        if (ROMenusLab.Status == "BOOKED")
                                        {
                                            bBooked = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (bBooked) break;
                    }
                }

                // Searched and couldnt find anything booked
                if (!bBooked) break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilUpdateLead_CheckedNoteItemsExist(RepairOrderResponse? RepairOrderInfo)
        {
            var bResultLoop = false;
            var bNotes = false;

            while (!bResultLoop)
            {
                // Create routine to loop through notes items
                if (RepairOrderInfo?.Jobs is not null)
                {
                    foreach (var RepairOrderJob in RepairOrderInfo?.Jobs)
                    {
                        if (RepairOrderJob?.Notes is not null)
                        {
                            foreach (var RONotes in RepairOrderJob?.Notes)
                            {
                                if (RONotes is not null)
                                {
                                    bNotes = true;
                                    break;
                                }
                            }
                        }

                        // loop through menu's as well
                        if (RepairOrderJob?.Menus is not null)
                        {
                            foreach (var ROMenus in RepairOrderJob?.Menus)
                            {
                                if (ROMenus?.Notes is not null)
                                {
                                    foreach (var ROMenusNotes in ROMenus?.Notes)
                                    {
                                        if (ROMenusNotes is not null)
                                        {
                                            bNotes = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (bNotes) break;
                    }
                }

                // Searched and couldnt find anything
                if (!bNotes) break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

            public static bool RwilUpdateLead_RwilT5C(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken)
            {
                var bResultLoop = false;
                var RwilT5CJsonString = "";

                while (!bResultLoop)
                {
                    if (!RwilUpdateLead_RwilT5CRequest_Build(RepairOrderInfo, ref RwilT5CJsonString, GedaiServiceID)) break;
                    Console.WriteLine(RwilT5CJsonString);
                    // Async task needed here to perform the Rwil T5C Update, dont forget error handling
                    var RwilT5Ctask = RwilUpdateLead_RwilTUpdateAsync(RwilToken, RwilT5CJsonString);
                    var RwilT5CRespJsonString = RwilT5Ctask.GetAwaiter().GetResult();
                    if (RwilT5CRespJsonString == "-1") break;
                    Console.WriteLine(RwilT5CRespJsonString);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_RwilT5CRequest_Build(RepairOrderResponse? RepairOrderInfo, ref string RwilT5CJsonString, string GedaiServiceID)
            {
                // The following needs to be done after appointment created to patch the detail notes which is used in this routine
                // Online Booking
                // Customer Contacted: Phone/Email,
                // T5_C_14: No appointment needed: Y/N,
                // T5_C_15: No appointment wanted: Y/N,
                // T5_C_16: Customer not reachable after # contact attempts: Y/N,
                // T5_C_17: Contact channels no longer valid: Y/N,
                // T5_C_18: Customer no longer in possesion of vehicle: Y/N
                // T5_C_19: Duplicate lead: Y/N
                // T5_C_20: Reject lead: Y/N
                // T5_C_21: Other reason: Y/N [reason text]
                // OptionaltText assumed to be the correct place

                var bResultLoop = false;
                string sContactType = "", cancelType = "", sReason = "";

                while (!bResultLoop)
                {
                    RwilUpdateLead_GetDetailsFromNotes(RepairOrderInfo?.Details.Notes, ref sContactType, ref cancelType, ref sReason);
                    if (cancelType == "") break;

                    var t5c_rwilt5c = new T5C_RwilT5C()
                    {
                        Event = new T5C_Event()
                        {
                            EventType = "T5_C",
                            RejectionCode = cancelType,
                            ServiceLeadRecordID = GedaiServiceID,
                            Timestamp = DateTime.Now,
                            CustomerContactChannel = sContactType,
                            AdditionalReason = "DMS Repair Order Cancelled",
                            OptionalText = sReason,
                        },
                    };

                    RwilT5CJsonString = JsonSerializer.Serialize(t5c_rwilt5c);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_RwilTBA(string GedaiServiceID, ref string sAppDate, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken, ref string updateStage)
            {
                var bResultLoop = false;
                var bIndividualCheck = false;

                while (!bResultLoop)
                {
                    switch (RepairOrderInfo?.Status)
                    {
                        case "WORKCOMPLETED":
                            bIndividualCheck = true;
                            updateStage = "END";
                            break;
                        case "CLOSED":
                            bIndividualCheck = true;
                            updateStage = "END";
                            break;
                        default:
                            if (RwilUpdateLead_CheckAppointmentChanged(ref sAppDate, RepairOrderInfo))
                            {
                                // T6 update
                                bIndividualCheck = RwilUpdateLead_RwilT6(GedaiServiceID, RepairOrderInfo, RwilToken);
                                updateStage = "TBA";
                            }
                            else if (!RwilUpdateLead_CheckedJobItemsBooked(RepairOrderInfo))
                            {
                                // T7 update
                                bIndividualCheck = RwilUpdateLead_RwilT7(GedaiServiceID, RwilToken);
                                updateStage = "END";
                            }

                            break;
                    }

                    if (!bIndividualCheck) break;
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_CheckAppointmentChanged(ref string sAppDate, RepairOrderResponse? RepairOrderInfo)
            {
                var bDateChanged = false;
                var OriginalAppDate = DateTime.Parse(sAppDate);
                var CurrentAppDate = DateTime.Parse(RepairOrderInfo?.Appointment.DueInDateTime);

                if (OriginalAppDate != CurrentAppDate)
                {
                    sAppDate = RepairOrderInfo?.Appointment.DueInDateTime;
                    bDateChanged = true;
                }

                return bDateChanged;
            }

            public static bool RwilUpdateLead_RwilT6(string GedaiServiceID, RepairOrderResponse? RepairOrderInfo, JsonElement RwilToken)
            {
                var bResultLoop = false;
                var RwilT6JsonString = "";

                while (!bResultLoop)
                {
                    // Reminder Logic Done here
                    if (!RwilUpdateLead_RwilT6Request_Build(RepairOrderInfo, ref RwilT6JsonString, GedaiServiceID)) break;
                    Console.WriteLine(RwilT6JsonString);
                    // Async task needed here to perform the Rwil T6 Update, dont forget error handling
                    var RwilT6task = RwilUpdateLead_RwilTUpdateAsync(RwilToken, RwilT6JsonString);
                    var RwilT6RespJsonString = RwilT6task.GetAwaiter().GetResult();
                    if (RwilT6RespJsonString == "-1") break;
                    Console.WriteLine(RwilT6RespJsonString);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_RwilT6Request_Build(RepairOrderResponse? RepairOrderInfo, ref string RwilT6JsonString, string GedaiServiceID)
            {
                var bResultLoop = false;

                while (!bResultLoop)
                {
                    var t6_rwilt6 = new T6_RwilT6()
                    {
                        Event = new T6_Event()
                        {
                            EventType = "T6",
                            ServiceLeadRecordID = GedaiServiceID,
                            Timestamp = DateTime.Now,
                            AppointmentDate = RwilUpdateLead_ConvertToTimeZone(RepairOrderInfo?.Appointment.DueInDateTime),
                            OptionalText = "",
                        },
                    };

                    RwilT6JsonString = JsonSerializer.Serialize(t6_rwilt6);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_RwilT7(string GedaiServiceID, JsonElement RwilToken)
            {
                var bResultLoop = false;
                var RwilT7JsonString = "";

                while (!bResultLoop)
                {
                    // Reminder Logic Done here
                    if (!RwilUpdateLead_RwilT7Request_Build(ref RwilT7JsonString, GedaiServiceID)) break;
                    Console.WriteLine(RwilT7JsonString);
                    // Async task needed here to perform the Rwil T4 Update, dont forget error handling
                    var RwilT7task = RwilUpdateLead_RwilTUpdateAsync(RwilToken, RwilT7JsonString);
                    var RwilT7RespJsonString = RwilT7task.GetAwaiter().GetResult();
                    if (RwilT7RespJsonString == "-1") break;
                    Console.WriteLine(RwilT7RespJsonString);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool RwilUpdateLead_RwilT7Request_Build(ref string RwilT7JsonString, string GedaiServiceID)
            {
                var bResultLoop = false;

                while (!bResultLoop)
                {
                    var t7_rwilt7 = new T7_RwilT7()
                    {
                        Event = new T7_Event()
                        {
                            EventType = "T7",
                            ServiceLeadRecordID = GedaiServiceID,
                            Timestamp = DateTime.Now,
                            OptionalText = "",
                        },
                    };

                    RwilT7JsonString = JsonSerializer.Serialize(t7_rwilt7);
                    bResultLoop = true;
                }

                return bResultLoop;
            }

            public static bool Rwil_CheckReplaceDataFile(string oldfile, string newfile)
            {
                var bResultLoop = false;

                while (!bResultLoop)
                {
                    var path = newfile;
                    var path2 = oldfile;

                    try
                    {
                        if (!File.Exists(path))
                        {
                            using var fs = File.Create(path);
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

            public static string RwilUpdateLead_CurrentDateTime()
            {
                var dt1 = DateTime.Now;
                return dt1.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
            }
            public static DateTime RwilUpdateLead_ConvertToTimeZone(string sTimeWithZ)
            {
                DateTime dt1 = DateTime.Now;
                System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(sTimeWithZ);
                strBuilder[sTimeWithZ.IndexOf('Z')] = 'z';
                string adjusted_dmsdateformat = strBuilder.ToString();
                DateTime ConvertedDateTime = DateTime.Parse(dt1.ToString(adjusted_dmsdateformat));
                return ConvertedDateTime;
            }
        }
}