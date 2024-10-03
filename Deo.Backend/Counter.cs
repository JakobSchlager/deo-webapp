public class Counter
{
  private readonly object locker = new();
  private int value;

  public int Get()
  {
    lock (locker)
    {
      return value;
    }
  }

  public (int Before, int After) Increment(int inc)
  {
    lock (locker)
    {
      return (value, value += inc);
    }
  }

  public (int Before, int After) Reset()
  {
    lock (locker)
    {
      return (value, value = 0);
    }
  }

  public bool IsPositive()
  {
    lock (locker)
    {
      return int.IsPositive(value);
    }
  }

  public bool IsNegative()
  {
    lock (locker)
    {
      return int.IsNegative(value);
    }
  }

  public bool IsEven()
  {
    lock (locker)
    {
      return int.IsEvenInteger(value);
    }
  }

  public bool IsOdd()
  {
    lock (locker)
    {
      return int.IsOddInteger(value);
    }
  }
}