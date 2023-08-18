
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using Serilog;
using Serilog.Core;

namespace Ocelot.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAutoOcelotConfig(this IConfigurationBuilder builder, IWebHostEnvironment env)
        {
            return builder.AddAutoOcelotConfig(".", env);
        }

        static object locker = new object();

        public static IConfigurationBuilder AddAutoOcelotConfig(this IConfigurationBuilder builder, string folder, IWebHostEnvironment env)
        {
            WriteFile(folder, env);
            Action<object, FileSystemEventArgs> fileChanged = (sender, e) =>
            {
                lock (locker)
                {
                    Log.Debug("Ocelot config regenerate...");
                    Thread.Sleep(100);  //解决vs文件保存时多次触发change事件时引起异常
                    WriteFile(folder, env);
                }
            };

            FileSystemWatcher watcher = new FileSystemWatcher("OcelotConfig", "*.json");
            watcher.Changed += new FileSystemEventHandler(fileChanged);
            watcher.Deleted += new FileSystemEventHandler(fileChanged);
            watcher.Created += new FileSystemEventHandler(fileChanged);
            watcher.Renamed += new RenamedEventHandler(fileChanged);
            watcher.EnableRaisingEvents = true;

            builder.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);
            return builder;
        }

        private static void WriteFile(string folder, IWebHostEnvironment env)
        {
            string excludeConfigName = ((env != null && env.EnvironmentName != null) ? ("ocelot." + env.EnvironmentName + ".json") : string.Empty);
            Regex reg = new Regex("^ocelot\\.(.*?)\\.json$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            List<FileInfo> list = (from fi in new DirectoryInfo(folder).EnumerateFiles()
                                   where reg.IsMatch(fi.Name) && fi.Name != excludeConfigName
                                   select fi).ToList();
            FileConfiguration fileConfiguration = new FileConfiguration();
            foreach (FileInfo item in list)
            {
                if (list.Count <= 1 || !item.Name.Equals("ocelot.json", StringComparison.OrdinalIgnoreCase))
                {
                    FileConfiguration fileConfiguration2 = JsonConvert.DeserializeObject<FileConfiguration>(File.ReadAllText(item.FullName));
                    if (fileConfiguration2 == null)
                    {
                        Log.Fatal($"Ocelot config file \"{item.FullName}\" is empty");
                    }
                    if (item.Name.Equals("ocelot.global.json", StringComparison.OrdinalIgnoreCase))
                    {
                        fileConfiguration.GlobalConfiguration = fileConfiguration2.GlobalConfiguration;
                    }

                    fileConfiguration.Aggregates.AddRange(fileConfiguration2.Aggregates);
                    fileConfiguration.Routes.AddRange(fileConfiguration2.Routes);
                }
            }

            string contents = JsonConvert.SerializeObject(fileConfiguration, Formatting.Indented);
            File.WriteAllText("ocelot.json", contents);
        }
    }
}