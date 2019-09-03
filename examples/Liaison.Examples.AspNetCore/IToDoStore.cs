using System.Collections.Generic;
using Liaison.Examples.AspNetCore.Todos;

namespace Liaison.Examples.AspNetCore
{
    public interface IToDoStore
    {
        string AddOrUpdateTodo(
            Todo todo );

        IEnumerable<Todo> GetTodos();

        Todo GetById(
            string id );
    }
}