using Finonex.Automation.Project.Common;
using Finonex.Automation.Project.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;

namespace Finonex.Automation.Project.Tests;

[Category("Production")]
public class FinonexNavigateTest : TestBase
{
    private IPage _page = null!;
    private static ILogger Logger => Log.ForContext<FinonexNavigateTest>();

    [SetUp]
    public async Task Setup()
    {
        Logger.Information("Warm up system");
        _page = await BrowserContext.NewPageAsync();

        if (ConfigurationManager.FullUrl != null) await _page.GotoAsync(ConfigurationManager.FullUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task NavigateAllPages()
    {
        //about
        await _page.ClickAsync("#menu-item-14 > a");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        //product
        await _page.ClickAsync("#menu-item-240 > a");
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        //careers
        await _page.ClickAsync("#menu-item-18 > a");
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        //contact
        await _page.ClickAsync("#menu-item-39 > a");
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}