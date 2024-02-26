using System.Net.Http.Json;

namespace InsightBytes.Utilities.Endpoints;
public class GolangService
{
    private readonly HttpClient _httpClient;

    public GolangService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<User>> GetUserDataAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:8080/api/users/");
        response.EnsureSuccessStatusCode();

        // Deserialize the JSON response to the UsersResponse class
        var usersResponse = await response.Content.ReadFromJsonAsync<UsersResponse>();

        return usersResponse?.Users;
    }
}

public class UsersResponse
{
    public List<User> Users { get; set; }
}
public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
