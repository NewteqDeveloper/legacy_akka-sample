using Akka.Actor;
using AkkaSample.Actors;
using System;

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
            echo.Tell("Hello World Again Again");
            echo.Tell(PoisonPill.Instance);

            Console.ReadLine();
        }
    }
}
