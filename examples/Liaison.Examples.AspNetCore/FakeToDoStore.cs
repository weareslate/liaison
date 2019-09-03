using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Liaison.Extensions;
using Liaison.Examples.AspNetCore.Todos;

namespace Liaison.Examples.AspNetCore
{
    public class FakeToDoStore : IToDoStore
    {
        private readonly ConcurrentDictionary<string, Todo> store;

        public FakeToDoStore(IEnumerable<Todo> sampleData)
        {
            this.store = new ConcurrentDictionary<string, Todo>();
            sampleData.ForEach( t =>
            {
                var id = Guid.NewGuid().ToString( "N" );
                t.Id = id;
                this.store.TryAdd( id, t );
            } );
        }

        public string AddOrUpdateTodo(
            Todo todo )
        {
            todo.Id = string.IsNullOrWhiteSpace( todo.Id )
                          ? Guid.NewGuid().ToString( "N" )
                          : todo.Id;

            this.store
                .AddOrUpdate( todo.Id,
                              (
                                  key ) => todo,
                              (
                                  key,
                                  foundTodo ) => todo );

            return todo.Id;
        }

        public IEnumerable<Todo> GetTodos()
        {
            return this.store.Values;
        }

        public Todo GetById(
            string id )
        {
            var result = this.store.TryGetValue( id, out var todo );

            if ( result )
            {
                return todo;
            }

            return null;
        }
    }
}