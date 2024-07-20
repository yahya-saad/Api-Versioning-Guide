using Newtonsoft.Json;

namespace Api.Models
{
    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("city_name_ar")]
        public string CityNameAr { get; set; }

        [JsonProperty("city_name_en")]
        public string CityNameEn { get; set; }

        [JsonProperty("governorate_id")]
        public int GovernmentId { get; set; }

        public Government Government { get; set; }
    }
}
