using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.GetTodo
{
    public class GetTodoRequestHandler
        : IRequestHandler<GetTodoRequest, Todo>
    {
        private readonly IToDoStore store;

        public GetTodoRequestHandler(
            IToDoStore store)
        {
            this.store = store;
        }
        
        public Task<Todo> Handle(
            GetTodoRequest request,
            CancellationToken cancellationToken )
        {
            var todo = this.store.GetById( request.Id );
            
            return Task.FromResult( todo );
        }
    }
}