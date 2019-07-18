using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AkkaSample.Actors
{
    public class EchoActor : UntypedActor
    {
        private IActorRef child;

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
            var useMessage = message.ToString();
            WriteMessage($"Message has been received :: {useMessage}");
            if (useMessage.ToLower() == "create")
            {
                if (child == null)
                {
                    child = Context.ActorOf<EchoChildActor>();
                }
            }

            if (child != null)
            {
                child.Tell($"Telling {useMessage}");
                child.Forward($"Forwarding {useMessage}");
            }
        }

        private void WriteMessage(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}
