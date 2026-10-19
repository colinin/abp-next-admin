using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

/// <summary>
/// 测试用 HTTP 消息处理器
/// </summary>
/// <remarks>
/// 记录请求并按队列返回预设响应, 用于验证接口路径/参数/请求体, 不产生真实网络请求
/// </remarks>
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<FakeHttpResponse> _responses = new Queue<FakeHttpResponse>();

    public List<FakeHttpRequest> Requests { get; } = new List<FakeHttpRequest>();

    public FakeHttpRequest LastRequest => Requests.Count > 0 ? Requests[Requests.Count - 1] : throw new System.InvalidOperationException("No request captured.");

    public FakeHttpMessageHandler Enqueue(HttpStatusCode statusCode, string content, string contentType = "application/json")
    {
        _responses.Enqueue(new FakeHttpResponse(statusCode, content, contentType));

        return this;
    }

    public FakeHttpMessageHandler EnqueueOk(string content)
    {
        return Enqueue(HttpStatusCode.OK, content);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content == null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken);

        Requests.Add(new FakeHttpRequest(
            request.Method,
            request.RequestUri,
            body,
            request.Content?.Headers.ContentType?.MediaType));

        var fakeResponse = _responses.Count > 0
            ? _responses.Dequeue()
            : new FakeHttpResponse(HttpStatusCode.OK, "{}", "application/json");

        var response = new HttpResponseMessage(fakeResponse.StatusCode)
        {
            Content = new StringContent(fakeResponse.Content, Encoding.UTF8),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(fakeResponse.ContentType);

        return response;
    }
}

public class FakeHttpResponse
{
    public HttpStatusCode StatusCode { get; }
    public string Content { get; }
    public string ContentType { get; }

    public FakeHttpResponse(HttpStatusCode statusCode, string content, string contentType)
    {
        StatusCode = statusCode;
        Content = content;
        ContentType = contentType;
    }
}

public class FakeHttpRequest
{
    public HttpMethod Method { get; }
    public System.Uri? RequestUri { get; }
    public string? Body { get; }
    public string? ContentType { get; }

    public FakeHttpRequest(HttpMethod method, System.Uri? requestUri, string? body, string? contentType)
    {
        Method = method;
        RequestUri = requestUri;
        Body = body;
        ContentType = contentType;
    }

    public string PathAndQuery => RequestUri?.PathAndQuery ?? string.Empty;
}
