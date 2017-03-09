using Nancy;
using Nancy.Configuration;
using Nancy.Json;
using Nancy.ModelBinding;
using Nancy.Responses;
using Newtonsoft.Json;
using OnDemandTools.Common.Exceptions;
using System;


namespace OnDemandTools.API.v1.Models
{
    public class ErrorResponse : JsonResponse
    {
        readonly Error _error;

        public string ErrorMessage { get { return _error.Message; } }
                

        private ErrorResponse(Error error): base(error, new DefaultJsonSerializer(GetEnvironment()), GetEnvironment())
        {
            _error = error;
        }

        public static ErrorResponse FromMessage(string message)
        {
            return new ErrorResponse(new Error { Message = message });
        }

        public static ErrorResponse FromException(Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            var error = new Error { Message = ex.Message };

            if (ex is AiringNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }

            if (ex is SecurityAccessDeniedException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }
            
            if (ex is ModelBindingException || ex is JsonException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            var response = new ErrorResponse(error)
            {
                StatusCode = statusCode
            };

            return response;
        }

        private static INancyEnvironment GetEnvironment()
        {
            var environment =
                new DefaultNancyEnvironment();         

            environment.Tracing(
                enabled: true,
                displayErrorTraces: true);

            environment.Json();
            environment.Globalization(new[] { "en-US" });

            return environment;
        }
    }

    class Error
    {
        public string Message { get; set; }
    }


}
