using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    [DependsOn(
        typeof(AbpEmailingExceptionHandlingModule),
        typeof(AbpTestsBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpExceptionHandlingEmailingTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                BasePath = @"D:\Projects\Development\Abp\ExceptionHandling\Emailing",
                EnvironmentName = "Development"
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 加解密
            Configure<AbpStringEncryptionOptions>(options =>
            {
                options.DefaultPassPhrase = "s46c5q55nxpeS8Ra";
                options.InitVectorBytes = Encoding.ASCII.GetBytes("s83ng0abvd02js84");
                options.DefaultSalt = Encoding.ASCII.GetBytes("sf&5)s3#");
            });

            // 自定义需要处理的异常
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                //  加入需要处理的异常类型
                options.Handlers.Add<AbpException>();
            });
            // 自定义需要发送邮件通知的异常类型
            Configure<AbpEmailExceptionHandlingOptions>(options =>
            {
                // 是否发送堆栈信息
                options.SendStackTrace = true;
                // 未指定异常接收者的默认接收邮件
                options.DefaultReceiveEmail = "colin.in@foxmail.com";
                //  指定某种异常发送到哪个邮件
                options.HandReceivedException<AbpException>("colin.in@foxmail.com");
                options.HandReceivedException<TestSendEmailException>("colin.in@foxmail.com");
            });
        }
    }
}
