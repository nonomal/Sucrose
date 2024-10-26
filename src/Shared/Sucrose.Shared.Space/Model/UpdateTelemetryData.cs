using Newtonsoft.Json;

namespace Sucrose.Shared.Space.Model
{
    internal class UpdateTelemetryData
    {
        [JsonProperty("AppVersion", Required = Required.Always)]
        public string AppVersion { get; set; }

        [JsonProperty("UpdateAutoType", Required = Required.Always)]
        public string UpdateAutoType { get; set; }
    }
}