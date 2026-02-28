// Copyright 2025 The Ip2Region Authors. All rights reserved.
// Use of this source code is governed by a Apache2.0-style
// license that can be found in the LICENSE file.
// @Author Alan <lzh.shap@gmail.com>
// @Date   2023/07/25
// Updated by Argo Zhang <argo@live.ca> at 2025/11/21

using IP2Region.Net.XDB;
using System.IO;

namespace IP2Region.Net.Internal;

static class CacheStrategyFactory
{
    public static ICacheStrategy CreateCacheStrategy(CachePolicy cachePolicy, Stream xdbStream) => cachePolicy switch
    {
        CachePolicy.Content => new ContentCacheStrategy(xdbStream),
        CachePolicy.VectorIndex => new VectorIndexCacheStrategy(xdbStream),
        _ => new StreamCacheStrategy(xdbStream),
    };
}