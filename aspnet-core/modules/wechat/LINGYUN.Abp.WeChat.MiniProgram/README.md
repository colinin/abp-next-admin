# LINGYUN.Abp.WeChat.MiniProgram

微信小程序SDK集成,考虑是否需要集成[Senparc.Weixin SDK](https://github.com/JeffreySu/WeiXinMPSDK)

大部分重写的模块都和官方模块名称保持一致,通过命名空间区分,主要是只改写了一小部分或者增加额外的功能
如果大部分模块代码都重写,或者完全就是扩展模块,才会定义自己的名字

#### 注意



## 配置使用


```csharp
[DependsOn(typeof(AbpWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
