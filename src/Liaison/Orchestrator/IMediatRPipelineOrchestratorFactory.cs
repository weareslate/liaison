using System;
using MediatR;

namespace Liaison.Orchestrator
{
    public interface IMediatRPipelineOrchestratorFactory
    {
        IMediatRPipelineExecutorOrchestrator Create< TRequest >(
            IMediator mediator );

        IMediatRPipelineExecutorOrchestrator Create(
            Type requestType,
            IMediator mediator );
    }
}