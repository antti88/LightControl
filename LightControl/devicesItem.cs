using Newtonsoft.Json;

namespace LightControl
{
    public class devicesItem
    {
        [JsonProperty(PropertyName = "deviceId")]
        public int deviceId { get; set; }
        [JsonProperty(PropertyName = "deviceName")]
        public string deviceName { get; set; }
        [JsonProperty(PropertyName = "deviceOn")]
        public string deviceOn { get; set; }
        [JsonProperty(PropertyName = "deviceOff")]
        public string deviceOff { get; set; }
        [JsonProperty(PropertyName = "timer")]
        public int timer { get; set; }
    }
}