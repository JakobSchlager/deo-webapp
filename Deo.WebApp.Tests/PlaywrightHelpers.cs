using PW = Microsoft.Playwright;

namespace Deo.WebApp.Tests;

internal class PlaywrightHelpers
{
  public enum BrowserChoice
  {
    Chromium,
    Firefox,
    Webkit
  }

  public static void Command(string cmd)
  {
    var exitCode = PW.Program.Main([cmd]);
    if (exitCode != 0)
    {
      throw new Exception($"{nameof(Microsoft.Playwright)} exited with code {exitCode} on command {cmd}.");
    }
  }

  public static void InstallDeps() => Command("install-deps");

  public static void Install() => Command("install");

  public static async Task<(PW.IPlaywright PlaywrightInstance, PW.IBrowser BrowserInstance)> LaunchBrowser(BrowserChoice choice, PW.BrowserTypeLaunchOptions options)
  {
    var playwright = await PW.Playwright.CreateAsync();
    var browserType = choice switch
    {
      BrowserChoice.Chromium => playwright.Chromium,
      BrowserChoice.Firefox => playwright.Firefox,
      BrowserChoice.Webkit => playwright.Webkit,
      _ => throw new ArgumentOutOfRangeException(nameof(choice), $"Unknown browser choice \"{choice}\"."),
    };

    var launchedBrowser = await browserType.LaunchAsync(options);
    return (playwright, launchedBrowser);
  }
}
