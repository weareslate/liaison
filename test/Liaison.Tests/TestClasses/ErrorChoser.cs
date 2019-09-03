using System;

namespace Liaison.Tests.TestClasses
{
    internal class ErrorChoser : IErrorChoser
    {
        private readonly Exception exToThrow;

        public ErrorChoser(Exception exToThrow)
        {
            this.exToThrow = exToThrow;
        }
        public Exception ThrowMe() => this.exToThrow;
    }
}