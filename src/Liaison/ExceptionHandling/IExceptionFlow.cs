using System;

namespace Liaison.ExceptionHandling
{
    internal interface IExceptionFlow
    {
        IExceptionWriter GetWriter(
            Exception ex );
    }
}