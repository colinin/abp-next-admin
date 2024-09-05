// Copyright 2023 The Ip2Region Authors. All rights reserved.
// Use of this source code is governed by a Apache2.0-style
// license that can be found in the LICENSE file.
// @Author Alan <lzh.shap@gmail.com>
// @Date   2023/07/25

using IP2Region.Net.Internal.Abstractions;
using System;
using System.IO;

namespace IP2Region.Net.Internal;

internal class ContentCacheStrategy : AbstractCacheStrategy
{
    private readonly ReadOnlyMemory<byte> _cacheData;

    public ContentCacheStrategy(Stream xdbStream) : base(xdbStream)
    {
        _cacheData = base.GetData(0, (int)XdbStream.Length);
        XdbStream.Close();
        XdbStream.Dispose();
    }

    internal override ReadOnlyMemory<byte> GetVectorIndex(uint ip)
    {
        int idx = GetVectorIndexStartPos(ip);
        return _cacheData.Slice(HeaderInfoLength + idx, VectorIndexSize);
    }

    internal override ReadOnlyMemory<byte> GetData(int offset, int length)
    {
        return _cacheData.Slice(offset, length);
    }
}