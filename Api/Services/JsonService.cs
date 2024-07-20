using Newtonsoft.Json;

namespace Api.Services
{
    public class JsonService : IJsonService
    {
        public List<T> GetAll<T>(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string json = File.ReadAllText(filePath);
            List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
            return items;
        }
    }
}
