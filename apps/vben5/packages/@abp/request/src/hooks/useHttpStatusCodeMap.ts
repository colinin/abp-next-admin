import { HttpStatusCode } from '../constants/httpStatus';

export function useHttpStatusCodeMap() {
  const httpStatusCodeMap: { [key: number]: string } = {
    [HttpStatusCode.Accepted]: '202 - Accepted',
    [HttpStatusCode.Ambiguous]: '300 - Ambiguous/Multiple Choices',
    [HttpStatusCode.BadGateway]: '502 - Bad Gateway',
    [HttpStatusCode.BadRequest]: '400 - Bad Request',
    [HttpStatusCode.Conflict]: '409 - Conflict',
    [HttpStatusCode.Continue]: '100 - Continue',
    [HttpStatusCode.Created]: '201 - Created',
    [HttpStatusCode.ExpectationFailed]: '417 - Expectation Failed',
    [HttpStatusCode.Forbidden]: '403 - Forbidden',
    [HttpStatusCode.GatewayTimeout]: '504 - Gateway Timeout',
    [HttpStatusCode.Gone]: '410 - Gone',
    [HttpStatusCode.HttpVersionNotSupported]:
      '505 - Http Version Not Supported',
    [HttpStatusCode.InternalServerError]: '500 - Internal Server Error',
    [HttpStatusCode.LengthRequired]: '411 - Length Required',
    [HttpStatusCode.MethodNotAllowed]: '405 - Method Not Allowed',
    [HttpStatusCode.Moved]: '301 - Moved/Moved Permanently',
    [HttpStatusCode.NoContent]: '204 - No Content',
    [HttpStatusCode.NonAuthoritativeInformation]:
      '203 - Non Authoritative Information',
    [HttpStatusCode.NotAcceptable]: '406 - Not Acceptable',
    [HttpStatusCode.NotFound]: '404 - Not Found',
    [HttpStatusCode.NotImplemented]: '501 - Not Implemented',
    [HttpStatusCode.NotModified]: '304 - Not Modified',
    [HttpStatusCode.OK]: '200 - OK',
    [HttpStatusCode.PartialContent]: '206 - Partial Content',
    [HttpStatusCode.PaymentRequired]: '402 - Payment Required',
    [HttpStatusCode.PreconditionFailed]: '412 - Precondition Failed',
    [HttpStatusCode.ProxyAuthenticationRequired]:
      '407 - Proxy Authentication Required',
    [HttpStatusCode.Redirect]: '302 - Found/Redirect',
    [HttpStatusCode.RedirectKeepVerb]:
      '307 - Redirect Keep Verb/Temporary Redirect',
    [HttpStatusCode.RedirectMethod]: '303 - Redirect Method/See Other',
    [HttpStatusCode.RequestedRangeNotSatisfiable]:
      '416 - Requested Range Not Satisfiable',
    [HttpStatusCode.RequestEntityTooLarge]: '413 - Request Entity Too Large',
    [HttpStatusCode.RequestTimeout]: '408 - Request Timeout',
    [HttpStatusCode.RequestUriTooLong]: '414 - Request Uri Too Long',
    [HttpStatusCode.ResetContent]: '205 - Reset Content',
    [HttpStatusCode.ServiceUnavailable]: '503 - Service Unavailable',
    [HttpStatusCode.SwitchingProtocols]: '101 - Switching Protocols',
    [HttpStatusCode.Unauthorized]: '401 - Unauthorized',
    [HttpStatusCode.UnsupportedMediaType]: '415 - Unsupported Media Type',
    [HttpStatusCode.Unused]: '306 - Unused',
    [HttpStatusCode.UpgradeRequired]: '426 - Upgrade Required',
    [HttpStatusCode.UseProxy]: '305 - Use Proxy',
  };

  function getHttpStatusColor(statusCode: HttpStatusCode) {
    if (statusCode < 200) {
      return 'default';
    }
    if (statusCode >= 200 && statusCode < 300) {
      return 'success';
    }
    if (statusCode >= 300 && statusCode < 400) {
      return 'processing';
    }
    if (statusCode >= 400 && statusCode < 500) {
      return 'warning';
    }
    if (statusCode >= 500) {
      return 'error';
    }
  }

  return {
    getHttpStatusColor,
    httpStatusCodeMap,
  };
}
