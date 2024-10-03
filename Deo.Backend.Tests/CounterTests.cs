namespace Deo.Backend.Tests;

public class CounterTests
{
    [Fact(DisplayName = "Counter when initialized is 0.")]
    public void CounterStartsAtZero()
    {
        var sut = new Counter();

        var expected = 0;
        var actual = sut.Get();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter when initialized and incremented by n is n.")]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void CounterIncrementsValue(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = inc;
        var actual = sut.Get();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter when initialized and incremented m times by n is m * n.")]
    [InlineData(1, 4)]
    [InlineData(3, 8)]
    [InlineData(6, 5)]
    public void CounterIncrementsValueNumTimes(int inc, int num)
    {
        var sut = new Counter();

        for (int i = 0; i < num; i++)
        {
            sut.Increment(inc);
        }

        var expected = num * inc;
        var actual = sut.Get();

        Assert.Equal(expected, actual);
    }

    [Fact(DisplayName = "Counter when incremeneted and reset is 0.")]
    public void CounterResetsToZero()
    {
        var sut = new Counter();

        var num = 5;
        var inc = 2;
        for (int i = 0; i < num; i++)
        {
            sut.Increment(inc);
        }

        {
            var expected = num * inc;
            var actual = sut.Get();
            Assert.Equal(expected, actual);
        }

        sut.Reset();

        {
            var expected = 0;
            var actual = sut.Get();
            Assert.Equal(expected, actual);
        }
    }

    [Theory(DisplayName = "Counter is even when value is even.")]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public void CounterIsEven(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = true;
        var actual = sut.IsEven();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is not odd when value is even.")]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public void CounterIsNotOdd(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = false;
        var actual = sut.IsOdd();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is odd when value is odd.")]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void CounterIsOdd(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = true;
        var actual = sut.IsOdd();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is not even when value is odd.")]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void CounterIsNotEven(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = false;
        var actual = sut.IsEven();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is positive when value is positive.")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CounterIsPositive(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = true;
        var actual = sut.IsPositive();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is not negative when value is positive.")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CounterIsNotNegative(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = false;
        var actual = sut.IsNegative();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is negative when value is negative.")]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-3)]
    public void CounterIsNegative(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = true;
        var actual = sut.IsNegative();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "Counter is not positive when value is negative.")]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-3)]
    public void CounterIsNotPositive(int inc)
    {
        var sut = new Counter();

        sut.Increment(inc);
        var expected = false;
        var actual = sut.IsPositive();

        Assert.Equal(expected, actual);
    }
}