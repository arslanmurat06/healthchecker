

namespace HealthChecker.Contracts.DTOs
{
    public class ErrorDTO
    {
        public string Message { get; set; }

        public ErrorDTO( string message)
        {
            Message = message;
        }
    }
}
