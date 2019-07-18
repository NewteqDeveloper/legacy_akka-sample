using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaSample.Api.Actors;
using Microsoft.AspNetCore.Mvc;

namespace AkkaSample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IActorRef echoActor;
        private readonly IActorRef putActor;
        private readonly IActorRef getActor;

        public ValuesController(EchoActorProvider echoActorProvider, PutActorProvider putActorProvider, GetActorProvider getActorProvider)
        {
            this.echoActor = echoActorProvider();
            this.putActor = putActorProvider();
            this.getActor = getActorProvider();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            echoActor.Tell("Hello World");

            var list = new List<string>();

            for(var i = 0; i < 100; i++)
            {
                putActor.Tell($"{i}");
                // var answer = await echoActor.Ask<string>(i);
                // list.Add(answer);
            }
            for(var i = 0; i < 100; i++)
            {
                list.Add(await getActor.Ask<string>(i));
            }

            //return Ok(new string[] { "value1", "value2" });
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
