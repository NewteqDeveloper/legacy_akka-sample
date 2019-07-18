using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Config
{
    public class AkkaConfig
    {
        public ActorConfig actor { get; set; }
    }

    public class ActorConfig
    {
        public DeploymentConfig deployment { get; set; }
    }

    public class DeploymentConfig
    {
        [JsonProperty(PropertyName = "/user/echo")]
        public ActorSettingsConfig echo { get; set; }
    }

    public class ActorSettingsConfig
    {
        [JsonProperty(PropertyName = "nr-of-instances")]
        public int nrofinstances { get; set; }
        public ResizerConfig resizer { get; set; }
    }

    public class ResizerConfig
    {
        public string enabled { get; set; }
        [JsonProperty(PropertyName = "lower-bound")]
        public int lowerbound { get; set; }
        [JsonProperty(PropertyName = "upper-bound")]
        public int upperbound { get; set; }
    }
}
