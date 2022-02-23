namespace ClinicProject.Shared.Models.Error
{
    public class ModelValidationResult
    {
        public Dictionary<string, string> Results { get; set; }

        public override string ToString()
        {
            return string.Join("\n", Results.Where(r => r.Value != null).Select(r => r.Value));
        }
    }
}