# LINGYUN.Abp.Dapr.Client.Wrapper

Dapr服务间调用，对包装后的响应结果解包  

## 功能特性

* 自动响应结果包装/解包
* 与ABP包装系统集成
* 自定义包装响应的错误处理
* 支持成功/错误代码配置
* HTTP状态码映射
* 支持自定义响应包装格式
* 灵活的包装控制选项

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class AbpDaprClientWrapperModule : AbpModule
{
}
```

## 配置选项

模块使用来自`LINGYUN.Abp.Wrapper`包的`AbpWrapperOptions`进行配置：

```json
{
    "Wrapper": {
        "IsEnabled": true,  // 启用/禁用响应包装
        "CodeWithSuccess": "0",  // 包装响应中的成功代码
        "HttpStatusCode": 200,  // 包装响应的默认HTTP状态码
        "WrapOnError": true,  // 是否包装错误响应
        "WrapOnSuccess": true  // 是否包装成功响应
    }
}
```

## 实现示例

1. 服务定义

```csharp
public interface IProductService
{
    Task<ProductDto> GetAsync(string id);
    Task<List<ProductDto>> GetListAsync();
    Task<ProductDto> CreateAsync(CreateProductDto input);
}
```

2. 服务实现

```csharp
public class ProductService : IProductService
{
    private readonly DaprClient _daprClient;

    public ProductService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<ProductDto> GetAsync(string id)
    {
        // 调用会自动处理响应包装
        return await _daprClient.InvokeMethodAsync<ProductDto>(
            "product-service",  // 目标服务ID
            $"api/products/{id}",  // 方法路径
            HttpMethod.Get
        );
    }

    public async Task<List<ProductDto>> GetListAsync()
    {
        return await _daprClient.InvokeMethodAsync<List<ProductDto>>(
            "product-service", 
            "api/products",
            HttpMethod.Get
        );
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto input)
    {
        return await _daprClient.InvokeMethodAsync<ProductDto>(
            "product-service",
            "api/products",
            HttpMethod.Post,
            input
        );
    }
}
```

## 响应格式

当启用包装时，响应将采用以下格式：

```json
{
    "code": "0",  // 响应代码，默认"0"表示成功
    "message": "Success",  // 响应消息
    "details": null,  // 附加详情（可选）
    "result": {  // 实际响应数据
        // ... 响应内容
    }
}
```

## 错误处理

模块自动处理错误响应：
* 对于包装的响应（带有`AbpWrapResult`头）：
  * 解包响应并检查代码
  * 如果代码与`CodeWithSuccess`不匹配，抛出`AbpRemoteCallException`
  * 在异常中包含错误消息、详情和代码
  * 支持自定义错误代码映射
* 对于未包装的响应：
  * 传递原始响应
  * 使用标准HTTP错误处理
  * 保持原始错误信息

### 错误响应示例

```json
{
    "code": "ERROR_001",
    "message": "产品未找到",
    "details": "ID为'123'的产品不存在",
    "result": null
}
```

## 高级用法

### 1. 控制响应包装

可以通过HTTP头控制单个请求的响应包装：

```csharp
// 在请求头中添加
var headers = new Dictionary<string, string>
{
    { "X-Abp-Wrap-Result", "true" },  // 强制启用包装
    // 或
    { "X-Abp-Dont-Wrap-Result", "true" }  // 强制禁用包装
};

await _daprClient.InvokeMethodAsync<ProductDto>(
    "product-service",
    "api/products",
    HttpMethod.Get,
    null,
    headers
);
```

### 2. 自定义错误处理

```csharp
public class CustomErrorHandler : IAbpWrapperErrorHandler
{
    public Task HandleAsync(AbpWrapperErrorContext context)
    {
        // 自定义错误处理逻辑
        if (context.Response.Code == "CUSTOM_ERROR")
        {
            // 特殊处理
        }
        return Task.CompletedTask;
    }
}
```

## 注意事项

* 响应包装可以通过以下方式控制：
  * 配置文件中的全局设置
  * HTTP头控制单个请求
  * 代码中的动态控制
* 错误响应尽可能保持原始错误结构
* 模块与ABP的远程服务错误处理系统集成
* 建议在微服务架构中统一使用响应包装
* 包装格式可以通过继承`IAbpWrapperResponseBuilder`自定义

[查看英文](README.EN.md)
