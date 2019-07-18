using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaSample.Actors
{
    public class EchoActor : UntypedActor
    {
        protected override void PreStart()
        {
            base.PreStart();
            WriteMessage($"In PreStart");
        }

        protected override void PostStop()
        {
            base.PostStop();
            WriteMessage($"In PostStop");
        }

        protected override void OnReceive(object message)
        {
            WriteMessage($"Message has been received :: {message}");
        }

        private void WriteMessage(string message)
        {
            Console.WriteLine($"Action Message from {nameof(EchoActor)}. This message is:\r\n--{message}");
        }
    }
}
