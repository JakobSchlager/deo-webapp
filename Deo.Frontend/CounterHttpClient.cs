using System.Text.Json;

public class CounterHttpClient(HttpClient http)
{
  public async Task<int> GetValueAsync()
  {
    var json = await http.GetFromJsonAsync<JsonElement>("/");
    return json.GetProperty("value").GetInt32();
  }

  public async Task<(int, int)> IncrementValueAsync()
  {
    var response = await http.PostAsync("/increment", null);
    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
    return (
      json.GetProperty("before").GetInt32(),
      json.GetProperty("after").GetInt32()
    );
  }

  public async Task<(int, int)> ResetValueAsync()
  {
    var response = await http.PostAsync("/reset", null);
    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
    return (
      json.GetProperty("before").GetInt32(),
      json.GetProperty("after").GetInt32()
    );
  }
}