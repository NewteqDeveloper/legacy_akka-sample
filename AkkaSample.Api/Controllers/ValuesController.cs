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

        public ValuesController(EchoActorProvider echoActor)
        {
            this.echoActor = echoActor();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            echoActor.Tell("Hello World");
            return new string[] { "value1", "value2" };
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
