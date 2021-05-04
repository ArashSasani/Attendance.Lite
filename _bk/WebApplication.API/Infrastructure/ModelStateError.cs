using System;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.API.Infrastructure
{
    public class ModelStateError
    {
        private IExceptionLogger _logger;
        public ModelStateError(IExceptionLogger logger)
        {
            _logger = logger;
        }

        public string OutputMessage(ModelStateDictionary modelState)
        {
            var errorMessage = "";
            foreach (var errorKey in modelState.Keys)
            {
                modelState.TryGetValue(errorKey, out ModelState errorValue);

                foreach (var item in errorValue.Errors)
                {
                    var error = new ValidationError
                    {
                        Value = errorValue.Value,
                        Exception = item.Exception,
                        Message = item.ErrorMessage
                    };
                    if (error.Exception != null)
                    {
                        _logger.LogRunTimeError(error.Exception, error.Exception.Message);
                    }
                    errorMessage += error.GenerateMessage();
                }
            }
            return errorMessage;
        }
    }

    public class ValidationError
    {
        public ValueProviderResult Value { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }

        public string GenerateMessage()
        {
            string message = "";
            if (Value != null)
            {
                if (!string.IsNullOrEmpty(Value.AttemptedValue))
                    message += $"value: {Value.AttemptedValue}.<br/>";
            }
            if (!string.IsNullOrEmpty(Message))
            {
                message += $"{Message}.<br/>";
            }
            if (Exception != null)
            {
                message += AppSettings.MODELSTATE_ERROR_MESSAGE;
            }
            return message;
        }
    }
}