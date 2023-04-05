using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace oemLeads.Commands.Models
{
    // Models Class for (single Rwil Lead)
    public class AdditionalServiceLeadInformation
    {
        [JsonPropertyName("customerAddress")]
        public CustomerAddress? CustomerAddress { get; set; }

        [JsonPropertyName("customerDesiredDate")]
        public DateTime? CustomerDesiredDate { get; set; }

        [JsonPropertyName("dataPrivacyApproval")]
        public string? DataPrivacyApproval { get; set; }

        [JsonPropertyName("fleetVehicle")]
        public string? FleetVehicle { get; set; }

        [JsonPropertyName("licensePlate")]
        public string? LicensePlate { get; set; }

        [JsonPropertyName("mot")]
        public string? Mot { get; set; }

        [JsonPropertyName("replacementMobility")]
        public List<ReplacementMobility>? ReplacementMobility { get; set; }

        [JsonPropertyName("serviceScope")]
        public List<ServiceScope>? ServiceScope { get; set; }

        [JsonPropertyName("shoppingCardID")]
        public string? ShoppingCardID { get; set; }

        [JsonPropertyName("specifications")]
        public List<SpecificationC>? Specifications { get; set; }
    }

    public class ApplicationIdentifier
    {
        [JsonPropertyName("ID")]
        public string? ID { get; set; }

        [JsonPropertyName("schemeID")]
        public string? SchemeID { get; set; }
    }

    public class BusinessPartner
    {
        [JsonPropertyName("academicTitle")]
        public string? AcademicTitle { get; set; }

        [JsonPropertyName("applicationIdentifiers")]
        public List<ApplicationIdentifier>? ApplicationIdentifiers { get; set; }

        [JsonPropertyName("communicationChannels")]
        public List<CommunicationChannel>? CommunicationChannels { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("partnerKey")]
        public PartnerKey? PartnerKey { get; set; }

        [JsonPropertyName("partnerKind")]
        public string? PartnerKind { get; set; }

        [JsonPropertyName("salutation")]
        public string? Salutation { get; set; }

        [JsonPropertyName("wholesalerKey")]
        public WholesalerKey? WholesalerKey { get; set; }
    }

    public class CommunicationChannel
    {
        [JsonPropertyName("channelType")]
        public string? ChannelType { get; set; }

        [JsonPropertyName("channelValue")]
        public string? ChannelValue { get; set; }
    }

    public class CustomerAddress
    {
        [JsonPropertyName("additional")]
        public string? Additional { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("houseNumber")]
        public string? HouseNumber { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }
    }

    public class CustomerServiceRequest
    {
        [JsonPropertyName("applicationIdentifiers")]
        public List<ApplicationIdentifier>? ApplicationIdentifiers { get; set; }

        [JsonPropertyName("channelType")]
        public string? ChannelType { get; set; }

        [JsonPropertyName("customerServiceRequestUID")]
        public string? CustomerServiceRequestUID { get; set; }

        [JsonPropertyName("maturity")]
        public string? Maturity { get; set; }

        [JsonPropertyName("partnerKey")]
        public PartnerKey? PartnerKey { get; set; }

        [JsonPropertyName("requestCreatingSystem")]
        public string? RequestCreatingSystem { get; set; }

        [JsonPropertyName("requestType")]
        public string? RequestType { get; set; }

        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("spreadAppointmentIND")]
        public string? SpreadAppointmentIND { get; set; }

        [JsonPropertyName("vehicleStatusRef")]
        public List<VehicleStatusRef>? VehicleStatusRef { get; set; }
    }

    public class EngineCodeC
    {
        [JsonPropertyName("engineCode")]
        public string? EngineCode { get; set; }
    }

    public class Escalation
    {
        [JsonPropertyName("escalationV1")]
        public DateTime? EscalationV1 { get; set; }

        [JsonPropertyName("escalationV2")]
        public DateTime? EscalationV2 { get; set; }

        [JsonPropertyName("escalationV3")]
        public DateTime? EscalationV3 { get; set; }
    }

    public class Header
    {
        [JsonPropertyName("wholesalerKey")]
        public string? WholesalerKey { get; set; }

        [JsonPropertyName("partnerKey")]
        public string? PartnerKey { get; set; }

        [JsonPropertyName("businessObjectType")]
        public string? BusinessObjectType { get; set; }

        [JsonPropertyName("eventType")]
        public string? EventType { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }

        [JsonPropertyName("messageId")]
        public string? MessageId { get; set; }

        [JsonPropertyName("relatesTo")]
        public string? RelatesTo { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }

    public class ListOfVehicleStatus
    {
        [JsonPropertyName("actuation")]
        public string? Actuation { get; set; }

        [JsonPropertyName("creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }

        [JsonPropertyName("odometerValue")]
        public long? OdometerValue { get; set; }

        [JsonPropertyName("odometerValueUnit")]
        public string? OdometerValueUnit { get; set; }

        [JsonPropertyName("vehicleStatusUID")]
        public string? VehicleStatusUID { get; set; }
    }

    public class MaintenanceEvent
    {
        [JsonPropertyName("distance")]
        public int? Distance { get; set; }

        [JsonPropertyName("distanceUnit")]
        public string? DistanceUnit { get; set; }

        [JsonPropertyName("maintenanceEventType")]
        public string? MaintenanceEventType { get; set; }

        [JsonPropertyName("maintenanceStatus")]
        public string? MaintenanceStatus { get; set; }

        [JsonPropertyName("days")]
        public int? Days { get; set; }
    }

    public class ModelCodeC
    {
        [JsonPropertyName("modelCode")]
        public string? ModelCode { get; set; }
    }

    public class PartnerKey
    {
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("countryImporter")]
        public string? CountryImporter { get; set; }

        [JsonPropertyName("partnerNumber")]
        public string? PartnerNumber { get; set; }
    }

    public class Payload
    {
        [JsonPropertyName("additionalServiceLeadInformation")]
        public AdditionalServiceLeadInformation? AdditionalServiceLeadInformation { get; set; }

        [JsonPropertyName("businessPartners")]
        public List<BusinessPartner>? BusinessPartners { get; set; }

        [JsonPropertyName("customerServiceRequest")]
        public CustomerServiceRequest? CustomerServiceRequest { get; set; }

        [JsonPropertyName("listOfVehicleStatus")]
        public List<ListOfVehicleStatus>? ListOfVehicleStatus { get; set; }

        [JsonPropertyName("processValues")]
        public ProcessValues? ProcessValues { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("serviceLeadRecordID")]
        public string? ServiceLeadRecordID { get; set; }

        [JsonPropertyName("vehicle")]
        public Vehicle? Vehicle { get; set; }

        [JsonPropertyName("vehicleMaintenanceInformation")]
        public VehicleMaintenanceInformation? VehicleMaintenanceInformation { get; set; }

        [JsonPropertyName("wholesalerNumber")]
        public string? WholesalerNumber { get; set; }
    }

    public class Priority
    {
        [JsonPropertyName("initStage")]
        public string? InitStage { get; set; }

        [JsonPropertyName("secondStage")]
        public string? SecondStage { get; set; }

        [JsonPropertyName("secondStageTime")]
        public DateTime? SecondStageTime { get; set; }

        [JsonPropertyName("thirdStage")]
        public string? ThirdStage { get; set; }

        [JsonPropertyName("thirdStageTime")]
        public DateTime? ThirdStageTime { get; set; }
    }

    public class ProcessValues
    {
        [JsonPropertyName("escalation")]
        public Escalation? Escalation { get; set; }

        [JsonPropertyName("priority")]
        public Priority? Priority { get; set; }

        [JsonPropertyName("reminder")]
        public Reminder? Reminder { get; set; }

        [JsonPropertyName("timeAllowance")]
        public TimeAllowance? TimeAllowance { get; set; }
    }

    public class Reminder
    {
    }

    public class ReplacementMobility
    {
        [JsonPropertyName("languageKey")]
        public string? LanguageKey { get; set; }

        [JsonPropertyName("replacementReferenceCode")]
        public string? ReplacementReferenceCode { get; set; }

        [JsonPropertyName("replacementText")]
        public string? ReplacementText { get; set; }
    }

    public class RwilSingleLead
    {
        [JsonPropertyName("header")]
        public Header? Header { get; set; }

        [JsonPropertyName("payload")]
        public Payload? Payload { get; set; }
    }

    public class ServiceScope
    {
        [JsonPropertyName("languageKey")]
        public string? LanguageKey { get; set; }

        [JsonPropertyName("serviceReferenceCode")]
        public string? ServiceReferenceCode { get; set; }

        [JsonPropertyName("serviceText")]
        public string? ServiceText { get; set; }
    }

    public class SpecificationC
    {
        [JsonPropertyName("specification")]
        public string? Specification { get; set; }
    }

    public class TimeAllowance
    {
        [JsonPropertyName("T0")]
        public DateTime? T0 { get; set; }

        [JsonPropertyName("targetT3")]
        public DateTime? TargetT3 { get; set; }

        [JsonPropertyName("targetT4")]
        public DateTime? TargetT4 { get; set; }

        [JsonPropertyName("targetT5")]
        public DateTime? TargetT5 { get; set; }
    }

    public class TransmissionCodeC
    {
        [JsonPropertyName("transmissionCode")]
        public string? TransmissionCode { get; set; }
    }

    public class Vehicle
    {
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("engineCodes")]
        public List<EngineCodeC>? EngineCodes { get; set; }

        [JsonPropertyName("modelCodes")]
        public List<ModelCodeC>? ModelCodes { get; set; }

        [JsonPropertyName("modelName")]
        public string? ModelName { get; set; }

        [JsonPropertyName("modelYear")]
        public int? ModelYear { get; set; }

        [JsonPropertyName("transmissionCodes")]
        public List<TransmissionCodeC>? TransmissionCodes { get; set; }

        [JsonPropertyName("vin")]
        public string? Vin { get; set; }
    }

    public class VehicleMaintenanceInformation
    {
        [JsonPropertyName("maintenanceEvents")]
        public List<MaintenanceEvent>? MaintenanceEvents { get; set; }

        [JsonPropertyName("warningLightPriority")]
        public string? WarningLightPriority { get; set; }
    }

    public class VehicleStatusRef
    {
        [JsonPropertyName("vehicleStatusUID")]
        public string? VehicleStatusUID { get; set; }
    }

    public class WholesalerKey
    {
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("wholesalerNumber")]
        public string? WholesalerNumber { get; set; }
    }

    // Model Class for (Rwil T0 Update)
    // RwilT0 myDeserializedClass = JsonSerializer.Deserialize<RwilT0>(myJsonResponse);
    public class RwilT0
    {
        [JsonPropertyName("event")]
        public Event? Event { get; set; }
    }

    public class Event
    {
        [JsonPropertyName("eventType")]
        public string? EventType { get; set; }

        [JsonPropertyName("serviceLeadRecordID")]
        public string? ServiceLeadRecordID { get; set; }

        [JsonPropertyName("receivingStatus")]
        public string? ReceivingStatus { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
    // Model Class for (Rwil Offset Commit)
    // RwillOffsetCommit myDeserializedClass = JsonSerializer.Deserialize<RwillOffsetCommit>(myJsonResponse);
    public class RwillOffsetCommit
    {
        [JsonPropertyName("offsets")]
        public List<OffsetC>? Offsets { get; set; }
    }

    public class OffsetC
    {
        [JsonPropertyName("bo_type")]
        public string? BoType { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }

    // Model Class for (Rwil T3 Update)
    // RwilT3 myDeserializedClass = JsonSerializer.Deserialize<RwilT3>(myJsonResponse);
    public class RwilT3
    {
        [JsonPropertyName("event")]
        public EventT3? Event { get; set; }
    }

    public class EventT3
    {
        [JsonPropertyName("eventType")]
        public string? EventType { get; set; }

        [JsonPropertyName("serviceLeadRecordID")]
        public string? ServiceLeadRecordID { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
    }

}
