public interface ICounterService
{
  Task<int> GetValueAsync();
  Task<(int Before, int After)> IncrementValueAsync();
  Task<(int Before, int After)> ResetValueAsync();
}
