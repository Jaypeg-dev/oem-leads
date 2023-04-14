// VW Rwil Adaptor Preperations
using oemLeads.Commands.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

            while (!bResultLoop)
            {
                if (!RwilLeadGedaiAuth(ref RwilAccess_Token)) break;
                if (!RwilLeadCreateConsumer(RwilAccess_Token)) break;
                if (!KeyloopGatewayOAuth(ref Keyloop_Token)) break;
                if (!RwilGetServiceLeads(RwilAccess_Token, Keyloop_Token)) break;
                // Only a single loop so we can break when we dont want to continue and return result
                // TODO Next routine to know use the database and perform T4 T5 T6 T7 updates to Rwil using repair order
                bResultLoop = true;
            }

            return bResultLoop;
        }

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
            var bTesting = false  ;
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
            string? GedaiServiceLeadID = "";
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
                    if (!RwilProcessLead(Leads.ToString(), RwilToken, KeyloopToken, ref SABAppID, ref GedaiServiceLeadID))
                    {
                        bLeadProcessFail = true;
                        bResultLoop = false;
                        break;
                    }
                    // TODO Replace with database write
                    using StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(GedaiServiceLeadID + "," + SABAppID + ",T4");
                }
                if (bLeadProcessFail) break;
            }

            return bResultLoop;
        }

        public static bool RwilProcessLead(string RwilJasonSingleLead, JsonElement RwilToken, JsonElement KeyloopToken, ref string SABAppID, ref string? GedaiServiceLeadID)
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
                if (!RwilProcessLead_SAB(RwilLead, ref SABAppID, KeyloopToken)) break;
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

        public static bool RwilProcessLead_SAB(RwilSingleLead? RwilLead, ref string AppointmentID, JsonElement KeyloopToken)
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (RwilLead?.Payload?.Vehicle?.ModelCodes is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var MCodes in RwilLead?.Payload?.Vehicle?.ModelCodes)
                        RWModelCode = $"{MCodes?.ModelCode}";

#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (RwilLead?.Payload?.Vehicle?.EngineCodes is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var ECodes in RwilLead?.Payload?.Vehicle?.EngineCodes)
                        RWEngineCode = $"{ECodes?.EngineCode}";

#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (RwilLead?.Payload?.ListOfVehicleStatus is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                var sabrequest = new SABRequest()
                {
                    Details = new Details()
                    {
                        ExternalReference = "RWil Appoint Book",
                        DueInDateTime = DateTime.Now,
                        DueOutDateTime = DateTime.Now,
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
                            UserId = "api",
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (RwilLead?.Payload?.Vehicle?.ModelCodes is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var MCodes in RwilLead?.Payload?.Vehicle?.ModelCodes)
                        RWModelCode = $"{MCodes?.ModelCode}";

#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (RwilLead?.Payload?.ListOfVehicleStatus is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
                var appointmentrtc = new AppointmentRTC { AppointmentTypeId = "Service", AppointmentDateTime = DateTime.Now, AlternateAppointmentDateTime = DateTime.Now, Comments = $"Appointment ID: {SABAppID}" };
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

        static async Task<string> RwilProcessLead_SABAsync(JsonElement KeyloopAccess_Token, string SABRequest)
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

        static async Task<string> RwilProcessLead_RwilT0Async(JsonElement RwilToken, string RwilT0Request)
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

        static async Task<string> RwilProcessLead_KeyloopLeadAsync(string KeyloopLeadRequest)
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

        static async Task<string> RwilProcessLead_RwilOffsetAsync(JsonElement RwilToken, string RwilOffsetRequest)
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

        static async Task<string> RwilProcessLead_RwilT3Async(JsonElement RwilToken, string RwilT3Request)
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
    }
}