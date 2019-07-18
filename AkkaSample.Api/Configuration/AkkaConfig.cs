using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Configuration
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
        [JsonProperty(PropertyName = "/echo")]
        public ActorSettingsConfig echo { get; set; }
    }

    public class ActorSettingsConfig
    {
        public string router { get; set; }
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
