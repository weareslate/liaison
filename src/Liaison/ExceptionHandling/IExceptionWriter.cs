using System;
using Microsoft.AspNetCore.Http;

namespace Liaison.ExceptionHandling
{
    public interface IExceptionWriter
    {
        bool CanWrite(
            Exception ex );

        void WriteException(
            Exception ex,
            HttpContext context );
    }
}