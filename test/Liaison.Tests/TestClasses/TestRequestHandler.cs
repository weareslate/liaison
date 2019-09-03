using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Tests.TestClasses
{
    public class TestRequestHandler 
        : IRequestHandler<TestRequest>
    {
        private readonly ITestService testService;

        public TestRequestHandler(
            ITestService testService)
        {
            this.testService = testService;
        }
        
        public Task<Unit> Handle(
            TestRequest request,
            CancellationToken cancellationToken )
        {
            this.testService.Callme();
            // it told me to
            
            return Task.FromResult( Unit.Value );
        }
    }
}