// Copyright 2025 The Ip2Region Authors. All rights reserved.
// Use of this source code is governed by a Apache2.0-style
// license that can be found in the LICENSE file.
// @Author Alan <lzh.shap@gmail.com>
// @Date   2023/07/25
// Updated by Argo Zhang <argo@live.ca> at 2025/11/21

using IP2Region.Net.Abstractions;
using IP2Region.Net.Internal;
using IP2Region.Net.XDB;
using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LINGYUN.Abp.IP2Region;

/// <summary>
/// ip2region 离线查询器
/// </summary>
/// <remarks>
/// 支持 IPv4 与 IPv6 两个离线数据文件:
/// * <see cref="AbpSearcher(CachePolicy, Stream)"/>: 只加载一个数据文件, 查询时按地址的字节长度选择比较方式
///   (IPv4 为 4 字节、IPv6 为 16 字节), 即由调用方保证数据文件与查询地址的版本一致;
/// * <see cref="AbpSearcher(CachePolicy, Stream, Stream?)"/>: 同时加载 IPv4 与 IPv6 数据文件, 按地址族自动路由,
///   未提供对应地址族的数据文件时返回 <c>null</c>(避免使用错误的数据文件解析出错误的地理位置)。
/// 
/// IPv4 映射的 IPv6 地址(如 <c>::ffff:1.2.3.4</c>)会归一到 IPv4 查询。
/// </remarks>
public class AbpSearcher : ISearcher
{
    private readonly ICacheStrategy _cacheStrategy;
    private readonly ICacheStrategy? _ipv6CacheStrategy;
    private readonly bool _isDualDatabase;

    /// <summary>
    /// 只加载一个数据文件(IPv4 或 IPv6)
    /// </summary>
    /// <param name="cachePolicy">缓存策略</param>
    /// <param name="xdbStream">数据文件流</param>
    public AbpSearcher(CachePolicy cachePolicy, Stream xdbStream)
        : this(cachePolicy, xdbStream, null, false)
    {
    }

    /// <summary>
    /// 同时加载 IPv4 与 IPv6 数据文件, 按地址族自动路由
    /// </summary>
    /// <param name="cachePolicy">缓存策略</param>
    /// <param name="xdbStream">IPv4 数据文件流</param>
    /// <param name="ipv6XdbStream">IPv6 数据文件流, 未提供时 IPv6 查询返回 <c>null</c></param>
    public AbpSearcher(CachePolicy cachePolicy, Stream xdbStream, Stream? ipv6XdbStream)
        : this(cachePolicy, xdbStream, ipv6XdbStream, true)
    {
    }

    private AbpSearcher(CachePolicy cachePolicy, Stream xdbStream, Stream? ipv6XdbStream, bool isDualDatabase)
    {
        _cacheStrategy = CacheStrategyFactory.CreateCacheStrategy(cachePolicy, xdbStream);

        _isDualDatabase = isDualDatabase;
        if (ipv6XdbStream != null)
        {
            _ipv6CacheStrategy = CacheStrategyFactory.CreateCacheStrategy(cachePolicy, ipv6XdbStream);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public int IoCount => _cacheStrategy.IoCount + (_ipv6CacheStrategy?.IoCount ?? 0);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Search(string ipStr) => Search(IPAddress.Parse(ipStr));

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Search(IPAddress ipAddress)
    {
        // IPv4 映射的 IPv6 地址归一到 IPv4 查询
        if (ipAddress.IsIPv4MappedToIPv6)
        {
            ipAddress = ipAddress.MapToIPv4();
        }

        var cacheStrategy = ResolveCacheStrategy(ipAddress);
        if (cacheStrategy == null)
        {
            return null;
        }

        return SearchCore(cacheStrategy, ipAddress.GetAddressBytes());
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Obsolete("已弃用，请改用其他方法；Deprecated; please use Search(string) or Search(IPAddress) method.")]
    [ExcludeFromCodeCoverage]
    public string? Search(uint ipAddress)
    {
        var bytes = BitConverter.GetBytes(ipAddress);
        Array.Reverse(bytes);

        return SearchCore(_cacheStrategy, bytes);
    }

    /// <summary>
    /// 按地址族选择数据文件
    /// </summary>
    /// <remarks>
    /// 单文件模式(只加载一个数据文件)下不做路由, 沿用按字节长度比较的行为
    /// </remarks>
    private ICacheStrategy? ResolveCacheStrategy(IPAddress ipAddress)
    {
        if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
        {
            if (_ipv6CacheStrategy != null)
            {
                return _ipv6CacheStrategy;
            }

            // 双文件模式下缺少 IPv6 数据文件, 不使用 IPv4 数据文件解析
            return _isDualDatabase ? null : _cacheStrategy;
        }

        return _cacheStrategy;
    }

    private string? SearchCore(ICacheStrategy cacheStrategy, byte[] ipBytes)
    {
        // 重置 IO 计数器
        cacheStrategy.ResetIoCount();
        _cacheStrategy.ResetIoCount();
        _ipv6CacheStrategy?.ResetIoCount();

        // 每个 vector 索引项的字节数
        var vectorIndexSize = 8;

        // vector 索引的列数
        var vectorIndexCols = 256;

        // 计算得到 vector 索引项的开始地址。
        var il0 = ipBytes[0];
        var il1 = ipBytes[1];
        var idx = il0 * vectorIndexCols * vectorIndexSize + il1 * vectorIndexSize;

        var vector = cacheStrategy.GetVectorIndex(idx);
        var sPtr = BinaryPrimitives.ReadUInt32LittleEndian(vector.Span);
        var ePtr = BinaryPrimitives.ReadUInt32LittleEndian(vector.Span.Slice(4));

        var length = ipBytes.Length;
        var indexSize = length * 2 + 6;
        var l = 0;
        var h = (ePtr - sPtr) / indexSize;
        var dataLen = 0;
        long dataPtr = 0;

        while (l <= h)
        {
            int m = (int)(l + h) >> 1;

            var p = sPtr + m * indexSize;
            var buff = cacheStrategy.GetData(p, indexSize);

            var s = buff.Span.Slice(0, length);
            var e = buff.Span.Slice(length, length);
            if (ByteCompare(ipBytes, s) < 0)
            {
                h = m - 1;
            }
            else if (ByteCompare(ipBytes, e) > 0)
            {
                l = m + 1;
            }
            else
            {
                dataLen = BinaryPrimitives.ReadUInt16LittleEndian(buff.Span.Slice(length * 2, 2));
                dataPtr = BinaryPrimitives.ReadUInt32LittleEndian(buff.Span.Slice(length * 2 + 2, 4));
                break;
            }
        }

        var regionBuff = cacheStrategy.GetData(dataPtr, dataLen);
        return Encoding.UTF8.GetString(regionBuff.Span.ToArray());
    }

    static int ByteCompare(byte[] ip1, ReadOnlySpan<byte> ip2) => ip1.Length == 4 ? IPv4Compare(ip1, ip2) : IPv6Compare(ip1, ip2);

    static int IPv4Compare(byte[] ip1, ReadOnlySpan<byte> ip2)
    {
        var ret = 0;
        for (int i = 0; i < ip1.Length; i++)
        {
            var ip2Index = ip1.Length - 1 - i;
            if (ip1[i] < ip2[ip2Index])
            {
                return -1;
            }
            else if (ip1[i] > ip2[ip2Index])
            {
                return 1;
            }
        }
        return ret;
    }

    static int IPv6Compare(byte[] ip1, ReadOnlySpan<byte> ip2)
    {
        var ret = 0;
        for (int i = 0; i < ip1.Length; i++)
        {
            if (ip1[i] < ip2[i])
            {
                return -1;
            }
            else if (ip1[i] > ip2[i])
            {
                return 1;
            }
        }
        return ret;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        _cacheStrategy.Dispose();
        _ipv6CacheStrategy?.Dispose();
        GC.SuppressFinalize(this);
    }
}
