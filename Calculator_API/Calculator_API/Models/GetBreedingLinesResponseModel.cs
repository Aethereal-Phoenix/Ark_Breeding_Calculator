using Calculator_API.Data;

namespace Calculator_API.Models
{
    public class GetBreedingLinesResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<BreedingLineData> BreedingLinesList { get; set; } = new List<BreedingLineData>();
    }
}
