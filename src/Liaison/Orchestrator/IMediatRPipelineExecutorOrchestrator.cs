using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liaison.Orchestrator
{
    public interface IMediatRPipelineExecutorOrchestrator
    {
        Task<object> SendAsync(
            string rawRequestData,
            IDictionary<string, object> requestParams = null,
            string dataObjectKey = null,
            CancellationToken cancellationToken = default(CancellationToken) );

        Task<TReturn> SendAsync< TReturn >(
            string rawRequestData,
            IDictionary<string, object> requestParams = null,
            string dataObjectKey = null,
            CancellationToken cancellationToken = default(CancellationToken) );
    }
}