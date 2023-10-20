using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using TencentCloud.Tts.V20190823;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.TTS;
public class TencentCloudTTSClientFactory : TencentCloudClientFactory<TtsClient>
{
    public TencentCloudTTSClientFactory(
        IMemoryCache clientCache,
        ISettingProvider settingProvider)
        : base(clientCache, settingProvider)
    {
    }
}
