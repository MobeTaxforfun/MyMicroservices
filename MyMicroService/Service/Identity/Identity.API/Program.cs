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
/// �Ұ�Webhost
/// </summary>
static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
    WebHost.CreateDefaultBuilder(args)                          
        .CaptureStartupErrors(false)                                        //��]�w��false��,�ҰʹL�{�������~�|�ɭP�D���h�X,��]�w�� true ��,�D���|����ҰʹL�{�������`,�åB�չϱҰʦ��A��(�Y�O�ϥ�IIS�ҰʡA�w�]��True)
        .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))  //���U configuration
        .UseStartup<Startup>()                                              //�ϥ� Startup �ҰʩI�s������O(ConfigureServices,Configure)> ConfigureServices:DI���UService��(�i�H����@);
                                                                            //Configure:������ΪA�Ȫ��֤߳W�d,�䤤IApplicationBuilder�����n�ѼƬO�M�wPipeline���a��
        .Build();


/// <summary>
/// ��Ҥ� Config
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