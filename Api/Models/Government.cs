using Newtonsoft.Json;

namespace Api.Models
{
    public class Government
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("governorate_name_ar")]
        public string GovernorateNameAr { get; set; }
        [JsonProperty("governorate_name_en")]
        public string GovernorateNameEn { get; set; }
    }
}
