using Newtonsoft.Json;
using Refit;
using System.Net;

namespace Domain.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public CustomException(HttpStatusCode status, string msg) : base(msg)
        {
            StatusCode = (int)status;
            Message = msg;
        }

        public CustomException(ApiException exception)
        {
            try
            {
                ProblemDetails problem = JsonConvert.DeserializeObject<ProblemDetails>(exception.Content);

                StatusCode = problem.Status;
                Message = problem.Detail;
            }
            catch (Exception)
            {
                StatusCode = (int)exception.StatusCode;
                Message = exception.Content;
            }
        }
    }
}
