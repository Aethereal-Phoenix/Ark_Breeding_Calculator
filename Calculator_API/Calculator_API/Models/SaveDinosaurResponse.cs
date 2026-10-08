namespace Calculator_API.Models
{
    public class SaveDinosaurResponse
    {
        public int StatusCode { get; set; }
        public bool Status {  get; set; }
        public string Message { get; set; }

        // Return to DTO not EF entity
        public DinosaurData? Data { get; set; }
        public Error? Error { get; set; }
        public int UserId { get; set; }
    }

    public class Error
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
