using System;
using System.Buffers;
using System.IO;

namespace IP2Region.Net.Internal;
internal class StreamCacheStrategy(Stream _xdbStream) : ICacheStrategy
{
    protected const int HeaderInfoLength = 256;
    protected const int VectorIndexSize = 8;

    protected const int BufferSize = 64 * 1024;

    public int IoCount { get; set; }

    public void ResetIoCount()
    {
        IoCount = 0;
    }

    public virtual ReadOnlyMemory<byte> GetVectorIndex(int offset) => GetData(HeaderInfoLength + offset, VectorIndexSize);

    public virtual ReadOnlyMemory<byte> GetData(long offset, int length)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            int totalBytesRead = 0;
            _xdbStream.Seek(offset, SeekOrigin.Begin);

            int bytesRead;
            while (totalBytesRead < length)
            {
                bytesRead = _xdbStream.Read(buffer, totalBytesRead, length - totalBytesRead);
                if (bytesRead == 0)
                {
                    break;
                }

                totalBytesRead += bytesRead;
                IoCount++;
            }

            var ret = new byte[totalBytesRead];
            if (totalBytesRead > 0)
            {
                Array.Copy(buffer, 0, ret, 0, totalBytesRead);
            }
            return ret;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// 释放文件句柄
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _xdbStream.Close();
            _xdbStream.Dispose();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
