using ClinicProject.Shared.Models.Payment;

namespace ClinicProject.Shared.DTOs
{
    public class TreatmentDTO : DTOBase
    {
        public string? TreatmentType { get; set; }
        public decimal TotalCost { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual ICollection<PaymentDTO> Payments { get; set; }
    }
}
