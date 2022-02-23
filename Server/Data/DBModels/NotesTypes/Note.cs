using ClinicProject.Server.Data.DBModels.ModelBaseTypes;

namespace ClinicProject.Server.Data.DBModels.NotesTypes
{
    public class Note : DBEntityBase
    {
        public string? Value { get; set; }
    }
}
