using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public List<Diagnose> Diagnoses { get; set; }
    }
}
