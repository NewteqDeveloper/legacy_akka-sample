using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using AkkaSample.Api.Actors;
using AkkaSample.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AkkaSample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = ConfigurationFactory.FromObject(new
            {
                akka = Configuration.GetSection("Akka").Get<AkkaConfig>()
            });
            var akkaConfig = ConfigurationLoader.Load();

            services.AddSingleton(x => ActorSystem.Create("my-system", config));
            // services.AddSingleton(x => ActorSystem.Create("my-system", akkaConfig));
            // services.AddSingleton(x => ActorSystem.Create("my-system"));
            services.AddSingleton<EchoActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                // var echoActor = actorSystem.ActorOf<EchoActor>("echo");
                //var echoActor = actorSystem.ActorOf(Props.Create<EchoActor>()
                //    .WithRouter(new RoundRobinPool(5)), "echo");
                var echoActor = actorSystem.ActorOf(Props.Create<EchoActor>().WithRouter(FromConfig.Instance), "echo");
                return () => echoActor;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>();
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
