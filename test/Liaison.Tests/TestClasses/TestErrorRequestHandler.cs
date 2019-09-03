using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Tests.TestClasses
{
    public class TestErrorRequestHandler : IRequestHandler<TestErrorRequest>
    {
        private readonly IErrorChoser errorChoser;

        public TestErrorRequestHandler(
            IErrorChoser errorChoser)
        {
            this.errorChoser = errorChoser;
        }
        public Task<Unit> Handle(
            TestErrorRequest request,
            CancellationToken cancellationToken )
        {
            throw this.errorChoser.ThrowMe();
        }
    }
}