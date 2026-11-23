namespace Calculator_API.Models
{
    public class DinosaurResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DinosaurData? Data {  get; set; }
    }
}
