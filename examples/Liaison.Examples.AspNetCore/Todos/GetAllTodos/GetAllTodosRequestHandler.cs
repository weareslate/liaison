using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.GetAllTodos
{
    public class GetAllTodosRequestHandler
        : IRequestHandler<GetAllTodosRequest, IEnumerable<Todo>>
    {
        private readonly IToDoStore store;

        public GetAllTodosRequestHandler(
            IToDoStore store )
        {
            this.store = store;
        }

        public Task<IEnumerable<Todo>> Handle(
            GetAllTodosRequest request,
            CancellationToken cancellationToken )
        {
            return Task.FromResult( this.store.GetTodos() );
        }
    }
}