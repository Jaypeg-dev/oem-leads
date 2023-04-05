using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace oemLeads.Commands.Models
{
    // Model Class for (Service Appointment Booking)
    // SABRequest myDeserializedClass = JsonSerializer.Deserialize<SABRequest>(myJsonResponse);
    public class AdditionalServiceProduct
    {
        [JsonPropertyName("serviceProductId")]
        public string? ServiceProductId { get; set; }

        [JsonPropertyName("customerNotes")]
        public string? CustomerNotes { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("streetName")]
        public string? StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public int? HouseNumber { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }
    }

    public class AuditData
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
    }

    public class BookingOption
    {
    }

    public class ContactInformation
    {
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("customerId")]
        public string? CustomerId { get; set; }

        [JsonPropertyName("names")]
        public Names? Names { get; set; }

        [JsonPropertyName("class")]
        public string? Class { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        [JsonPropertyName("contactInformation")]
        public ContactInformation? ContactInformation { get; set; }

        [JsonPropertyName("externalReference")]
        public ExternalReference? ExternalReference { get; set; }
    }

    public class Details
    {
        [JsonPropertyName("externalReference")]
        public string? ExternalReference { get; set; }

        [JsonPropertyName("dueInDateTime")]
        public DateTime? DueInDateTime { get; set; }

        [JsonPropertyName("dueOutDateTime")]
        public DateTime? DueOutDateTime { get; set; }

        [JsonPropertyName("advisorRequired")]
        public bool? AdvisorRequired { get; set; }

        [JsonPropertyName("contactAdvisorId")]
        public string? ContactAdvisorId { get; set; }
    }

    public class ExternalReference
    {
        [JsonPropertyName("providerCode")]
        public string? ProviderCode { get; set; }

        [JsonPropertyName("applicationCode")]
        public string? ApplicationCode { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class Identification
    {
        [JsonPropertyName("vin")]
        public string? Vin { get; set; }

        [JsonPropertyName("licensePlate")]
        public string? LicensePlate { get; set; }

        [JsonPropertyName("engineNumber")]
        public string? EngineNumber { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("auditData")]
        public AuditData? AuditData { get; set; }
    }

    public class Mileage
    {
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    public class Names
    {
        [JsonPropertyName("familyName")]
        public string? FamilyName { get; set; }

        [JsonPropertyName("givenName")]
        public string? GivenName { get; set; }
    }

    public class SABRequest
    {
        [JsonPropertyName("details")]
        public Details? Details { get; set; }

        [JsonPropertyName("customer")]
        public Customer? Customer { get; set; }

        [JsonPropertyName("vehicle")]
        public VehicleSAB? Vehicle { get; set; }

        [JsonPropertyName("bookingOption")]
        public BookingOption? BookingOption { get; set; }

        [JsonPropertyName("serviceProducts")]
        public List<object>? ServiceProducts { get; set; }

        [JsonPropertyName("additionalServiceProducts")]
        public List<AdditionalServiceProduct>? AdditionalServiceProducts { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }

    public class VehicleSAB
    {
        [JsonPropertyName("vehicleId")]
        public string? VehicleId { get; set; }

        [JsonPropertyName("class")]
        public string? Class { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("identification")]
        public Identification? Identification { get; set; }

        [JsonPropertyName("mileage")]
        public Mileage? Mileage { get; set; }
    }

    // Model Class for (Service Appointment Booking Response)
    // SABResponse myDeserializedClass = JsonSerializer.Deserialize<SABResponse>(myJsonResponse);
    public class SABResponse
    {
        [JsonPropertyName("appointmentId")]
        public string? AppointmentId { get; set; }

        [JsonPropertyName("details")]
        public Details? Details { get; set; }

        [JsonPropertyName("customer")]
        public Customer? Customer { get; set; }

        [JsonPropertyName("vehicle")]
        public Vehicle? Vehicle { get; set; }

        [JsonPropertyName("bookingOption")]
        public BookingOption? BookingOption { get; set; }

        [JsonPropertyName("serviceProducts")]
        public List<ServiceProductSABRes>? ServiceProducts { get; set; }

        [JsonPropertyName("additionalServiceProducts")]
        public List<AdditionalServiceProduct>? AdditionalServiceProducts { get; set; }
    }

    public class AdditionalServiceProductSABRes
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("customerNotes")]
        public string? CustomerNotes { get; set; }

        [JsonPropertyName("price")]
        public PriceSABRes? Price { get; set; }

        [JsonPropertyName("menus")]
        public List<MenuSABRes>? Menus { get; set; }

        [JsonPropertyName("laborItems")]
        public List<LaborItemSABRes>? LaborItems { get; set; }

        [JsonPropertyName("partItems")]
        public List<PartItemSABRes>? PartItems { get; set; }

        [JsonPropertyName("textItems")]
        public List<TextItemSABRes>? TextItems { get; set; }
    }

    public class AddressSABRes
    {
        [JsonPropertyName("streetType")]
        public string? StreetType { get; set; }

        [JsonPropertyName("streetName")]
        public string? StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public int? HouseNumber { get; set; }

        [JsonPropertyName("buildingName")]
        public string? BuildingName { get; set; }

        [JsonPropertyName("floorNumber")]
        public string? FloorNumber { get; set; }

        [JsonPropertyName("doorNumber")]
        public string? DoorNumber { get; set; }

        [JsonPropertyName("blockName")]
        public string? BlockName { get; set; }

        [JsonPropertyName("estate")]
        public string? Estate { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("suburb")]
        public string? Suburb { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }
    }

    public class BookingOptionSABRes
    {
        [JsonPropertyName("appointmentOptionId")]
        public string? AppointmentOptionId { get; set; }

        [JsonPropertyName("customerNotes")]
        public string? CustomerNotes { get; set; }

        [JsonPropertyName("collectAddress")]
        public CollectAddressSABRes? CollectAddress { get; set; }

        [JsonPropertyName("deliveryAddress")]
        public DeliveryAddressSABRes? DeliveryAddress { get; set; }
    }

    public class CollectAddressSABRes
    {
        [JsonPropertyName("streetType")]
        public string? StreetType { get; set; }

        [JsonPropertyName("streetName")]
        public string? StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public int? HouseNumber { get; set; }

        [JsonPropertyName("buildingName")]
        public string? BuildingName { get; set; }

        [JsonPropertyName("floorNumber")]
        public string? FloorNumber { get; set; }

        [JsonPropertyName("doorNumber")]
        public string? DoorNumber { get; set; }

        [JsonPropertyName("blockName")]
        public string? BlockName { get; set; }

        [JsonPropertyName("estate")]
        public string? Estate { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("suburb")]
        public string? Suburb { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }
    }

    public class ContactAdvisorSABRes
    {
        [JsonPropertyName("advisorId")]
        public string? AdvisorId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class ContactInformationSABRes
    {
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("landline")]
        public string? Landline { get; set; }

        [JsonPropertyName("fax")]
        public string? Fax { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class CustomerSABRes
    {
        [JsonPropertyName("customerId")]
        public string? CustomerId { get; set; }

        [JsonPropertyName("names")]
        public Names? Names { get; set; }

        [JsonPropertyName("class")]
        public string? Class { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        [JsonPropertyName("contactInformation")]
        public ContactInformation? ContactInformation { get; set; }

        [JsonPropertyName("externalReference")]
        public ExternalReference? ExternalReference { get; set; }
    }

    public class DeliveryAddressSABRes
    {
        [JsonPropertyName("streetType")]
        public string? StreetType { get; set; }

        [JsonPropertyName("streetName")]
        public string? StreetName { get; set; }

        [JsonPropertyName("houseNumber")]
        public int? HouseNumber { get; set; }

        [JsonPropertyName("buildingName")]
        public string? BuildingName { get; set; }

        [JsonPropertyName("floorNumber")]
        public string? FloorNumber { get; set; }

        [JsonPropertyName("doorNumber")]
        public string? DoorNumber { get; set; }

        [JsonPropertyName("blockName")]
        public string? BlockName { get; set; }

        [JsonPropertyName("estate")]
        public string? Estate { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("suburb")]
        public string? Suburb { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }
    }

    public class DetailsSABRes
    {
        [JsonPropertyName("appointmentReference")]
        public string? AppointmentReference { get; set; }

        [JsonPropertyName("externalReference")]
        public string? ExternalReference { get; set; }

        [JsonPropertyName("customerNotes")]
        public string? CustomerNotes { get; set; }

        [JsonPropertyName("dueInDateTime")]
        public DateTime? DueInDateTime { get; set; }

        [JsonPropertyName("dueOutDateTime")]
        public DateTime? DueOutDateTime { get; set; }

        [JsonPropertyName("contactAdvisor")]
        public ContactAdvisorSABRes? ContactAdvisor { get; set; }
    }

    public class ExternalReferenceSABRes
    {
        [JsonPropertyName("providerCode")]
        public string? ProviderCode { get; set; }

        [JsonPropertyName("applicationCode")]
        public string? ApplicationCode { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class IdentificationSABRes
    {
        [JsonPropertyName("vin")]
        public string? Vin { get; set; }

        [JsonPropertyName("chassis")]
        public int? Chassis { get; set; }

        [JsonPropertyName("licensePlate")]
        public string? LicensePlate { get; set; }

        [JsonPropertyName("engineNumber")]
        public string? EngineNumber { get; set; }
    }

    public class LaborItemSABRes
    {
        [JsonPropertyName("sequenceNumber")]
        public string? SequenceNumber { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("laborCode")]
        public string? LaborCode { get; set; }

        [JsonPropertyName("brandCode")]
        public string? BrandCode { get; set; }

        [JsonPropertyName("allowedTimeInMinutes")]
        public int? AllowedTimeInMinutes { get; set; }
    }

    public class MenuSABRes
    {
        [JsonPropertyName("sequenceNumber")]
        public string? SequenceNumber { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("menuCode")]
        public string? MenuCode { get; set; }

        [JsonPropertyName("brandCode")]
        public string? BrandCode { get; set; }

        [JsonPropertyName("laborItems")]
        public List<LaborItemSABRes>? LaborItems { get; set; }

        [JsonPropertyName("partItems")]
        public List<PartItemSABRes>? PartItems { get; set; }

        [JsonPropertyName("textItems")]
        public List<TextItemSABRes>? TextItems { get; set; }
    }

    public class MileageSABRes
    {
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    public class NamesSABRes
    {
        [JsonPropertyName("familyName")]
        public string? FamilyName { get; set; }

        [JsonPropertyName("familyName2")]
        public string? FamilyName2 { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("givenName")]
        public string? GivenName { get; set; }

        [JsonPropertyName("preferredName")]
        public string? PreferredName { get; set; }

        [JsonPropertyName("initials")]
        public string? Initials { get; set; }

        [JsonPropertyName("salutation")]
        public string? Salutation { get; set; }

        [JsonPropertyName("titleCommon")]
        public string? TitleCommon { get; set; }
    }

    public class PartItemSABRes
    {
        [JsonPropertyName("sequenceNumber")]
        public string? SequenceNumber { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("partId")]
        public string? PartId { get; set; }

        [JsonPropertyName("partCode")]
        public string? PartCode { get; set; }

        [JsonPropertyName("brandCode")]
        public string? BrandCode { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public string? UnitOfMeasure { get; set; }

        [JsonPropertyName("quantity")]
        public string? Quantity { get; set; }
    }

    public class PriceSABRes
    {
        [JsonPropertyName("netValue")]
        public int? NetValue { get; set; }

        [JsonPropertyName("grossValue")]
        public int? GrossValue { get; set; }

        [JsonPropertyName("taxValue")]
        public int? TaxValue { get; set; }

        [JsonPropertyName("taxRate")]
        public double? TaxRate { get; set; }

        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }
    }

    public class ReferenceDataSABRes
    {
        [JsonPropertyName("providerId")]
        public string? ProviderId { get; set; }

        [JsonPropertyName("makeCode")]
        public string? MakeCode { get; set; }

        [JsonPropertyName("modelCode")]
        public string? ModelCode { get; set; }

        [JsonPropertyName("variantCode")]
        public string? VariantCode { get; set; }

        [JsonPropertyName("vehicleCode")]
        public string? VehicleCode { get; set; }

        [JsonPropertyName("serviceGroupCode")]
        public string? ServiceGroupCode { get; set; }

        [JsonPropertyName("exteriorColorCode")]
        public string? ExteriorColorCode { get; set; }

        [JsonPropertyName("interiorColorCode")]
        public string? InteriorColorCode { get; set; }
    }

    public class ServiceProductSABRes
    {
        [JsonPropertyName("serviceProductId")]
        public string? ServiceProductId { get; set; }

        [JsonPropertyName("customerNotes")]
        public string? CustomerNotes { get; set; }
    }

    public class TextItemSABRes
    {
        [JsonPropertyName("sequenceNumber")]
        public string? SequenceNumber { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }

    public class VehicleSABRes
    {
        [JsonPropertyName("vehicleId")]
        public string? VehicleId { get; set; }

        [JsonPropertyName("class")]
        public string? Class { get; set; }

        [JsonPropertyName("description")]
        public object? Description { get; set; }

        [JsonPropertyName("identification")]
        public Identification? Identification { get; set; }

        [JsonPropertyName("referenceData")]
        public ReferenceDataSABRes? ReferenceData { get; set; }

        [JsonPropertyName("mileage")]
        public Mileage? Mileage { get; set; }
    }

}
