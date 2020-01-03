using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApplication.Models
{
    public class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
        }
        public ApiError(ModelStateDictionary modelState = null)
        {
            if (modelState != null)
            {
                Message = "Invalid Parameter.";
                Detail = modelState
                    .FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors
                    .FirstOrDefault().ErrorMessage;
            }
        }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}