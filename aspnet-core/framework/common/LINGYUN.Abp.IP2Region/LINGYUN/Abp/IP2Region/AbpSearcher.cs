using IP2Region.Net.Abstractions;
using IP2Region.Net.Internal;
using IP2Region.Net.Internal.Abstractions;
using IP2Region.Net.XDB;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace LINGYUN.Abp.IP2Region;
public class AbpSearcher : ISearcher
{
    const int SegmentIndexSize = 14;

    private readonly AbstractCacheStrategy _cacheStrategy;
    public int IoCount => _cacheStrategy.IoCount;

    public AbpSearcher(CachePolicy cachePolicy, Stream xdbStream)
    {
        var factory = new CacheStrategyFactory(xdbStream);
        _cacheStrategy = factory.CreateCacheStrategy(cachePolicy);
    }

    public string? Search(string ipStr)
    {
        var ip = Util.IpAddressToUInt32(ipStr);
        return Search(ip);
    }

    public string? Search(IPAddress ipAddress)
    {
        var ip = Util.IpAddressToUInt32(ipAddress);
        return Search(ip);
    }

    public string? Search(uint ip)
    {
        var index = _cacheStrategy.GetVectorIndex(ip);
        uint sPtr = MemoryMarshal.Read<uint>(index.Span);
        uint ePtr = MemoryMarshal.Read<uint>(index.Span.Slice(4));

        var dataLen = 0;
        uint dataPtr = 0;
        uint l = 0;
        uint h = (ePtr - sPtr) / SegmentIndexSize;

        while (l <= h)
        {
            var mid = Util.GetMidIp(l, h);
            var pos = sPtr + mid * SegmentIndexSize;

            var buffer = _cacheStrategy.GetData((int)pos, SegmentIndexSize);
            uint sip = MemoryMarshal.Read<uint>(buffer.Span);
            uint eip = MemoryMarshal.Read<uint>(buffer.Span.Slice(4));

            if (ip < sip)
            {
                h = mid - 1;
            }
            else if (ip > eip)
            {
                l = mid + 1;
            }
            else
            {
                dataLen = MemoryMarshal.Read<ushort>(buffer.Span.Slice(8));
                dataPtr = MemoryMarshal.Read<uint>(buffer.Span.Slice(10));
                break;
            }
        }

        if (dataLen == 0)
        {
            return default;
        }

        var regionBuff = _cacheStrategy.GetData((int)dataPtr, dataLen);
        return Encoding.UTF8.GetString(regionBuff.Span.ToArray());
    }
}
