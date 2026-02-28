using System;
using System.Diagnostics;

namespace LINGYUN.Abp.SystemInfo.Utils;

public class MemoryMetrics
{
    /// <summary>
    /// 总物理内存
    /// </summary>
    public long TotalMemory { get; set; }
    /// <summary>
    /// 工作集内存
    /// </summary>
    public long WorkingSet { get; set; }
    /// <summary>
    /// 私有内存
    /// </summary>
    public long PrivateMemory { get; set; }
    /// <summary>
    /// 虚拟内存
    /// </summary>
    public long VirtualMemory { get; set; }
    /// <summary>
    /// GC堆大小
    /// </summary>
    public long GCHeapSize { get; set; }
}

public class MemoryMonitor
{
    // 转换为MB
    const long MB = 1024 * 1024;

    public MemoryMetrics GetMemoryMetrics()
    {
        var process = Process.GetCurrentProcess();

        return new MemoryMetrics
        {
            TotalMemory = GC.GetTotalMemory(false) / MB,  
            WorkingSet = process.WorkingSet64 / MB,
            PrivateMemory = process.PrivateMemorySize64 / MB,
            VirtualMemory = process.VirtualMemorySize64 / MB,
            GCHeapSize = GetGCHeapSize() / MB
        };
    }

    private long GetGCHeapSize()
    {
        var gcInfo = GC.GetGCMemoryInfo();
        return (long)gcInfo.HeapSizeBytes;
    }
}
