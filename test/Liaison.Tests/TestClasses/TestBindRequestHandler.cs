using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Tests.TestClasses
{
    public class TestBindRequestHandler : IRequestHandler<TestBindRequest>
    {
        private readonly ITestService testService;

        public TestBindRequestHandler(
            ITestService testService)
        {
            this.testService = testService;
        }
        
        public Task<Unit> Handle(
            TestBindRequest request,
            CancellationToken cancellationToken )
        {
            this.testService.Callme(request.Name);       
            
            return Task.FromResult( Unit.Value );
        }
    }
}