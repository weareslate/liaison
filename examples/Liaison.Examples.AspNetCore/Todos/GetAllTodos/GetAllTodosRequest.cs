using System.Collections.Generic;
using MediatR;

namespace Liaison.Examples.AspNetCore.Todos.GetAllTodos
{
    public class GetAllTodosRequest : IRequest<IEnumerable<Todo>>
    {
    }
}