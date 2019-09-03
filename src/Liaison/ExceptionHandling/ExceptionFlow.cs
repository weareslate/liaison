using System;
using System.Collections.Generic;
using System.Linq;

namespace Liaison.ExceptionHandling
{
    internal class ExceptionFlow
        : IExceptionFlow
    {
        private readonly IEnumerable<IExceptionWriter> exceptionWriters;

        public ExceptionFlow(
            IEnumerable<IExceptionWriter> exceptionWriters )
        {
            this.exceptionWriters = exceptionWriters;
        }

        public IExceptionWriter GetWriter(
            Exception ex )
        {
            return this.exceptionWriters
                       .FirstOrDefault( x => x.CanWrite( ex ) );
        }
    }
}