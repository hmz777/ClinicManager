namespace ClinicProject.Shared.Models.Error
{
    public class ErrorModel
    {
        public List<string> Messages { get; set; } = new List<string>();

        public override string ToString()
        {
            return string.Join("\\n", Messages);
        }
    }
}