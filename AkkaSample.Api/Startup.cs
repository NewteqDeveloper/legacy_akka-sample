using Akka.Actor;
using Akka.Configuration;
using Akka.DI.Core;
using Akka.Routing;
using AkkaSample.Api.Actors;
using AkkaSample.Api.Configuration;
using AkkaSample.Api.Deps;
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
        private const string getActorName = "getter";
        private const string putActorName = "putter";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<ICustomMouse, CustomMouse>();

            CreateActorsWithAkkaConf(services);

            var provider = services.BuildServiceProvider();
            new KestrelDependencyResolver(provider, provider.GetService<ActorSystem>());
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
                var actor = actorSystem.ActorOf(Props.Create<EchoActor>().WithRouter(FromConfig.Instance), echoActorName);
                return () => actor;
            });
            services.AddSingleton<GetActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var actor = actorSystem.ActorOf(Props.Create<GetActor>().WithRouter(FromConfig.Instance), getActorName);
                return () => actor;
            });
            services.AddSingleton<PutActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                // var actor = actorSystem.ActorOf(Props.Create<PutActor>().WithRouter(FromConfig.Instance), putActorName);
                var actor = actorSystem.ActorOf(actorSystem.DI().Props<PutActor>().WithRouter(FromConfig.Instance), putActorName);
                return () => actor;
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
