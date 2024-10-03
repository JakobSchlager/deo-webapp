public class CounterService(CounterHttpClient http) : ICounterService
{
  public Task<int> GetValueAsync() => http.GetValueAsync();
  public Task<(int Before, int After)> IncrementValueAsync() => http.IncrementValueAsync();
  public Task<(int Before, int After)> ResetValueAsync() => http.ResetValueAsync();
}
