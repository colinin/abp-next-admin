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

            while (true)
            {
                WriteLine("D:解密 E:加密 Q: 退出");

                var opt = ReadLine();
                var en = false;

                if (opt == "Q")
                {
                    break;
                }

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

                WriteLine("按任意键继续");

                ReadKey();

                Clear();
            }

            application.Shutdown();
        }
    }
}
