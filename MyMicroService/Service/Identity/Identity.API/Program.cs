using Identity.API;
using Microsoft.AspNetCore;

var configuration = GetConfiguration();

try
{
    var host = BuildWebHost(configuration, args);
    host.Run();
    return 0;
}
catch (Exception ex)
{
    return 1;
}

/// <summary>
/// 啟動Webhost
/// </summary>
static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
    WebHost.CreateDefaultBuilder(args)                          
        .CaptureStartupErrors(false)                                        //當設定為false時,啟動過程中的錯誤會導致主機退出,當設定為 true 時,主機會捕獲啟動過程中的異常,並且試圖啟動伺服器(若是使用IIS啟動，預設為True)
        .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))  //註冊 configuration
        .UseStartup<Startup>()                                              //使用 Startup 啟動呼叫兩個類別(ConfigureServices,Configure)> ConfigureServices:DI註冊Service用(可以不實作);
                                                                            //Configure:整個應用服務的核心規範,其中IApplicationBuilder為必要參數是決定Pipeline的地方
        .Build();


/// <summary>
/// 實例化 Config
/// </summary>
IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Build();

    return builder.Build();
}