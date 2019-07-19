using Akka.Actor;
using AkkaSample.Api.Deps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaSample.Api.Actors
{
    public class PutActor : UntypedActor
    {
        public PutActor()
        {

        }

        public PutActor(ICustomMouse mouse)
        {
            mouse.Move();
        }

        protected override void OnReceive(object message)
        {
            if (message.ToString().ToLower().Contains("1"))
            {
                Thread.Sleep(1000);
            }
            FakeDb.Rows.Add($"Message from actor: {message.ToString()}");
            Console.WriteLine($"Message processed by put: {message}");
        }
    }
}
