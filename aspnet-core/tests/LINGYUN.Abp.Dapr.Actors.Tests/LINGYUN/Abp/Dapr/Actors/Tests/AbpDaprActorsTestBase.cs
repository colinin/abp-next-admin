using LINGYUN.Abp.Tests;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace LINGYUN.Abp.Dapr.Actors.Tests
{
    public class AbpDaprActorsTestBase : AbpTestsBase<AbpDaprActorsTestModule>
    {
        // private Process _hostProcess;

        //protected override void BeforeAddApplication(IServiceCollection services)
        //{
        //    // TODO: 运行测试前先启动宿主进程 dapr run --app-id testdapr --app-port 5000 -H 10000 -- dotnet run --no-build
        //    var workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../../../../LINGYUN.Abp.Dapr.AspNetCore.TestHost");
        //    _hostProcess = Process.Start(new ProcessStartInfo
        //    {
        //        WorkingDirectory = workingDirectory,
        //        FileName = "powershell",
        //        Arguments = "dapr run --app-id testdapr --app-port 5000 -H 10000 -- dotnet run --no-build",
        //        UseShellExecute = true
        //    });

        //    // 等待.NET进程启动完毕
        //    Thread.Sleep(15000);
        //}

        //public override void Dispose()
        //{
        //    _hostProcess?.CloseMainWindow();
        //}
    }
}
