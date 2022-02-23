namespace ClinicProject.Client.Models
{
    public class PatchOp
    {
        public string op { get; set; }
        public string path { get; set; }
        public object value { get; set; }
    }
}
