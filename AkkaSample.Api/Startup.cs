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

namespace AkkaSample.Api
{
    public class Startup
    {
        private const string actorSystemName = "akka-sample-api";
        private const string echoActorName = "echo";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            CreateActorsWithAkkaConf(services);
        }

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

        private void CreateActorsWithoutConfig(IServiceCollection services, bool includeCodeConfig = false)
        {
            services.AddSingleton(x => ActorSystem.Create(actorSystemName));
            services.AddSingleton<EchoActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();

                IActorRef echoActor;
                if (includeCodeConfig)
                {
                    echoActor = actorSystem.ActorOf(Props.Create<EchoActor>()
                        .WithRouter(new RoundRobinPool(5)), echoActorName);
                }
                else
                {
                    echoActor = actorSystem.ActorOf<EchoActor>(echoActorName);
                }
                return () => echoActor;
            });
        }

        private void CreateActorsWithAkkaConf(IServiceCollection services)
        {
            var akkaConfig = ConfigurationLoader.Load();

            services.AddSingleton(x => ActorSystem.Create(actorSystemName, akkaConfig));
            services.AddSingleton<EchoActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var echoActor = actorSystem.ActorOf(Props.Create<EchoActor>().WithRouter(FromConfig.Instance), echoActorName);
                return () => echoActor;
            });
        }

        private void CreateActorsWithAppSettings(IServiceCollection services)
        {
            var config = ConfigurationFactory.FromObject(new
            {
                akka = Configuration.GetSection("Akka").Get<AkkaConfig>()
            });

            services.AddSingleton(x => ActorSystem.Create(actorSystemName, config));
            services.AddSingleton<EchoActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var echoActor = actorSystem.ActorOf(Props.Create<EchoActor>().WithRouter(FromConfig.Instance), echoActorName);
                return () => echoActor;
            });
        }
    }
}
