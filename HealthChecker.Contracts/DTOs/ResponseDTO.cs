using System.Collections.Generic;

namespace HealthChecker.Contracts.DTOs
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<ErrorDTO> Errors { get; set; }

        public ResponseDTO()
        {

        }
        public ResponseDTO(bool success)
        {
            Success = success;
            Errors = new List<ErrorDTO>();
        }

        public ResponseDTO(T data)
        {
            Data = data;
            Errors = new List<ErrorDTO>();
            Success = true;
        }

        public ResponseDTO(List<ErrorDTO> errors)
        {
            Errors = errors;
        }

        public ResponseDTO(ErrorDTO error)
        {
            Errors = new List<ErrorDTO> { error };
        }
    }

    public class ResponseDTO
    {
        public bool Success { get; set; }
        public List<ErrorDTO> Errors { get; set; }

        public ResponseDTO()
        {

        }

        public ResponseDTO(bool success)
        {
            Success = success;
            Errors = new List<ErrorDTO>();
        }

        public ResponseDTO(List<ErrorDTO> errors)
        {
            Errors = errors;
            Success = false;
        }

        public ResponseDTO(ErrorDTO error)
        {
            Success = false;
            Errors = new List<ErrorDTO> { error };
        }
    }
}
