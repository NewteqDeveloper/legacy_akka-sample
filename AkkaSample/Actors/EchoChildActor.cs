using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AkkaSample.Actors
{
    public class EchoChildActor : UntypedActor
    {
        protected override void PreRestart(Exception reason, object message)
        {
            WriteMessage($"CHILD:: A restart has been requested: The message that was sent that casued the restart is: {message}. The exception is: {reason.Message}");
        }

        protected override void PostRestart(Exception reason)
        {
            WriteMessage($"CHILD:: A restart has been completed for {reason.Message}");
        }

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
            if (useMessage.ToLower().Contains("again"))
            {
                throw new ArithmeticException("This is the exception to test again - ArithmeticException");
            }
            if (useMessage.ToLower().Contains("me"))
            {
                throw new NullReferenceException("This is an exception to test me - NullReferenceException");
            }
            WriteMessage($"Message has been received :: {message}");
        }

        private void WriteMessage(string message)
        {
            Console.WriteLine($"I am a echo child -- {message}");
        }
    }
}
