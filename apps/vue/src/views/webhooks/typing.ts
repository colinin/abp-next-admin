import { HttpStatusCode } from '/@/enums/httpEnum';

export const httpStatusCodeMap = {
  [HttpStatusCode.Continue]: '100 - Continue',
  [HttpStatusCode.SwitchingProtocols]: '101 - Switching Protocols',
  [HttpStatusCode.OK]: '200 - OK',
  [HttpStatusCode.Created]: '201 - Created',
  [HttpStatusCode.Accepted]: '202 - Accepted',
  [HttpStatusCode.NonAuthoritativeInformation]: '203 - Non Authoritative Information',
  [HttpStatusCode.NoContent]: '204 - No Content',
  [HttpStatusCode.ResetContent]: '205 - Reset Content',
  [HttpStatusCode.PartialContent]: '206 - Partial Content',
  [HttpStatusCode.Ambiguous]: '300 - Ambiguous',
  [HttpStatusCode.MultipleChoices]: '300 - Multiple Choices',
  [HttpStatusCode.Moved]: '301 - Moved',
  [HttpStatusCode.MovedPermanently]: '301 - Moved Permanently',
  [HttpStatusCode.Found]: '302 - Found',
  [HttpStatusCode.Redirect]: '302 - Redirect',
  [HttpStatusCode.RedirectMethod]: '303 - Redirect Method',
  [HttpStatusCode.SeeOther]: '303 - See Other',
  [HttpStatusCode.NotModified]: '304 - Not Modified',
  [HttpStatusCode.UseProxy]: '305 - Use Proxy',
  [HttpStatusCode.Unused]: '306 - Unused',
  [HttpStatusCode.RedirectKeepVerb]: '307 - Redirect Keep Verb',
  [HttpStatusCode.TemporaryRedirect]: '307 - Temporary Redirect',
  [HttpStatusCode.BadRequest]: '400 - Bad Request',
  [HttpStatusCode.Unauthorized]: '401 - Unauthorized',
  [HttpStatusCode.PaymentRequired]: '402 - Payment Required',
  [HttpStatusCode.Forbidden]: '403 - Forbidden',
  [HttpStatusCode.NotFound]: '404 - Not Found',
  [HttpStatusCode.MethodNotAllowed]: '405 - Method Not Allowed',
  [HttpStatusCode.NotAcceptable]: '406 - Not Acceptable',
  [HttpStatusCode.ProxyAuthenticationRequired]: '407 - Proxy Authentication Required',
  [HttpStatusCode.RequestTimeout]: '408 - Request Timeout',
  [HttpStatusCode.Conflict]: '409 - Conflict',
  [HttpStatusCode.Gone]: '410 - Gone',
  [HttpStatusCode.LengthRequired]: '411 - Length Required',
  [HttpStatusCode.PreconditionFailed]: '412 - Precondition Failed',
  [HttpStatusCode.RequestEntityTooLarge]: '413 - Request Entity Too Large',
  [HttpStatusCode.RequestUriTooLong]: '414 - Request Uri Too Long',
  [HttpStatusCode.UnsupportedMediaType]: '415 - Unsupported Media Type',
  [HttpStatusCode.RequestedRangeNotSatisfiable]: '416 - Requested Range Not Satisfiable',
  [HttpStatusCode.ExpectationFailed]: '417 - Expectation Failed',
  [HttpStatusCode.UpgradeRequired]: '426 - Upgrade Required',
  [HttpStatusCode.InternalServerError]: '500 - Internal Server Error',
  [HttpStatusCode.NotImplemented]: '501 - Not Implemented',
  [HttpStatusCode.BadGateway]: '502 - Bad Gateway',
  [HttpStatusCode.ServiceUnavailable]: '503 - Service Unavailable',
  [HttpStatusCode.GatewayTimeout]: '504 - Gateway Timeout',
  [HttpStatusCode.HttpVersionNotSupported]: '505 - Http Version Not Supported',
}

export const httpStatusOptions = Object.keys(httpStatusCodeMap).map((key) => {
  return {
    label: httpStatusCodeMap[key],
    value: key,
  };
})

export function getHttpStatusColor(statusCode: HttpStatusCode) {
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