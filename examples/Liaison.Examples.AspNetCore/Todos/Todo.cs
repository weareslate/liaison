namespace Liaison.Examples.AspNetCore.Todos
{
    public class Todo
    {
        public Todo(
            string name)
        {
            this.Name = name;
        }

        public string Id { get; set; }
        public string Name { get; }
    }
}