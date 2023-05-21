using Finonex.Automation.Project.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;

namespace Finonex.Automation.Project.Common;

public class TestBase
{
    private IPlaywright Playwright { get; set; } = null!;
    protected IBrowser Browser { get; set; } = null!;
    protected IBrowserContext BrowserContext { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task OneTimeWarmup()
    {
        /*Init Logger*/
        Log.Logger = LoggerManager.CreateLoggerInstance();

        /*Init AppSetting config*/
        ConfigurationManager.CreateConfigurationContext();

        /*Install webdriver independent on OS*/
        Log.Logger.Debug("Installing/Updating browser");
        var exitCode = Program.Main(new[] { "install" });
        if (exitCode != 0) throw new Exception($"Playwright exited with code {exitCode}");
        Log.Logger.Information("Browser install/update completed");

        /*Initialize browser*/
        Log.Logger.Information("Starting Browser...");
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await BrowserManager.CreateBrowser(Playwright);

        BrowserContext = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                },
                IgnoreHTTPSErrors = true
            }
        );

        Log.Logger.Information(
            "\nBrowser info \ntype: {BrowserType} \nversion: {BrowserVersion} \nisConnected: {IsConnected} \nOpen browsers: {Count}",
            Browser.BrowserType.Name,
            Browser.Version,
            Browser.IsConnected,
            Browser.Contexts.Count);

        Log.Logger.Information("Browser started");
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        Log.Logger.Information("Starting OneTimeTearDown for the TestClass '{ClassName}', ThreadId:  {ThreadId}",
            TestContext.CurrentContext.Test.ClassName,
            Environment.CurrentManagedThreadId);

        /*Dispose browser instance*/
        await BrowserContext.CloseAsync();
        await Browser.CloseAsync();
        Playwright.Dispose();

        Log.Logger.Information("OneTimeTearDown finished successfully for TestClass '{ClassName}'",
            TestContext.CurrentContext.Test.ClassName);
    }
}