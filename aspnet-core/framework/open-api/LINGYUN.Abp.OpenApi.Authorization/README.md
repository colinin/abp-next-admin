# LINGYUN.Abp.OpenApi.Authorization

OpenApi 认证授权中间件模块，为 ABP 应用程序提供基于 AppKey/AppSecret 的 API 签名认证中间件功能。

## 功能特性

* 提供 OpenApi 认证中间件
* 支持请求签名验证
* 支持防重放攻击（Nonce随机数验证）
* 支持请求时间戳验证
* 支持客户端白名单验证
* 支持IP地址白名单验证
* 支持自定义认证逻辑
* 支持异常处理和错误信息包装

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenApi.Authorization
```

## 模块引用

```csharp
[DependsOn(typeof(AbpOpenApiAuthorizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 启用 OpenApi 认证中间件
   ```csharp
   public void Configure(IApplicationBuilder app)
   {
       // 添加 OpenApi 认证中间件
       app.UseOpenApiAuthorization();
   }
   ```

2. 自定义认证服务（可选）
   ```csharp
   public class CustomOpenApiAuthorizationService : OpenApiAuthorizationService
   {
       public CustomOpenApiAuthorizationService(
           INonceStore nonceStore,
           IAppKeyStore appKeyStore,
           IClientChecker clientChecker,
           IIpAddressChecker ipAddressChecker,
           IWebClientInfoProvider clientInfoProvider,
           IOptionsMonitor<AbpOpenApiOptions> options,
           IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
           : base(nonceStore, appKeyStore, clientChecker, ipAddressChecker,
                 clientInfoProvider, options, exceptionHandlingOptions)
       {
       }

       // 重写认证逻辑
       public override async Task<bool> AuthorizeAsync(HttpContext httpContext)
       {
           // 实现自定义认证逻辑
           return await base.AuthorizeAsync(httpContext);
       }
   }
   ```

## 认证流程

1. 验证客户端IP地址
   * 通过 `IIpAddressChecker` 接口验证客户端IP地址是否在允许范围内

2. 验证应用凭证
   * 验证请求头中的 AppKey、签名、随机数和时间戳
   * 通过 `IAppKeyStore` 接口获取应用信息
   * 通过 `IClientChecker` 接口验证客户端是否允许访问
   * 验证签名的有效性和时效性

## 签名规则

1. 请求参数按照参数名ASCII码从小到大排序
2. 使用URL编码（UTF-8）将排序后的参数转换为查询字符串
3. 将请求路径和查询字符串拼接后进行MD5加密得到签名

示例：
```
请求路径：/api/test
参数：
  appKey=test
  appSecret=123456
  nonce=abc
  t=1577808000000

签名计算：
1. 参数排序并拼接：appKey=test&appSecret=123456&nonce=abc&t=1577808000000
2. 拼接请求路径：/api/test?appKey=test&appSecret=123456&nonce=abc&t=1577808000000
3. URL编码并MD5加密得到最终签名
```

## 更多信息

* [LINGYUN.Abp.OpenApi](../LINGYUN.Abp.OpenApi/README.md)
* [ABP框架](https://abp.io/)
