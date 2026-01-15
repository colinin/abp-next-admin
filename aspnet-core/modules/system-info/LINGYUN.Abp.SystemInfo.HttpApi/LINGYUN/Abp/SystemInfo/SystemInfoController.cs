using DotNetCore.CAP;
using LINGYUN.Abp.SystemInfo.Models;
using LINGYUN.Abp.SystemInfo.Permissions;
using LINGYUN.Abp.SystemInfo.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SystemInfo;

[Controller]
[Route("api/system-info")]
[Authorize(SystemInfoPermissions.Default)]
public class SystemInfoController : AbpControllerBase
{
    [HttpGet]
    public async virtual Task<SystemInfoModel> GetSystemInfoAsync()
    {
        return new SystemInfoModel
        {
            Components = new ComponentInfoModel[]
            {
                GetSystemInfo(),
                GetPerformanceInfo(),
                GetRedisInfo(),
                GetCapInfo(),
            },
        };
    }

    private ComponentInfoModel GetSystemInfo()
    {
        var env = HttpContext.RequestServices.GetService<IHostEnvironment>();
        using var process = Process.GetCurrentProcess();

        var systemKeys = new List<ComponentKeyModel>
        {
            new ComponentKeyModel("sys_machine_name", "机器名称"),
            new ComponentKeyModel("sys_environment", "运行环境"),
            new ComponentKeyModel("sys_framework_version", ".NET版本"),
            new ComponentKeyModel("sys_os_version", "操作系统版本"),
            new ComponentKeyModel("sys_app_db_provider", "数据库"),
            new ComponentKeyModel("sys_app_version", "应用版本"),
            new ComponentKeyModel("sys_app_build_time", "编译时间"),
            new ComponentKeyModel("sys_app_start_time", "启动时间"),
        };

        var systemDetails = new Dictionary<string, object>
        {
            { "sys_machine_name", Environment.MachineName },
            { "sys_environment", env?.EnvironmentName ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
            { "sys_framework_version", RuntimeInformation.FrameworkDescription },
            { "sys_os_version", RuntimeInformation.OSDescription },
            { "sys_app_db_provider", Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER") },
            { "sys_app_start_time", process.StartTime.ToString("yyyy-MM-dd HH:mm:ss") },
        };

        try
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var assemblyName = assembly.GetName();
            var buildTime = GetBuildTime(assembly);

            systemDetails.Add("sys_app_version", fileVersionInfo.FileVersion ?? assemblyName.Version?.ToString() ?? "N/A");
            systemDetails.Add("sys_app_build_time", buildTime.HasValue ? buildTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A");
        }
        catch
        {
            systemDetails.Add("sys_app_version", "N/A");
            systemDetails.Add("sys_app_build_time", "N/A");
        }

        return new ComponentInfoModel("系统", systemKeys.ToArray(), systemDetails);
    }

    private ComponentInfoModel GetPerformanceInfo()
    {
        using var process = Process.GetCurrentProcess();

        var memoryMetrics = new MemoryMonitor().GetMemoryMetrics();
        var threadMetrics = new ThreadMonitor().GetThreadMetrics();
        var gcMetrics = new GCMonitor().GetGCMetrics();

        return new ComponentInfoModel(
            "性能",
            new ComponentKeyModel[]
            {
                new ComponentKeyModel("perf_total_memory", "总内存(MB)"),
                new ComponentKeyModel("perf_working_set", "工作集内存(MB)"),
                new ComponentKeyModel("perf_private_memory", "私有内存(MB)"),
                new ComponentKeyModel("perf_gc_heap_size", "GC堆大小(MB)"),
                new ComponentKeyModel("perf_gc0", "Gen0回收次数"),
                new ComponentKeyModel("perf_gc1", "Gen1回收次数"),
                new ComponentKeyModel("perf_gc2", "Gen2回收次数"),
                new ComponentKeyModel("perf_active_total_memory", "总可用内存"),
                new ComponentKeyModel("perf_memory_load", "内存负载"),
                new ComponentKeyModel("perf_fragmented_bytes", "内存碎片大小"),
                new ComponentKeyModel("perf_available_worker_threads", "可用工作线程数"),
                new ComponentKeyModel("perf_available_completion_port_threads", "可用I/O线程数"),
                new ComponentKeyModel("perf_max_worker_threads", "最大工作线程数"),
                new ComponentKeyModel("perf_max_completion_port_threads", "最大I/O线程数"),
                new ComponentKeyModel("perf_active_threads", "进程总线程数"),
                new ComponentKeyModel("perf_thread_pool_thread_count", "线程池活动线程数"),
            },
            new Dictionary<string, object>
            {
                { "perf_total_memory", memoryMetrics.TotalMemory },
                { "perf_working_set", memoryMetrics.WorkingSet },
                { "perf_private_memory", memoryMetrics.PrivateMemory },
                { "perf_gc_heap_size", memoryMetrics.GCHeapSize },
                { "perf_gc0", gcMetrics.Gen0Collections },
                { "perf_gc1", gcMetrics.Gen1Collections },
                { "perf_gc2", gcMetrics.Gen2Collections },
                { "perf_active_total_memory", gcMetrics.TotalMemory },
                { "perf_memory_load", gcMetrics.MemoryLoad },
                { "perf_fragmented_bytes", gcMetrics.FragmentedBytes },
                { "perf_available_worker_threads", threadMetrics.AvailableWorkerThreads },
                { "perf_available_completion_port_threads", threadMetrics.AvailableCompletionPortThreads },
                { "perf_max_worker_threads", threadMetrics.MaxWorkerThreads },
                { "perf_max_completion_port_threads", threadMetrics.MaxCompletionPortThreads },
                { "perf_active_threads", threadMetrics.ActiveThreads },
                { "perf_thread_pool_thread_count", threadMetrics.ThreadPoolThreadCount },
            });
    }

    private ComponentInfoModel GetCapInfo()
    {
        var cap = HttpContext.RequestServices.GetService<CapMarkerService>();
        var storage = HttpContext.RequestServices.GetService<CapStorageMarkerService>();
        var broker = HttpContext.RequestServices.GetService<CapMessageQueueMakerService>();

        var capKeys = new List<ComponentKeyModel>
        {
            new ComponentKeyModel("cap_status", "状态"),
            new ComponentKeyModel("cap_version", "版本"),
            new ComponentKeyModel("cap_storage", "持久化"),
            new ComponentKeyModel("cap_transport", "传输"),
        };
        var capDetails = new Dictionary<string, object>
        {
            { "cap_status", cap != null ? "正常" : "未启用" },
            { "cap_version", cap != null ? cap.Version.Substring(0, 5) : "N/A" },
            { "cap_storage", storage != null ? storage.Name : "N/A" },
            { "cap_transport", broker != null ? broker.Name : "N/A" }
        };

        return new ComponentInfoModel("CAP组件", capKeys.ToArray(), capDetails);
    }

    private ComponentInfoModel GetRedisInfo()
    {
        var redis = HttpContext.RequestServices.GetService<IConnectionMultiplexer>();
        if (redis == null)
        {
            return new ComponentInfoModel(
                "Redis组件",
                new ComponentKeyModel[]
                {
                    new ComponentKeyModel("redis_status", "状态")
                },
                new Dictionary<string, object>
                {
                    { "redis_status", "未注册" },
                });
        }
        var redisInfo = new Dictionary<string, string>();

        try
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var serverInfo = server.Info();
            foreach (var section in serverInfo)
            {
                foreach (var item in section)
                {
                    redisInfo[item.Key] = item.Value;
                }
            }
        }
        catch(Exception ex)
        {
            Logger.LogWarning("There is an exception in connecting to the redis server: {message}", ex.Message);

            return new ComponentInfoModel(
                "Redis组件",
                new ComponentKeyModel[]
                {
                    new ComponentKeyModel("redis_status", "状态")
                },
                new Dictionary<string, object>
                {
                    { "redis_status", "连接异常" },
                });
        }

        var uptimeSeconds = redisInfo.ContainsKey("uptime_in_seconds") ? long.Parse(redisInfo["uptime_in_seconds"]) : 0;
        var days = Math.Floor((double)uptimeSeconds / 86400);
        var hours = Math.Floor((double)uptimeSeconds % 86400 / 3600);
        var minutes = Math.Floor((double)uptimeSeconds % 3600 / 60);

        var usedMemory = redisInfo.ContainsKey("used_memory") ? long.Parse(redisInfo["used_memory"]) : 0;
        var maxMemory = redisInfo.ContainsKey("maxmemory") ? long.Parse(redisInfo["maxmemory"]) : 0;
        var totalKeys = CalculateTotalKeys(redisInfo);
        var expiresKeys = CalculateExpires(redisInfo);
        var avgTtl = CalculateAvgTtl(redisInfo);

        var redisKeys = new List<ComponentKeyModel>()
        {
            new ComponentKeyModel("redis_version", "版本"),
            new ComponentKeyModel("redis_mode", "运行模式"),
            new ComponentKeyModel("redis_os", "操作系统"),
            new ComponentKeyModel("redis_uptime", "运行时间"),
            new ComponentKeyModel("redis_used_cpu_sys", "系统CPU使用时间"),
            new ComponentKeyModel("redis_used_cpu_user", "用户CPU使用时间"),
            new ComponentKeyModel("redis_used_memory_mb", "使用内存(MB)"),
            new ComponentKeyModel("redis_used_memory_rss_mb", "物理内存(MB)"),
            new ComponentKeyModel("redis_used_memory_peak_mb", "内存使用峰值(MB)"),
            new ComponentKeyModel("redis_maxmemory_mb", "最大内存限制(MB)"),
            new ComponentKeyModel("redis_memory_usage_percent", "内存使用率(%)"),
            new ComponentKeyModel("redis_mem_fragmentation_ratio", "内存碎片率"),
            new ComponentKeyModel("redis_connected_clients", "已连接客户端数"),
            new ComponentKeyModel("redis_blocked_clients", "阻塞客户端数"),
            new ComponentKeyModel("redis_client_longest_output_list", "客户端最长输出列表"),
            new ComponentKeyModel("redis_client_biggest_input_buf", "客户端最大输入缓冲区"),
            new ComponentKeyModel("redis_maxclients", "最大客户端数"),
            new ComponentKeyModel("redis_total_keys", "总键数量"),
            new ComponentKeyModel("redis_expires_keys", "设置过期键数量"),
            new ComponentKeyModel("redis_expired_keys", "已过期键数量"),
            new ComponentKeyModel("redis_evicted_keys", "被驱逐键数量"),
            new ComponentKeyModel("redis_avg_ttl_seconds", "平均TTL(秒)"),
        };
        var redisDetails = new Dictionary<string, object>()
        {
            { "redis_version", redisInfo.GetValueOrDefault("redis_version", "unknown") },
            { "redis_mode", redisInfo.GetValueOrDefault("redis_mode", "unknown") },
            { "redis_os", redisInfo.GetValueOrDefault("os", "unknown") },
            { "redis_uptime", $"{days}天{hours}时{minutes}分" },
            { "redis_used_cpu_sys", redisInfo.GetValueOrDefault("used_cpu_sys", "0") },
            { "redis_used_cpu_user", redisInfo.GetValueOrDefault("used_cpu_user", "0") },
            { "redis_used_memory_mb", Math.Round((double)usedMemory / 1024 / 1024, 2) },
            { "redis_used_memory_rss_mb", redisInfo.ContainsKey("used_memory_rss") ?
                    Math.Round(double.Parse(redisInfo["used_memory_rss"]) / 1024 / 1024, 2) : 0 },
            { "redis_used_memory_peak_mb", redisInfo.ContainsKey("used_memory_peak") ?
                    Math.Round(double.Parse(redisInfo["used_memory_peak"]) / 1024 / 1024, 2) : 0 },
            { "redis_maxmemory_mb", maxMemory > 0 ? Math.Round((double)maxMemory / 1024 / 1024, 2) : 0 },
            { "redis_memory_usage_percent", maxMemory > 0 ? Math.Round((double)usedMemory / maxMemory * 100, 2) : 0 },
            { "redis_mem_fragmentation_ratio", redisInfo.GetValueOrDefault("mem_fragmentation_ratio", "0") },
            { "redis_connected_clients", redisInfo.GetValueOrDefault("connected_clients", "0") },
            { "redis_blocked_clients", redisInfo.GetValueOrDefault("blocked_clients", "0") },
            { "redis_client_longest_output_list", redisInfo.GetValueOrDefault("client_longest_output_list", "0") },
            { "redis_client_biggest_input_buf", redisInfo.GetValueOrDefault("client_biggest_input_buf", "0") },
            { "redis_maxclients", redisInfo.GetValueOrDefault("maxclients", "unlimited") },
            { "redis_total_keys", totalKeys },
            { "redis_expires_keys", expiresKeys },
            { "redis_expired_keys", redisInfo.GetValueOrDefault("expired_keys", "0") },
            { "redis_evicted_keys", redisInfo.GetValueOrDefault("evicted_keys", "0") },
            { "redis_avg_ttl_seconds", avgTtl },
        };

        return new ComponentInfoModel("Redis组件", redisKeys.ToArray(), redisDetails);
    }

    private long CalculateTotalKeys(Dictionary<string, string> redisInfo)
    {
        long total = 0;

        foreach (var item in redisInfo)
        {
            if (item.Key.StartsWith("db"))
            {
                var keysPart = item.Value.Split(',').FirstOrDefault(x => x.StartsWith("keys="));
                if (keysPart != null && long.TryParse(keysPart.Split('=')[1], out var count))
                {
                    total += count;
                }
            }
        }

        return total;
    }

    private long CalculateExpires(Dictionary<string, string> redisInfo)
    {
        long expires = 0;

        foreach (var item in redisInfo)
        {
            if (item.Key.StartsWith("db"))
            {
                var expiresPart = item.Value.Split(',').FirstOrDefault(x => x.StartsWith("expires="));
                if (expiresPart != null && long.TryParse(expiresPart.Split('=')[1], out var count))
                {
                    expires += count;
                }
            }
        }

        return expires;
    }

    private long CalculateAvgTtl(Dictionary<string, string> redisInfo)
    {
        long totalTtl = 0;
        long dbCount = 0;

        foreach (var item in redisInfo)
        {
            if (item.Key.StartsWith("db"))
            {
                var ttlPart = item.Value.Split(',').FirstOrDefault(x => x.StartsWith("avg_ttl="));
                if (ttlPart != null && long.TryParse(ttlPart.Split('=')[1], out var ttl) && ttl > 0)
                {
                    totalTtl += ttl;
                    dbCount++;
                }
            }
        }

        return dbCount > 0 ? totalTtl / dbCount : -1;
    }

    private DateTime? GetBuildTime(Assembly assembly)
    {
        try
        {
            var attribute = assembly.GetCustomAttribute<AssemblyMetadataAttribute>();
            if (attribute != null && attribute.Key == "BuildTimestamp")
            {
                if (DateTime.TryParse(attribute.Value, out var buildTime))
                {
                    return buildTime;
                }
            }

            var fileInfo = new FileInfo(assembly.Location);
            if (fileInfo.Exists)
            {
                return fileInfo.LastWriteTime;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}
