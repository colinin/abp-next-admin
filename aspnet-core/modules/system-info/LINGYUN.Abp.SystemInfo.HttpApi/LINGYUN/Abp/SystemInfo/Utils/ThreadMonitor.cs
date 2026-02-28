using System.Diagnostics;
using System.Threading;

namespace LINGYUN.Abp.SystemInfo.Utils;

public class ThreadMetrics
{
    /// <summary>
    /// 可用工作线程数
    /// </summary>
    public int AvailableWorkerThreads { get; set; }
    /// <summary>
    /// 可用I/O线程数
    /// </summary>
    public int AvailableCompletionPortThreads { get; set; }
    /// <summary>
    /// 最大工作线程数
    /// </summary>
    public int MaxWorkerThreads { get; set; }
    /// <summary>
    /// 最大I/O线程数
    /// </summary>
    public int MaxCompletionPortThreads { get; set; }
    /// <summary>
    /// 进程总线程数
    /// </summary>
    public int ActiveThreads { get; set; }
    /// <summary>
    /// 线程池活动线程数
    /// </summary>
    public int ThreadPoolThreadCount { get; set; }
}

public class ThreadMonitor
{
    public ThreadMetrics GetThreadMetrics()
    {
        ThreadPool.GetAvailableThreads(
            out var availableWorkerThreads,
            out var availableCompletionPortThreads);
        ThreadPool.GetMaxThreads(
            out var maxWorkerThreads,
            out var maxCompletionPortThreads);

        // 获取进程线程数（近似活动线程数）
        var process = Process.GetCurrentProcess();
        var threads = process.Threads;

        return new ThreadMetrics
        {
            AvailableWorkerThreads = availableWorkerThreads,
            AvailableCompletionPortThreads = availableCompletionPortThreads,
            MaxWorkerThreads = maxWorkerThreads,
            MaxCompletionPortThreads = maxCompletionPortThreads,
            ActiveThreads = threads.Count,
            ThreadPoolThreadCount = ThreadPool.ThreadCount
        };
    }
}
