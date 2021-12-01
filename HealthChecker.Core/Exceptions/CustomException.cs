namespace HealthCheckerWeb.Core.Exceptions
{
    public class CustomException: Exception
    {
        private readonly string _error;
        public CustomException(string error):base(error)
        {
            _error = error;
        }
    }
}
