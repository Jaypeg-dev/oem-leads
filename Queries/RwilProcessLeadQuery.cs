using oemLeads.Commands.Models;
using System.Text.Json;

namespace oemLeads.Queries
{
    public class RwilProcessLeadQuery : TRwilProcessLeadQuery
    {

        public static bool RwilLeadGedaiAuth(ref JsonElement RwilAccess_Token)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var Rwiltask = RwilLeadGedaiAuthAsync();
                var sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Rwil Gedai Auth = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                if (sResponse == "-1") break;
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
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var Rwiltask = RwilLeadCreateConsumerAsync(RwilToken);
                var sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Rwil Create Consumer = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                if (sResponse == "-1") break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool KeyloopGatewayOAuth(ref JsonElement KeyloopAccess_Token)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var Rwiltask = KeyloopGatewayOAuthAsync();
                var sResponse = Rwiltask.GetAwaiter().GetResult();
                Console.WriteLine("Keyloop Gateway Auth = " + sResponse);
                // Some code to check if (faliure), not sure how the reponse look like for failure
                if (sResponse == "-1") break;
                var KeyloopAuthObject = System.Text.Json.JsonDocument.Parse(sResponse);
                // retrieve the value
                KeyloopAccess_Token = KeyloopAuthObject.RootElement.GetProperty("access_token");
                Console.WriteLine("Keyloop access token: " + KeyloopAccess_Token);
                // Only a single loop so we can break when we dont want to continue and return result
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilGetServiceLeads(JsonElement RwilToken, JsonElement KeyloopToken)
        {
            var bResultLoop = false;
            var bTesting = false;
            string sResponse;

            while (!bResultLoop)
            {
                if (bTesting)
                {
                    sResponse = File.ReadAllText(@"c:\kerridge\rwil-leads.json");
                }
                else
                {
                    var Rwiltask = RwilGetServiceLeadsAsync(RwilToken);
                    sResponse = Rwiltask.GetAwaiter().GetResult();
                }

                Console.WriteLine("Rwil Gedai Sevice Leads: " + sResponse);
                if (sResponse == "-1") break;
                if (!RwilReadLeads(sResponse, RwilToken, KeyloopToken)) break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilReadLeads(string RwilLeadjsonString, JsonElement RwilToken, JsonElement KeyloopToken)
        {
            var bResultLoop = false;
            var bLeadProcessFail = false;
            string SABAppID = "", path = @"C:\Kerridge\DataBase.txt";
            string GedaiServiceLeadID = "";
            string sAppDate = "";
            // TODO This needs to be replaced with database model check if (file doesnt exist create it)
            if (!File.Exists(path))
            {
                using StreamWriter sw = File.CreateText(path);
            }
            while (!bResultLoop)
            {
                using var RwilLeads = JsonDocument.Parse(RwilLeadjsonString);
                var RwilLeadsRoot = RwilLeads.RootElement;
                var count = RwilLeadsRoot.GetArrayLength();

                if (count == 0) break;
                else if (count > 0) bResultLoop = true;

                foreach (JsonElement Leads in RwilLeadsRoot.EnumerateArray())
                {
                    // Routine to process each lead
                    if (!RwilProcessSingleLead(Leads.ToString(), RwilToken, KeyloopToken, ref SABAppID, ref GedaiServiceLeadID,ref sAppDate))
                    {
                        bLeadProcessFail = true;
                        bResultLoop = false;
                        break;
                    }
                    // TODO Replace with database write
                    using StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(GedaiServiceLeadID + "," + SABAppID + "," + sAppDate + ",T4");
                }
                if (bLeadProcessFail) break;
            }

            return bResultLoop;
        }

        public static bool RwilProcessSingleLead(string RwilJasonSingleLead, JsonElement RwilToken, JsonElement KeyloopToken, ref string SABAppID, ref string? GedaiServiceLeadID,ref string? sAppDate)
        {
            var bResultLoop = false;
            var RwilLead = JsonSerializer.Deserialize<RwilSingleLead>(RwilJasonSingleLead);

            while (!bResultLoop)
            {
                Console.WriteLine("");
                Console.WriteLine("Start Processing Lead");
                // Set the gedai service lead ID
                GedaiServiceLeadID = RwilLead?.Payload?.ServiceLeadRecordID;
                // SAB Method
                Console.WriteLine("");
                if (!RwilProcessLead_SAB(RwilLead, ref SABAppID,ref sAppDate, KeyloopToken)) break;
                // Update Repair Order Notes
                if (!RwilProcessLead_RepairOrderDetails(SABAppID, KeyloopToken)) break;
                // T0 Update to Rwil Request
                Console.WriteLine("");
                if (!RwilProcessLead_RwilT0(RwilLead, RwilToken)) break;
                // RTC Method
                Console.WriteLine("");
                if (!RwilProcessLead_KeyloopLead(RwilLead, SABAppID)) break;
                // Rwil Offset Commit
                Console.WriteLine("");
                if (!RwilProcessLead_RwilOffset(RwilLead, RwilToken)) break;
                // T3 Update to Rwil Request
                Console.WriteLine("");
                if (!RwilProcessLead_RwilT3(RwilLead, RwilToken)) break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_SAB(RwilSingleLead? RwilLead, ref string AppointmentID, ref string AppointmentDate, JsonElement KeyloopToken)
        {
            var bResultLoop = false;
            var bTesting = false;
            var SABReqJsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_SABRequest_Build(RwilLead, ref SABReqJsonString)) break;
                string SABResJsonString;

                if (bTesting)
                {
                    SABResJsonString = File.ReadAllText(@"c:\kerridge\rwil-sab-response.json");
                }
                else
                {
                    var SABtask = RwilProcessLead_SABAsync(KeyloopToken, SABReqJsonString);
                    SABResJsonString = SABtask.GetAwaiter().GetResult();
                }

                if (SABResJsonString == "-1") break;
                var sabresponse = JsonSerializer.Deserialize<SABResponse>(SABResJsonString);
                Console.WriteLine($"Appointment ID: {sabresponse?.AppointmentId}");
                AppointmentID = $"{sabresponse?.AppointmentId}";
                AppointmentDate = $"{sabresponse?.Details.DueInDateTime}";
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_SABRequest_Build(RwilSingleLead? RwilLead, ref string SABReqJsonString)
        {
            var bResultLoop = false;
            string RWFirstName = "", RWLastName = "", RWPhone = "", RWEmail = "", RWEngineCode = "", RWModelCode = "", RWOdoMeterUnitMeasure = "", RWMileage = "0";

            while (!bResultLoop)
            {
                // Perform a single loop to reduce process and store info in variables
                if (RwilLead?.Payload?.BusinessPartners is not null)
                {
                    foreach (var BPCustomer in RwilLead?.Payload?.BusinessPartners)
                    {
                        RWFirstName = $"{BPCustomer?.FirstName}";
                        RWLastName = $"{BPCustomer?.LastName}";

                        if (BPCustomer?.CommunicationChannels is not null)
                        {
                            foreach (var CommsInfo in BPCustomer?.CommunicationChannels)
                            {
                                switch (CommsInfo?.ChannelType)
                                {
                                    case "Phone":
                                        RWPhone = $"{CommsInfo?.ChannelValue}";
                                        break;
                                    case "Email":
                                        RWEmail = $"{CommsInfo?.ChannelValue}";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                if (RwilLead?.Payload?.Vehicle?.ModelCodes is not null)
                {
                    foreach (var MCodes in RwilLead?.Payload?.Vehicle?.ModelCodes)
                        RWModelCode = $"{MCodes?.ModelCode}";
                }

                if (RwilLead?.Payload?.Vehicle?.EngineCodes is not null)
                {
                    foreach (var ECodes in RwilLead?.Payload?.Vehicle?.EngineCodes)
                        RWEngineCode = $"{ECodes?.EngineCode}";
                }

                if (RwilLead?.Payload?.ListOfVehicleStatus is not null)
                {
                    foreach (var LVStatus in RwilLead?.Payload?.ListOfVehicleStatus)
                    {
                        RWMileage = $"{LVStatus?.OdometerValue}";

                        RWOdoMeterUnitMeasure = (LVStatus?.OdometerValueUnit) switch
                        {
                            "km" => "KM",
                            "mi" => "MI",
                            _ => $"{LVStatus?.OdometerValueUnit}",
                        };
                    }
                }

                if (RWFirstName == "" && RWLastName == "") break;
                if (RWPhone == "" && RWEmail == "") break;

                var sabrequest = new SABRequest()
                {
                    Details = new Details()
                    {
                        ExternalReference = "RWil Appoint Book",
                        DueInDateTime = RwilProcessLead_CurrentDateTime(),
                        DueOutDateTime = RwilProcessLead_CurrentDateTime(),
                        AdvisorRequired = false,
                        ContactAdvisorId = "",
                    },
                    Customer = new Customer()
                    {
                        CustomerId = "",
                        Names = new Names()
                        {
                            FamilyName = RWLastName,
                            GivenName = RWFirstName,
                        },
                        Class = "CAR",
                        Address = new Address()
                        {
                            StreetName = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.Street}",
                            HouseNumber = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.HouseNumber}",
                            PostalCode = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.PostalCode}",
                            City = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.City}",
                            County = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.Country}",
                            Province = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.CustomerAddress?.State}",
                        },
                        ContactInformation = new ContactInformation()
                        {
                            Mobile = RWPhone,
                            Email = RWEmail,
                        },
                        ExternalReference = new ExternalReference()
                        {
                            ProviderCode = "RiWIL",
                            ApplicationCode = $"{RwilLead?.Header?.Source}",
                            Value = $"{RwilLead?.Header?.MessageId}",
                        },
                    },
                    Vehicle = new VehicleSAB()
                    {
                        VehicleId = "",
                        Class = "CAR",
                        Description = $"{RwilLead?.Payload?.Vehicle?.ModelName}",
                        Identification = new Identification()
                        {
                            Vin = $"{RwilLead?.Payload?.Vehicle?.Vin}",
                            LicensePlate = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.LicensePlate}",
                            EngineNumber = RWEngineCode,
                        },
                        Mileage = new Mileage()
                        {
                            Unit = RWOdoMeterUnitMeasure,
                            Value = int.Parse(RWMileage),
                        },
                    },
                    BookingOption = new BookingOption(),
                    ServiceProducts = new List<object>(),
                    AdditionalServiceProducts = new List<AdditionalServiceProduct>(),
                    Metadata = new Metadata()
                    {
                        AuditData = new AuditData()
                        {
                            // Need to consider parametizing this user ID 
                            UserId = "apiuser",
                            UserName = "API Default User",
                        },
                    },
                };

                var AdditionalSerProd = new AdditionalServiceProduct { ServiceProductId = "", CustomerNotes = $"Appointment Created from RWil Lead {RWModelCode}" };
                sabrequest.AdditionalServiceProducts.Add(AdditionalSerProd);
                SABReqJsonString = JsonSerializer.Serialize(sabrequest);
                Console.WriteLine(SABReqJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }


        public static bool RwilProcessLead_RepairOrderDetails(string AppointmentID, JsonElement KeyloopToken)
        {
            var bResultLoop = false;
            var RODReqJsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_RODetailsRequest_Build(ref RODReqJsonString)) break;

                var RODtask = RwilProcessLead_PatchRODetailsAsync(KeyloopToken, RODReqJsonString, AppointmentID);
                string RODResJsonString = RODtask.GetAwaiter().GetResult();
                if (RODResJsonString == "-1") break;
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RODetailsRequest_Build(ref string RODReqJsonString)
        {
            var bResultLoop = false;
            

            while (!bResultLoop)
            {
                // Perform a single loop to reduce process and store info in variables
                var repairorderdetailsreq = new RepairOrderDetailsReq()
                {
                    Notes = "Customer Contacted: Phone/Email,\n T5_C_14: No appointment needed: Y/N,\n T5_C_15: No appointment wanted: Y/N,\n T5_C_16: Customer not reachable after # contact attempts: Y/N,\n T5_C_17: Contact channels no longer valid: Y/N,\n T5_C_18: Customer no longer in possesion of vehicle: Y/N",
                };
                RODReqJsonString = JsonSerializer.Serialize(repairorderdetailsreq);
                Console.WriteLine(RODReqJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilT0(RwilSingleLead? RwilLead, JsonElement RwilToken)
        {
            var bResultLoop = false;
            var RwilT0JsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_RwilT0Request_Build(RwilLead, ref RwilT0JsonString)) break;
                Console.WriteLine(RwilT0JsonString);
                // Async task needed here to perform the Rwil T0 Update, dont forget error handling
                var RwilT0task = RwilProcessLead_RwilT0Async(RwilToken, RwilT0JsonString);
                var RwilT0RespJsonString = RwilT0task.GetAwaiter().GetResult();
                if (RwilT0RespJsonString == "-1") break;
                Console.WriteLine(RwilT0RespJsonString);

                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilT0Request_Build(RwilSingleLead? RwilLead, ref string RwilT0JsonString)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var rwilt0 = new RwilT0()
                {
                    Event = new Event()
                    {
                        EventType = "T0",
                        ServiceLeadRecordID = RwilLead?.Payload?.ServiceLeadRecordID,
                        ReceivingStatus = "Confirmation",
                        Timestamp = DateTime.Now,
                    },
                };

                RwilT0JsonString = JsonSerializer.Serialize(rwilt0);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_KeyloopLead(RwilSingleLead? RwilLead, string? SABAppID = null)
        {
            var bResultLoop = false;
            var bTesting = false;
            var KeyloopJsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_KeyloopLeadRequest_Build(RwilLead, ref KeyloopJsonString, SABAppID)) break;

                string RTCResJsonString;

                if (bTesting)
                {
                    RTCResJsonString = File.ReadAllText(@"c:\kerridge\rwil-rtc-response.json");
                }
                else
                {
                    var KeyloopLeadtask = RwilProcessLead_KeyloopLeadAsync(KeyloopJsonString);
                    RTCResJsonString = KeyloopLeadtask.GetAwaiter().GetResult();
                }

                if (RTCResJsonString == "-1") break;
                var RTCResponse = JsonSerializer.Deserialize<RTCKeyloopLeadRes>(RTCResJsonString);
                Console.WriteLine($"RTC Lead ID: {RTCResponse?.LeadGuid}");

                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_KeyloopLeadRequest_Build(RwilSingleLead? RwilLead, ref string KeyloopJsonString, string? SABAppID = null)
        {
            var bResultLoop = false;
            string RWFirstName = "", RWLastName = "", RWPhone = "", RWEmail = "", RWModelCode = "", RWOdoMeterUnitMeasure = "", RWMileage = "", RWTrans = "";

            while (!bResultLoop)
            {
                // Perform a single loop to reduce process and store info in variables
                if (RwilLead?.Payload?.BusinessPartners is not null)
                {
                    foreach (var BPCustomer in RwilLead?.Payload?.BusinessPartners)
                    {
                        RWFirstName = $"{BPCustomer?.FirstName}";
                        RWLastName = $"{BPCustomer?.LastName}";

                        if (BPCustomer?.CommunicationChannels is not null)
                        {
                            foreach (var CommsInfo in BPCustomer?.CommunicationChannels)
                            {
                                switch (CommsInfo?.ChannelType)
                                {
                                    case "Phone":
                                        RWPhone = $"{CommsInfo?.ChannelValue}";
                                        break;
                                    case "Email":
                                        RWEmail = $"{CommsInfo?.ChannelValue}";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                if (RwilLead?.Payload?.Vehicle?.ModelCodes is not null)
                {
                    foreach (var MCodes in RwilLead?.Payload?.Vehicle?.ModelCodes)
                        RWModelCode = $"{MCodes?.ModelCode}";
                }

                if (RwilLead?.Payload?.ListOfVehicleStatus is not null)
                {
                    foreach (var LVStatus in RwilLead?.Payload?.ListOfVehicleStatus)
                    {
                        RWTrans = $"{LVStatus?.Actuation}";
                        RWMileage = $"{LVStatus?.OdometerValue}";

                        RWOdoMeterUnitMeasure = (LVStatus?.OdometerValueUnit) switch
                        {
                            "km" => "KM",
                            "mi" => "MI",
                            _ => $"{LVStatus?.OdometerValueUnit}",
                        };
                    }
                }

                // RTC Logic to build the RTC Lead using Rwil and SAB Response
                var rtckeyloopleadreq = new RTCKeyloopLeadReq()
                {
                    EventDate = RwilLead?.Header?.Created,
                    Salutation = null,
                    FirstName = RWFirstName,
                    MiddleName = null,
                    LastName = RWLastName,
                    LeadSource = new LeadSourceRTC()
                    {
                        // TODO Speak with Jules on why we hard coded this ID in the POSTMAN request
                        LeadSourceGuid = "3e68016d-74c6-453d-87a4-af9c01347e39",
                    },
                    LangId = 2,
                    PreferredVehiclePaymentType = 0,
                    LeadEmailList = new List<LeadEmailListRTC>(),
                    LeadPhoneList = new List<LeadPhoneListRTC>(),
                    Consent = new ConsentRTC()
                    {
                        IsDealerMarketable = null,
                        IsDealerEmailMarketable = null,
                        IsDealerPhoneMarketable = null,
                        IsDealerSmsMarketable = null,
                        IsDealerPostalMailMarketable = null,
                        IsOemMarketable = null,
                        IsOemEmailMarketable = null,
                        IsOemPhoneMarketable = null,
                        IsOemSmsMarketable = null,
                        IsOemPostalMailMarketable = null,
                    },
                    // Will build this on the outside
                    Vehicles = new List<VehicleRTC>(),
                    PurchaseHorizon = null,
                    Comments = new List<object>(),
                    // Will build this on the outside
                    Appointments = new List<AppointmentRTC>(),
                    // Will build this on the outside
                    LeadPropertyValues = new List<LeadPropertyValueRTC>(),
                };

                var leademaillistrtc = new LeadEmailListRTC { EmailAddress = RWEmail };
                rtckeyloopleadreq.LeadEmailList.Add(leademaillistrtc);
                var leadphonelistrtc = new LeadPhoneListRTC { PhoneNumber = RWPhone, PhoneExtension = "", LeadPhoneTypeId = 1 };
                rtckeyloopleadreq.LeadPhoneList.Add(leadphonelistrtc);

                var vehiclertc = new VehicleRTC
                {
                    Year = RwilLead?.Payload?.Vehicle?.ModelYear,
                    Make = $"{RwilLead?.Payload?.Vehicle?.Brand}",
                    Model = $"{RwilLead?.Payload?.Vehicle?.ModelName}",
                    ModelCode = RWModelCode,
                    ModelGroup = "",
                    Trim = "",
                    BodyStyle = "",
                    Transmission = RWTrans,
                    StockTypeId = 2,
                    Vin = $"{RwilLead?.Payload?.Vehicle?.Vin}",
                    VehicleRegistrationNumber = $"{RwilLead?.Payload?.AdditionalServiceLeadInformation?.LicensePlate}",
                    LeadVehicleTypeId = 1,
                    StockNumber = "",
                    FuelTypeCode = "",
                    Odometer = new OdometerRTC()
                    {
                        Value = RWMileage,
                        MeaseureTypeId = RWOdoMeterUnitMeasure,
                    },
                    OfferPrice = null,
                    SellingPrice = null,
                    AppraisalPrice = null,
                    Options = new List<object>(),
                    Comments = new List<object>(),
                    Financing = null
                };

                rtckeyloopleadreq.Vehicles.Add(vehiclertc);
                var appointmentrtc = new AppointmentRTC { AppointmentTypeId = "Service", AppointmentDateTime = RwilProcessLead_CurrentDateTime(), AlternateAppointmentDateTime = RwilProcessLead_CurrentDateTime(), Comments = $"Appointment ID: {SABAppID}" };
                rtckeyloopleadreq.Appointments.Add(appointmentrtc);

                var leadpropertyvaluertc = new LeadPropertyValueRTC
                {
                    LeadPropertyField = new LeadPropertyFieldRTC()
                    {
                        LeadProperty = new LeadPropertyRTC()
                        {
                            Name = "ThirdPartyLeadId",
                        },
                        LeadPropertyFieldName = "ThirdPartyLeadId_Value",
                    },
                    Value = $"{RwilLead?.Payload?.ServiceLeadRecordID}",
                };

                rtckeyloopleadreq.LeadPropertyValues.Add(leadpropertyvaluertc);
                KeyloopJsonString = JsonSerializer.Serialize(rtckeyloopleadreq);
                Console.WriteLine(KeyloopJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilOffset(RwilSingleLead? RwilLead, JsonElement RwilToken)
        {
            var bResultLoop = false;
            var RwilOffsetJsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_RwilOffsetRequest_Build(RwilLead, ref RwilOffsetJsonString)) break;
                Console.WriteLine(RwilOffsetJsonString);
                // Async task needed here to perform the Rwil T0 Update, dont forget error handling
                var RwilT0task = RwilProcessLead_RwilOffsetAsync(RwilToken, RwilOffsetJsonString);
                var RwilT0RespJsonString = RwilT0task.GetAwaiter().GetResult();
                if (RwilT0RespJsonString == "-1") break;
                Console.WriteLine(RwilT0RespJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilOffsetRequest_Build(RwilSingleLead? RwilLead, ref string RwilOffsetJsonString)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var rwilloffsetcommit = new RwillOffsetCommit()
                {
                    Offsets = new List<OffsetC>(),
                };

                var rwiloffset = new OffsetC { BoType = "serviceLead", Offset = (RwilLead?.Header?.Offset + 1) };
                rwilloffsetcommit.Offsets.Add(rwiloffset);
                RwilOffsetJsonString = JsonSerializer.Serialize(rwilloffsetcommit);

                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilT3(RwilSingleLead? RwilLead, JsonElement RwilToken)
        {
            var bResultLoop = false;
            var RwilT3JsonString = "";

            while (!bResultLoop)
            {
                // Reminder Logic Done here
                if (!RwilProcessLead_RwilT3Request_Build(RwilLead, ref RwilT3JsonString)) break;
                Console.WriteLine(RwilT3JsonString);
                // Async task needed here to perform the Rwil T0 Update, dont forget error handling
                var RwilT3task = RwilProcessLead_RwilT3Async(RwilToken, RwilT3JsonString);
                var RwilT3RespJsonString = RwilT3task.GetAwaiter().GetResult();
                if (RwilT3RespJsonString == "-1") break;
                Console.WriteLine(RwilT3RespJsonString);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead_RwilT3Request_Build(RwilSingleLead? RwilLead, ref string RwilT3JsonString)
        {
            var bResultLoop = false;

            while (!bResultLoop)
            {
                var rwilt3 = new RwilT3()
                {
                    Event = new EventT3()
                    {
                        EventType = "T3",
                        ServiceLeadRecordID = RwilLead?.Payload?.ServiceLeadRecordID,
                        Timestamp = DateTime.Now,
                    },
                };

                RwilT3JsonString = JsonSerializer.Serialize(rwilt3);
                bResultLoop = true;
            }

            return bResultLoop;
        }

        public static string RwilProcessLead_CurrentDateTime()
        {
            DateTime dt1 = DateTime.Now;
            return dt1.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
        }
        public static DateTime RwilProcessLead_ConvertToTimeZone(string sTimeWithZ)
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
