using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Speakers;
using GraphQL.Tracks;
using GraphQL.Sessions;

namespace GraphQL
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllersWithViews(options =>{
            //     var complexModelBinderProvider = options.ModelBinderProviders.OfType<ComplexObjectModelBinderProvider>();
            // });
        //   services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"));
          services.AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"));
          services.AddGraphQLServer()
                // .AddQueryType<Query>()
                .AddQueryType( d => d.Name("Query"))
                .AddType<SpeakerQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<SessionMutations>()
                    .AddTypeExtension<SpeakerMutations>()
                    .AddTypeExtension<TrackMutations>()
                    // .AddType<SpeakerMutations>()
                .AddType<AttendeeType>()
                .AddType<SessionType>()
                .AddType<SpeakerType>()
                .AddType<TrackType>()
                // we will refactor the schema to a proper relay style.
                // The first thing we have to do here is to EnableRelaySupport on the schema. After that,
                // we will focus on the first Relay server specification called Global Object Identification
                .EnableRelaySupport()
                // .AddMutationType<SpeakerMutations>()
                .AddDataLoader<SpeakerByIdDataLoader>()
                .AddDataLoader<SessionByIdDataLoader>()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // app.UseGraphQL();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
            });
        }
    }
}
