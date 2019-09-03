using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.SaveTodo
{
    public class SaveTodoRequest : IRequest<string>
    {
        public SaveTodoRequest(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public string Id { get; set; }

        public Todo ToTodo()
        {
            var todo = new Todo( this.Name ) { Id = this.Id };
            return todo;
        }
    }
}