using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicProject.Server.Data.DBModels.ModelBaseTypes
{
    public abstract class DBEntityBase
    {
        public DBEntityBase()
        {
            CreationDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime UpdateDate { get; set; }
    }
}
