namespace webapi.Utility
{
    public class PostPatientRequest
    {
        public int PersonId { get; set; }
        public List<int> DiagnoseIds { get; set; }
    }
}
