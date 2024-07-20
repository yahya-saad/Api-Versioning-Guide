namespace Api.Services
{
    public interface IJsonService
    {
        List<T> GetAll<T>(string filename);
    }
}
