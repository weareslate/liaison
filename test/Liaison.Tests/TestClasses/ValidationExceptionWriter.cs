using System;
using FluentValidation;
using Liaison.AspNetCore.Extensions;
using Liaison.ExceptionHandling;
using Microsoft.AspNetCore.Http;

namespace Liaison.Tests.TestClasses
{
    internal class ValidationExceptionWriter : IExceptionWriter
    {
        private readonly Action<ValidationException> callback;

        public ValidationExceptionWriter(Action<ValidationException> callback)
        {
            this.callback = callback;
        }
        
        public bool CanWrite(
            Exception ex ) =>
            ex is ValidationException;

        public void WriteException(
            Exception ex,
            HttpContext context )
        {
            var validationException = ( ex as ValidationException );
            this.callback( validationException );
            context.Response.WriteBadResponseAsync( validationException?.ToString() );
        }
    }
}