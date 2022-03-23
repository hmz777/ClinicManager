using ClinicProject.Server.Data.DBModels.ModelBaseTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Shared.Models.Payment;

namespace ClinicProject.Server.Data.DBModels.TreatmentTypes
{
    public class Treatment : DBEntityBase
    {
        public Treatment()
        {
            Payments = new HashSet<Payment>();
        }

        public string? TreatmentType { get; set; }
        public decimal TotalCost { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}
