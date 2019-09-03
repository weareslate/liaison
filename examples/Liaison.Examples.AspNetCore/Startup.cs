using System.Collections.Generic;
using Liaison.Examples.AspNetCore.Todos;
using Liaison.Examples.AspNetCore.Todos.GetAllTodos;
using Liaison.Examples.AspNetCore.Todos.GetTodo;
using Liaison.Examples.AspNetCore.Todos.SaveTodo;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liaison.Examples.AspNetCore
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration )
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services )
        {
            services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
            services.AddLogging( b => b.AddConsole()
                                       .AddConfiguration( this.Configuration ) );
            
            services.AddSingleton<IToDoStore>( ctx => new FakeToDoStore( new List<Todo>
                                                                         { new Todo( "one" ), new Todo( "two" ) } ) );
            
            services.AddMediatR(this.GetType().Assembly);
            services.AddLiaison();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            this.ConfigureTodos( app );
        }

        public void ConfigureTodos(
            IApplicationBuilder app )
        {
            app.UseLiaison( liaison =>
            {
                // /api/todos
                liaison.Route( "todos" )
                        //  GET {id}
                       .Get<GetTodoRequest>( "{id}" )
                        //  GET
                       .Get<GetAllTodosRequest>()
                        //  POST
                       .Post<SaveTodoRequest>();
            } );
        }
    }
}