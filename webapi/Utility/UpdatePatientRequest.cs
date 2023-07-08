using webapi.Models;

namespace webapi.Utility
{
    public class UpdatePatientRequest
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public List<int> DiagnoseIds { get; set; }
    }

}
