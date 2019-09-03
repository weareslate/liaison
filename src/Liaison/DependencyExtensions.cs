using System.Threading;
using Liaison.AspNetCore;
using Liaison.ExceptionHandling;
using Liaison.Orchestrator;
using Microsoft.Extensions.DependencyInjection;

namespace Liaison
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddLiaison(
            this IServiceCollection collection )
        {
            collection.AddSingleton<IRouteTable, RouteTable>();
            collection.AddSingleton<IExceptionFlow, ExceptionFlow>();
            collection.AddSingleton<ICancellationTokenHolder>( ctx =>
                                                                   new CancellationTokenHolder( CancellationToken
                                                                                                   .None ) );
            collection.AddSingleton<IMediatRPipelineOrchestratorFactory, MediatRPipelineOrchestratorFactory>();

            return collection;
        }
    }
}