using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.SaveTodo
{
    public class SaveTodoRequestHandler : IRequestHandler<SaveTodoRequest, string>
    {
        private readonly IToDoStore store;

        public SaveTodoRequestHandler(
            IToDoStore store )
        {
            this.store = store;
        }

        public Task<string> Handle(
            SaveTodoRequest request,
            CancellationToken cancellationToken )
        {
            var todo = request.ToTodo();
            
            var createdId = this.store.AddOrUpdateTodo( todo );

            return Task.FromResult( createdId );
        }
    }
}