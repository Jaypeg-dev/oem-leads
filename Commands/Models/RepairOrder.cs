using System.Text.Json.Serialization;

namespace oemLeads.Commands.Models
{
    // RepairOrderResponse myDeserializedClass = JsonSerializer.Deserialize<RepairOrderResponse>(myJsonResponse);
    public class RORAppointment
    {
        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("bookingOptions")]
        public RORBookingOptions BookingOptions { get; set; }

        [JsonPropertyName("dueInDateTime")]
        public string DueInDateTime { get; set; }

        [JsonPropertyName("dueOutDateTime")]
        public string DueOutDateTime { get; set; }

        [JsonPropertyName("customerWaiting")]
        public bool? CustomerWaiting { get; set; }
    }

    public class RORAssignedAdvisor
    {
        [JsonPropertyName("serviceAdvisorId")]
        public string ServiceAdvisorId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }

    public class RORBookingOptions
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class RORCheckInMileage
    {
        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    public class RORCheckOutMileage
    {
        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    public class RORCompany
    {
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class RORContact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
    }

    public class RORContactAdvisor
    {
        [JsonPropertyName("serviceAdvisorId")]
        public string ServiceAdvisorId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }

    public class RORCustomer
    {
        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }

        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("titleCommon")]
        public string TitleCommon { get; set; }
    }

    public class RORDetails
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("customerPurchaseOrderNumber")]
        public string CustomerPurchaseOrderNumber { get; set; }

        [JsonPropertyName("payerAccount")]
        public string PayerAccount { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("checkInDateTime")]
        public string CheckInDateTime { get; set; }

        [JsonPropertyName("checkOutDateTime")]
        public string CheckOutDateTime { get; set; }

        [JsonPropertyName("completedDateTime")]
        public string CompletedDateTime { get; set; }

        [JsonPropertyName("closedDateTime")]
        public string ClosedDateTime { get; set; }

        [JsonPropertyName("checkInMileage")]
        public RORCheckInMileage CheckInMileage { get; set; }

        [JsonPropertyName("checkOutMileage")]
        public RORCheckOutMileage CheckOutMileage { get; set; }

        [JsonPropertyName("onHoldReason")]
        public string OnHoldReason { get; set; }

        [JsonPropertyName("serviceTypes")]
        public List<string> ServiceTypes { get; set; }

        [JsonPropertyName("customerWaiting")]
        public bool? CustomerWaiting { get; set; }
    }

    public class RORDiscount
    {
        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class RORFee
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int? SequenceNumber { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("feeCode")]
        public string FeeCode { get; set; }

        [JsonPropertyName("feeType")]
        public string FeeType { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("discount")]
        public RORDiscount Discount { get; set; }

        [JsonPropertyName("linePayer")]
        public string LinePayer { get; set; }

        [JsonPropertyName("orderPrice")]
        public ROROrderPrice OrderPrice { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistory UpdateHistory { get; set; }

        [JsonPropertyName("linkReference")]
        public string LinkReference { get; set; }
    }

    public class RORJob
    {
        [JsonPropertyName("jobId")]
        public string JobId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("serviceTypes")]
        public List<string> ServiceTypes { get; set; }

        [JsonPropertyName("workStartedDateTime")]
        public string WorkStartedDateTime { get; set; }

        [JsonPropertyName("workCompletedDateTime")]
        public string WorkCompletedDateTime { get; set; }

        [JsonPropertyName("linkReference")]
        public string LinkReference { get; set; }

        [JsonPropertyName("isVhc")]
        public bool? IsVhc { get; set; }

        [JsonPropertyName("vhcDetails")]
        public RORVhcDetails VhcDetails { get; set; }

        [JsonPropertyName("menus")]
        public List<RORMenu> Menus { get; set; }

        [JsonPropertyName("labor")]
        public List<RORLabor> Labor { get; set; }

        [JsonPropertyName("parts")]
        public List<RORPart> Parts { get; set; }

        [JsonPropertyName("notes")]
        public List<RORNote> Notes { get; set; }

        [JsonPropertyName("fees")]
        public List<RORFee> Fees { get; set; }

        [JsonPropertyName("links")]
        public List<RORLink> Links { get; set; }
    }

    public class RORLabor
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int? SequenceNumber { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("laborCode")]
        public string LaborCode { get; set; }

        [JsonPropertyName("brandCode")]
        public string BrandCode { get; set; }

        [JsonPropertyName("allowedTimeInMinutes")]
        public int? AllowedTimeInMinutes { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("laborDepartmentType")]
        public string LaborDepartmentType { get; set; }

        [JsonPropertyName("skill")]
        public RORSkill Skill { get; set; }

        [JsonPropertyName("discount")]
        public RORDiscount Discount { get; set; }

        [JsonPropertyName("linePayer")]
        public string LinePayer { get; set; }

        [JsonPropertyName("listPrice")]
        public RORListPrice ListPrice { get; set; }

        [JsonPropertyName("orderPrice")]
        public ROROrderPrice OrderPrice { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistory UpdateHistory { get; set; }

        [JsonPropertyName("technician")]
        public RORTechnician Technician { get; set; }

        [JsonPropertyName("linkReference")]
        public string LinkReference { get; set; }
    }

    public class RORLink
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("rel")]
        public string Rel { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class RORListPrice
    {
        [JsonPropertyName("netValue")]
        public double? NetValue { get; set; }

        [JsonPropertyName("grossValue")]
        public double? GrossValue { get; set; }

        [JsonPropertyName("taxValue")]
        public double? TaxValue { get; set; }

        [JsonPropertyName("taxRate")]
        public double? TaxRate { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public class RORMenu
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int? SequenceNumber { get; set; }

        [JsonPropertyName("brandCode")]
        public string BrandCode { get; set; }

        [JsonPropertyName("menuCode")]
        public string MenuCode { get; set; }

        [JsonPropertyName("menuType")]
        public string MenuType { get; set; }

        [JsonPropertyName("parts")]
        public List<RORPart> Parts { get; set; }

        [JsonPropertyName("labor")]
        public List<RORLabor> Labor { get; set; }

        [JsonPropertyName("notes")]
        public List<RORNote> Notes { get; set; }

        [JsonPropertyName("fees")]
        public List<RORFee> Fees { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("discount")]
        public RORDiscount Discount { get; set; }

        [JsonPropertyName("linePayer")]
        public string LinePayer { get; set; }

        [JsonPropertyName("listPrice")]
        public RORListPrice ListPrice { get; set; }

        [JsonPropertyName("orderPrice")]
        public ROROrderPrice OrderPrice { get; set; }

        [JsonPropertyName("linkReference")]
        public string LinkReference { get; set; }
    }

    public class RORNote
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int? SequenceNumber { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistory UpdateHistory { get; set; }
    }

    public class ROROrderPrice
    {
        [JsonPropertyName("netValue")]
        public double? NetValue { get; set; }

        [JsonPropertyName("grossValue")]
        public double? GrossValue { get; set; }

        [JsonPropertyName("taxValue")]
        public double? TaxValue { get; set; }

        [JsonPropertyName("taxRate")]
        public double? TaxRate { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public class RORPart
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int? SequenceNumber { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("partId")]
        public string PartId { get; set; }

        [JsonPropertyName("brandCode")]
        public string BrandCode { get; set; }

        [JsonPropertyName("partCode")]
        public string PartCode { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public RORUnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("discount")]
        public RORDiscount Discount { get; set; }

        [JsonPropertyName("linePayer")]
        public string LinePayer { get; set; }

        [JsonPropertyName("listPrice")]
        public RORListPrice ListPrice { get; set; }

        [JsonPropertyName("orderPrice")]
        public ROROrderPrice OrderPrice { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistory UpdateHistory { get; set; }

        [JsonPropertyName("linkReference")]
        public string LinkReference { get; set; }
    }

    public class RORPlanning
    {
        [JsonPropertyName("plannedIn")]
        public string PlannedIn { get; set; }

        [JsonPropertyName("plannedOut")]
        public string PlannedOut { get; set; }
    }

    public class RORReferenceData
    {
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }

        [JsonPropertyName("makeCode")]
        public string MakeCode { get; set; }

        [JsonPropertyName("modelCode")]
        public string ModelCode { get; set; }

        [JsonPropertyName("variantCode")]
        public string VariantCode { get; set; }

        [JsonPropertyName("vehicleCode")]
        public string VehicleCode { get; set; }
    }

    public class RORResources
    {
        [JsonPropertyName("assignedAdvisor")]
        public RORAssignedAdvisor AssignedAdvisor { get; set; }

        [JsonPropertyName("contactAdvisor")]
        public RORContactAdvisor ContactAdvisor { get; set; }
    }

    public class RepairOrderResponse
    {
        [JsonPropertyName("repairOrderId")]
        public string RepairOrderId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("details")]
        public RORDetails Details { get; set; }

        [JsonPropertyName("vehicle")]
        public RORVehicle Vehicle { get; set; }

        [JsonPropertyName("customer")]
        public RORCustomer Customer { get; set; }

        [JsonPropertyName("company")]
        public RORCompany Company { get; set; }

        [JsonPropertyName("contact")]
        public RORContact Contact { get; set; }

        [JsonPropertyName("resources")]
        public RORResources Resources { get; set; }

        [JsonPropertyName("appointment")]
        public RORAppointment Appointment { get; set; }

        [JsonPropertyName("planning")]
        public RORPlanning Planning { get; set; }

        [JsonPropertyName("jobs")]
        public List<RORJob> Jobs { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistoryEnd UpdateHistory { get; set; }

        [JsonPropertyName("links")]
        public List<RORLink> Links { get; set; }
    }

    public class RORSkill
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class RORSubOrder
    {
        [JsonPropertyName("repairOrderId")]
        public string RepairOrderId { get; set; }
    }

    public class RORTechnician
    {
        [JsonPropertyName("technicianId")]
        public string TechnicianId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class RORUnitOfMeasure
    {
        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }
    }

    public class RORUpdateHistory
    {
        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("lastModified")]
        public string LastModified { get; set; }
    }

    public class RORUpdateHistoryEnd
    {
        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("lastModified")]
        public string LastModified { get; set; }

        [JsonPropertyName("updateHistory")]
        public RORUpdateHistory UpdateHistory { get; set; }

        [JsonPropertyName("previousRepairOrderId")]
        public string PreviousRepairOrderId { get; set; }

        [JsonPropertyName("mainRepairOrderId")]
        public string MainRepairOrderId { get; set; }

        [JsonPropertyName("subOrders")]
        public List<RORSubOrder> SubOrders { get; set; }
    }

    public class RORVehicle
    {
        [JsonPropertyName("vehicleId")]
        public string VehicleId { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("makeId")]
        public string MakeId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vin")]
        public string Vin { get; set; }

        [JsonPropertyName("licensePlate")]
        public string LicensePlate { get; set; }

        [JsonPropertyName("referenceData")]
        public RORReferenceData ReferenceData { get; set; }
    }

    public class RORVhcDetails
    {
        [JsonPropertyName("jobConditionCode")]
        public string JobConditionCode { get; set; }

        [JsonPropertyName("vhcOperator")]
        public string VhcOperator { get; set; }
    }

    // RepairOrderDetailsReq myDeserializedClass = JsonSerializer.Deserialize<RepairOrderDetailsReq>(myJsonResponse);
    // At present only notes have been applied to details
    public class RepairOrderDetailsReq
    {
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

}