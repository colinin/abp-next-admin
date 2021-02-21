using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Security.Encryption;
using static System.Console;

namespace LINGYUN.Abp.Encryption.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Hello World!");

            var application = AbpApplicationFactory.Create<AbpEncryptionConsoleModule>();

            application.Initialize();

            WriteLine("D:解密 E:加密");

            var opt = ReadLine();
            bool en = false;
            if ("E".Equals(opt, StringComparison.InvariantCultureIgnoreCase))
            {
                en = true;
                WriteLine("请输入需要加密的字符串");
            }
            else
            {
                WriteLine("请输入需要解密的字符串");
            }
            
            var sourceChr = ReadLine();
            var encryptionService = application.ServiceProvider.GetRequiredService<IStringEncryptionService>();
            WriteLine(en ? encryptionService.Encrypt(sourceChr) : encryptionService.Decrypt(sourceChr));

            application.Shutdown();

            ReadKey();
        }
    }
}
