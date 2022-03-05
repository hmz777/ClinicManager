namespace ClinicProject.Shared.Models.Error
{
    public class ModelValidationResult
    {
        public Dictionary<string, string> Results { get; set; }

        public bool HasResults { get { return Results != null && Results.Count > 0; } }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Results.Where(r => r.Value != null).Select(r => r.Value));
        }
    }
}