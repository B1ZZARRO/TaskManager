using RestSharp;

namespace TaskManager.API;

public class ServiceBuilder
{
    private static string rootUrl = "https://localhost:5001/api";
    private static RestClient _restClient;

    public static RestClient GetInstance()
    {
        if (_restClient == null)
        {
            _restClient = new RestClient(rootUrl);
        }

        return _restClient;
    }
}