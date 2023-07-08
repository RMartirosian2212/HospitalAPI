using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Diagnose
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
