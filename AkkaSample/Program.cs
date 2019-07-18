using Akka.Actor;
using AkkaSample.Actors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("akka-sample");
            var echo = actorSystem.ActorOf<EchoActor>(nameof(EchoActor));
            echo.Tell("Hello World");
            echo.Tell("create");
            echo.Tell("Hello World Again");
            echo.Tell("Hello World, it's me");

            // kill the actors after 5 seconds (the graceful stop and poison pill on it's own is not working like
            // I expect it to at the moment, which is why we have this delay
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                echo.Tell(PoisonPill.Instance);
            });

            // echo.GracefulStop(TimeSpan.FromSeconds(5));

            Console.ReadLine();
        }
    }
}
