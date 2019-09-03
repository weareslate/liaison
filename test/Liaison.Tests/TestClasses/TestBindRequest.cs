using MediatR;

namespace Liaison.Tests.TestClasses
{
    public class TestBindRequest : IRequest
    {
        public TestBindRequest(
            string name )
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}