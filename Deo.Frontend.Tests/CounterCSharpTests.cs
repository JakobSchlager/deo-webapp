using Deo.Frontend.Components.Pages;
using Moq;

namespace Deo.Frontend.Tests;

public class CounterCSharpTests : TestContext
{
    [Fact]
    public void CounterStartsAtZero()
    {
        var counterServiceMock = new Mock<ICounterService>();
        counterServiceMock.Setup(x => x.GetValueAsync()).Returns(Task.FromResult(0));
        Services.AddSingleton(counterServiceMock.Object);

        var sut = RenderComponent<LocalCounter>();

        sut.Find("p").MarkupMatches("<p role=\"status\">Current count: 0</p>");
    }
}
