using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.ExceptionHandle
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
        public class NotFoundException : ApiException
        {
            public NotFoundException(string message)
                    : base(message, HttpStatusCode.NotFound)
            {
            }
        }

        public class BadRequestException : ApiException
        {
            public BadRequestException(string message)
                : base(message, HttpStatusCode.BadRequest)
            {
            }
        }

        public class UnauthorizedException : ApiException
        {
            public UnauthorizedException(string message)
                : base(message, HttpStatusCode.Unauthorized)
            {
            }
        }

        public class BussinessException : ApiException
        {
            public BussinessException(string message)
                    : base(message, HttpStatusCode.BadRequest) // or another appropriate status code
            {
            }
        }


        public class ValidationException : ApiException
        {
            public List<ValidationFailure> Errors { get; }
            public ValidationException(List<ValidationFailure> errors)
                : base("Validation failed", HttpStatusCode.UnprocessableEntity) // or another appropriate status code
            {
                Errors = errors;
            }
        }
        public class ForbiddenException : ApiException
        {
            public ForbiddenException(string message)
                : base(message, HttpStatusCode.Forbidden)
            {
            }
        }
    }
}
