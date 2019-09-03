using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.GetTodo
{
    public class GetTodoRequest
        : IRequest<Todo>
    {
        public GetTodoRequest(
            string id)
        {
            this.Id = id;
        }    

        public string Id { get; }
    }
}