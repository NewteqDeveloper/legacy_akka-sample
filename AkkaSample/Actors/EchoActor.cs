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
                WriteMessage($"Telling the child :: {useMessage}");
                child.Tell($"Telling {useMessage}");
                // WriteMessage($"Forwarding to the child :: {useMessage}");
                // child.Forward($"Forwarding {useMessage}");
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                5,
                TimeSpan.FromMilliseconds(1),
                (ex) =>
                {
                    switch (ex)
                    {
                        case ArithmeticException ae:
                            return Directive.Resume;
                        case NullReferenceException ne:
                            return Directive.Restart;
                        default:
                            return Directive.Stop;
                    }
                });
        }

        private void WriteMessage(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}
