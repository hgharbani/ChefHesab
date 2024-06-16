using Newtonsoft.Json;

namespace ChefHesab.Share.model.KendoModel
{
    public class AggregateRequest
    {
        [JsonProperty("field")]
        public string? Field { get; set; }

        [JsonProperty("aggregate")]
        public string? Aggregate { get; set; }
    }
}