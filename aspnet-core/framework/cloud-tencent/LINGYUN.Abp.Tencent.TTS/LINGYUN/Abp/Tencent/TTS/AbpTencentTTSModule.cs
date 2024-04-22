using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Tencent.TTS;

[DependsOn(
    typeof(AbpTencentCloudModule))]
public class AbpTencentTTSModule : AbpModule
{
    //TencentCloud.Tts.V20190823.TtsClient
}
