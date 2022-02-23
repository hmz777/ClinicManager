using ClinicProject.Server.Data.DBModels.ModelBaseTypes;

namespace ClinicProject.Server.Data.DBModels.PaymentTypes
{
    public class Payment : DBEntityBase
    {
        public decimal Value { get; set; }
    }
}
