using ClinicProject.Server.Data.DBModels.ModelBaseTypes;

namespace ClinicProject.Server.Data.DBModels.PatientTypes
{
    public class ExtraData : DBEntityBase
    {
        public string? Data { get; set; }
    }
}
