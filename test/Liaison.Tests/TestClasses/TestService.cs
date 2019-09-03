using System;

namespace Liaison.Tests.TestClasses
{
    class TestService : ITestService
    {
        private readonly Action<string> getName;

        public TestService(Action<string> getName)
        {
            this.getName = getName;
        }
        public void Callme()
        {
            throw new NotImplementedException();
        }

        public void Callme(
            string name )
        {
            this.getName( name );
        }
    }
}