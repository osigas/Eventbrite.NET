using System;

namespace EventbriteNET.Http
{
    public class RestRequestException : Exception
    {
        public RestRequestException() : base() { }
        public RestRequestException(string message) : base(message) { }
        public RestRequestException(string message, Exception innerException) : base(message, innerException) { }

        public RestRequestException(ErrorField error) : base(error.ErrorDescription) { }
    }
}
