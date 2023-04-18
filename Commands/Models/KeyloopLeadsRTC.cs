using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace oemLeads.Commands.Models
{
    // Model Class for (RTC Lead Request)
    // RTCKeyloopLeadReq myDeserializedClass = JsonSerializer.Deserialize<RTCKeyloopLeadReq>(myJsonResponse);
    public class RTCKeyloopLeadReq
    {
        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; }

        [JsonPropertyName("salutation")]
        public object? Salutation { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("middleName")]
        public object? MiddleName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("leadSource")]
        public LeadSourceRTC? LeadSource { get; set; }

        [JsonPropertyName("langId")]
        public int? LangId { get; set; }

        [JsonPropertyName("preferredVehiclePaymentType")]
        public int? PreferredVehiclePaymentType { get; set; }

        [JsonPropertyName("leadEmailList")]
        public List<LeadEmailListRTC>? LeadEmailList { get; set; }

        [JsonPropertyName("leadPhoneList")]
        public List<LeadPhoneListRTC>? LeadPhoneList { get; set; }

        [JsonPropertyName("consent")]
        public ConsentRTC? Consent { get; set; }

        [JsonPropertyName("vehicles")]
        public List<VehicleRTC>? Vehicles { get; set; }

        [JsonPropertyName("purchaseHorizon")]
        public object? PurchaseHorizon { get; set; }

        [JsonPropertyName("comments")]
        public List<object>? Comments { get; set; }

        [JsonPropertyName("appointments")]
        public List<AppointmentRTC>? Appointments { get; set; }

        [JsonPropertyName("leadPropertyValues")]
        public List<LeadPropertyValueRTC>? LeadPropertyValues { get; set; }
    }

    public class AppointmentRTC
    {
        [JsonPropertyName("appointmentTypeId")]
        public string? AppointmentTypeId { get; set; }

        [JsonPropertyName("appointmentDateTime")]
        public string AppointmentDateTime { get; set; }

        [JsonPropertyName("alternateAppointmentDateTime")]
        public string AlternateAppointmentDateTime { get; set; }

        [JsonPropertyName("comments")]
        public string? Comments { get; set; }
    }

    public class ConsentRTC
    {
        [JsonPropertyName("isDealerMarketable")]
        public object? IsDealerMarketable { get; set; }

        [JsonPropertyName("isDealerEmailMarketable")]
        public object? IsDealerEmailMarketable { get; set; }

        [JsonPropertyName("isDealerPhoneMarketable")]
        public object? IsDealerPhoneMarketable { get; set; }

        [JsonPropertyName("isDealerSmsMarketable")]
        public object? IsDealerSmsMarketable { get; set; }

        [JsonPropertyName("isDealerPostalMailMarketable")]
        public object? IsDealerPostalMailMarketable { get; set; }

        [JsonPropertyName("isOemMarketable")]
        public object? IsOemMarketable { get; set; }

        [JsonPropertyName("isOemEmailMarketable")]
        public object? IsOemEmailMarketable { get; set; }

        [JsonPropertyName("isOemPhoneMarketable")]
        public object? IsOemPhoneMarketable { get; set; }

        [JsonPropertyName("isOemSmsMarketable")]
        public object? IsOemSmsMarketable { get; set; }

        [JsonPropertyName("isOemPostalMailMarketable")]
        public object? IsOemPostalMailMarketable { get; set; }
    }

    public class LeadEmailListRTC
    {
        [JsonPropertyName("emailAddress")]
        public string? EmailAddress { get; set; }
    }

    public class LeadPhoneListRTC
    {
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("phoneExtension")]
        public string? PhoneExtension { get; set; }

        [JsonPropertyName("leadPhoneTypeId")]
        public int? LeadPhoneTypeId { get; set; }
    }

    public class LeadPropertyRTC
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class LeadPropertyFieldRTC
    {
        [JsonPropertyName("leadProperty")]
        public LeadPropertyRTC? LeadProperty { get; set; }

        [JsonPropertyName("leadPropertyFieldName")]
        public string? LeadPropertyFieldName { get; set; }
    }

    public class LeadPropertyValueRTC
    {
        [JsonPropertyName("leadPropertyField")]
        public LeadPropertyFieldRTC? LeadPropertyField { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class LeadSourceRTC
    {
        [JsonPropertyName("leadSourceGuid")]
        public string? LeadSourceGuid { get; set; }
    }

    public class OdometerRTC
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("MeaseureTypeId")]
        public string? MeaseureTypeId { get; set; }
    }

    public class VehicleRTC
    {
        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonPropertyName("make")]
        public string? Make { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("modelCode")]
        public string? ModelCode { get; set; }

        [JsonPropertyName("modelGroup")]
        public string? ModelGroup { get; set; }

        [JsonPropertyName("trim")]
        public string? Trim { get; set; }

        [JsonPropertyName("bodyStyle")]
        public string? BodyStyle { get; set; }

        [JsonPropertyName("transmission")]
        public string? Transmission { get; set; }

        [JsonPropertyName("stockTypeId")]
        public int? StockTypeId { get; set; }

        [JsonPropertyName("vin")]
        public string? Vin { get; set; }

        [JsonPropertyName("vehicleRegistrationNumber")]
        public string? VehicleRegistrationNumber { get; set; }

        [JsonPropertyName("leadVehicleTypeId")]
        public int? LeadVehicleTypeId { get; set; }

        [JsonPropertyName("stockNumber")]
        public string? StockNumber { get; set; }

        [JsonPropertyName("fuelTypeCode")]
        public string? FuelTypeCode { get; set; }

        [JsonPropertyName("odometer")]
        public OdometerRTC? Odometer { get; set; }

        [JsonPropertyName("offerPrice")]
        public object? OfferPrice { get; set; }

        [JsonPropertyName("sellingPrice")]
        public object? SellingPrice { get; set; }

        [JsonPropertyName("appraisalPrice")]
        public object? AppraisalPrice { get; set; }

        [JsonPropertyName("options")]
        public List<object>? Options { get; set; }

        [JsonPropertyName("comments")]
        public List<object>? Comments { get; set; }

        [JsonPropertyName("financing")]
        public object? Financing { get; set; }
    }

    // Model Class for (RTC Response)
    // RTCKeyloopLeadRes myDeserializedClass = JsonSerializer.Deserialize<RTCKeyloopLeadRes>(myJsonResponse);
    public class RTCKeyloopLeadRes
    {
        [JsonPropertyName("leadGuid")]
        public string? LeadGuid { get; set; }
    }

}
