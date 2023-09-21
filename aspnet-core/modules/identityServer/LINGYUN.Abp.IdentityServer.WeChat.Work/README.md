# LINGYUN.Abp.IdentityServer.WeChat.Work

企业微信扩展登录集成


## 配置使用

```csharp
[DependsOn(typeof(AbpIdentityServerWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

```shell

curl -X POST "http://127.0.0.1:44385/connect/token" \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'grant_type=wx-work' \
--data-urlencode 'client_id=你的客户端标识' \
--data-urlencode 'client_secret=你的客户端密钥' \
--data-urlencode 'agent_id=你的企业微信应用标识' \
--data-urlencode 'code=用户扫描登录二维码后重定向页面携带的code标识, 换取用户信息的关键' \
```
