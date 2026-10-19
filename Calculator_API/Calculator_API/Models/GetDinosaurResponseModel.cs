namespace Calculator_API.Models
{
    public class GetDinosaursResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<DinosaurData> DinosaurList { get; set; } = new List<DinosaurData>();
    }
}
