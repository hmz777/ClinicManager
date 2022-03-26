using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.Models.Payment;
using ClinicProject.Shared.Payments;
using System.Text.Json.Serialization;

namespace ClinicProject.Shared.DTOs.Treatments
{
    public class TreatmentDTO : DTOBase
    {
        public TreatmentDTO()
        {
            PaymentStatus = PaymentStatus.Incomplete;
            PaymentType = PaymentType.Single;
        }

        [DataField(DisplayName = "Treatment Type", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public string? TreatmentType { get; set; }

        [DataField(DisplayName = "Total Cost (S.P)", DataField = DataField.Currency, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public decimal TotalCost { get; set; }

        [DataField(DisplayName = "Payment Type", DataField = DataField.Enum, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public virtual PaymentType PaymentType { get; set; }

        [DataField(DisplayName = "Payment Status", DataField = DataField.Enum, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public virtual PaymentStatus PaymentStatus { get; set; }

        [DataField(DisplayName = "Payments", DataField = DataField.NavigationView)]
        public virtual ICollection<PaymentDTO>? Payments { get; set; }

        [JsonIgnore]
        [DataField(DisplayName = "First Name", DataField = DataField.TextNavigationExpanded, ClientSearchable = true, ExpandedFrom = nameof(Patient))]
        public string? FirstName { get { return Patient?.FirstName; } set { if (Patient != null) { Patient.FirstName = value; } } }

        [JsonIgnore]
        [DataField(DisplayName = "Last Name", DataField = DataField.TextNavigationExpanded, ClientSearchable = true, ExpandedFrom = nameof(Patient))]
        public string? LastName { get { return Patient?.LastName; } set { if (Patient != null) { Patient.LastName = value; } } }

        [DataField(DisplayName = "Patient", DataField = DataField.Navigation, Expanded = true)]
        public PatientDTO? Patient { get; set; }

        [DataField(DisplayName = "Patient Id", DataField = DataField.None)]
        public int PatientId { get; set; }
    }
}