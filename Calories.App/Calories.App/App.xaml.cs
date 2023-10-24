using Calories.App.Helper;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Calories.App;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(App));

    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            ConfigureServices(services);
        }).Build();

        InitLog();
        log4net.Config.XmlConfigurator.Configure();
        var hierarchy = (Hierarchy)LogManager.GetRepository();
        var rootLogger = hierarchy.Root;
        rootLogger.Level = hierarchy.LevelMap["ERROR"];
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjYzNDM4MUAzMjMyMmUzMDJlMzBPSlpjMU8xYkljcDRhT1BTVEF5eHNJWlllcnUvTzlsYXhLUno0azFCd05rPQ==");
        Log.Info($@"Start Program");
        Log.Info("This is an info message."); // Won't be logged due to log level being set to DEBUG
        Log.Debug("This is a debug message.");
        Log.Error("This is a error message.");

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Views.MainWindow>();
        services.AddTransient<ViewModels.VmMainWindow>();

        //services.AddSingleton<NavigationService>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        _host.Services.GetRequiredService<Views.MainWindow>().Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        base.OnExit(e);
    }

    private void InitLog()
    {
        string logConfig = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
	<log4net>
		<root>
			<level value=""ALL"" />
			<appender-ref ref=""console"" />
			<appender-ref ref=""file"" />
		</root>
		<appender name=""console"" type=""log4net.Appender.ConsoleAppender"">
			<layout type=""log4net.Layout.PatternLayout"">
				<conversionPattern value=""%date{{HH:mm:ss.fff}} %level %logger - %message%newline"" />
			</layout>
		</appender>
		<appender name=""file"" type=""log4net.Appender.RollingFileAppender"">
			<file type=""log4net.Util.PatternString"" value=""%env{{LOCALAPPDATA}}\{Utils.AppName}\logs\"" />
			<appendToFile value=""true"" />
			<rollingStyle value=""Date"" />
			<datePattern value=""''yyyy-MM-dd'.log'""/>
			<staticLogFileName value=""false"" />
			<layout type=""log4net.Layout.PatternLayout"">
				<conversionPattern value=""%date{{HH:mm:ss.fff}},%level,%logger [%line],%message%newline"" />
			</layout>
		</appender>
	</log4net>
</configuration>";
        var doc = new System.Xml.XmlDocument();
        doc.LoadXml(logConfig);

        var element = doc["configuration"]?["log4net"];

        log4net.Config.XmlConfigurator.Configure(element);
    }
}