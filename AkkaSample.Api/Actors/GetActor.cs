using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Actors
{
    public class GetActor : ReceiveActor
    {
        public GetActor()
        {
            Receive<int>(item =>
            {
                string result;
                if (item < FakeDb.Rows.Count - 1)
                {
                    result = FakeDb.Rows[item];
                }
                else
                {
                    result = "That item was not yet found";
                }
                Sender.Tell(result);
            });
        }
    }
}
