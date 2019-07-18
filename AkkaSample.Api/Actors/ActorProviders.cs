using Akka.Actor;

namespace AkkaSample.Api.Actors
{
    public delegate IActorRef EchoActorProvider();
    public delegate IActorRef GetActorProvider();
    public delegate IActorRef PutActorProvider();
}
