using LINYUN.Abp.Location.Amap.Localization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Location.Amap
{
    public abstract class AmapHttpResponse
    {
        /// <summary>
        /// 返回结果状态值
        /// </summary>
        /// <remarks>
        /// 返回值为 0 或 1，0 表示请求失败；1 表示请求成功。
        /// </remarks>
        public int Status { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        /// <remarks>
        /// 当 status 为 0 时，info 会返回具体错误原因，否则返回“OK”。
        /// </remarks>
        public string Info { get; set; }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public string InfoCode { get; set; }

        public bool IsSuccess()
        {
            return Status == 1;
        }

        public ILocalizableString GetErrorMessage()
        {
            switch (Info)
            {
                case "OK":
                    return LocalizableString.Create<AmapLocationResource>("OK");
                case "INVALID_USER_KEY":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_USER_KEY");
                case "SERVICE_NOT_AVAILABLE":
                    return LocalizableString.Create<AmapLocationResource>("SERVICE_NOT_AVAILABLE");
                case "DAILY_QUERY_OVER_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("DAILY_QUERY_OVER_LIMIT");
                case "ACCESS_TOO_FREQUENT":
                    return LocalizableString.Create<AmapLocationResource>("ACCESS_TOO_FREQUENT");
                case "INVALID_USER_IP":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_USER_IP");
                case "INVALID_USER_DOMAIN":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_USER_DOMAIN");
                case "INVALID_USER_SIGNATURE":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_USER_SIGNATURE");
                case "INVALID_USER_SCODE":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_USER_SCODE");
                case "USERKEY_PLAT_NOMATCH":
                    return LocalizableString.Create<AmapLocationResource>("USERKEY_PLAT_NOMATCH");
                case "IP_QUERY_OVER_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("IP_QUERY_OVER_LIMIT");
                case "NOT_SUPPORT_HTTPS":
                    return LocalizableString.Create<AmapLocationResource>("NOT_SUPPORT_HTTPS");
                case "INSUFFICIENT_PRIVILEGES":
                    return LocalizableString.Create<AmapLocationResource>("INSUFFICIENT_PRIVILEGES");
                case "USER_KEY_RECYCLED":
                    return LocalizableString.Create<AmapLocationResource>("USER_KEY_RECYCLED");
                case "QPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("QPS_HAS_EXCEEDED_THE_LIMIT");
                case "GATEWAY_TIMEOUT":
                    return LocalizableString.Create<AmapLocationResource>("GATEWAY_TIMEOUT");
                case "SERVER_IS_BUSY":
                    return LocalizableString.Create<AmapLocationResource>("SERVER_IS_BUSY");
                case "RESOURCE_UNAVAILABLE":
                    return LocalizableString.Create<AmapLocationResource>("RESOURCE_UNAVAILABLE");
                case "CQPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("CQPS_HAS_EXCEEDED_THE_LIMIT");
                case "CKQPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("CKQPS_HAS_EXCEEDED_THE_LIMIT");
                case "CIQPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("CIQPS_HAS_EXCEEDED_THE_LIMIT");
                case "CIKQPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("CIKQPS_HAS_EXCEEDED_THE_LIMIT");
                case "KQPS_HAS_EXCEEDED_THE_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("KQPS_HAS_EXCEEDED_THE_LIMIT");
                case "ABROAD_DAILY_QUERY_OVER_LIMIT":
                    return LocalizableString.Create<AmapLocationResource>("ABROAD_DAILY_QUERY_OVER_LIMIT");
                case "INVALID_PARAMS":
                    return LocalizableString.Create<AmapLocationResource>("INVALID_PARAMS");
                case "MISSING_REQUIRED_PARAMS":
                    return LocalizableString.Create<AmapLocationResource>("MISSING_REQUIRED_PARAMS");
                case "ILLEGAL_REQUEST":
                    return LocalizableString.Create<AmapLocationResource>("ILLEGAL_REQUEST");
                case "UNKNOWN_ERROR":
                    return LocalizableString.Create<AmapLocationResource>("UNKNOWN_ERROR");
                case "INSUFFICIENT_ABROAD_PRIVILEGES":
                    return LocalizableString.Create<AmapLocationResource>("INSUFFICIENT_ABROAD_PRIVILEGES");
                case "ILLEGAL_CONTENT":
                    return LocalizableString.Create<AmapLocationResource>("ILLEGAL_CONTENT");
                case "OUT_OF_SERVICE":
                    return LocalizableString.Create<AmapLocationResource>("OUT_OF_SERVICE");
                case "NO_ROADS_NEARBY":
                    return LocalizableString.Create<AmapLocationResource>("NO_ROADS_NEARBY");
                case "ROUTE_FAIL":
                    return LocalizableString.Create<AmapLocationResource>("ROUTE_FAIL");
                case "OVER_DIRECTION_RANGE":
                    return LocalizableString.Create<AmapLocationResource>("OVER_DIRECTION_RANGE");
                case "ENGINE_RESPONSE_DATA_ERROR":
                    return LocalizableString.Create<AmapLocationResource>("ENGINE_RESPONSE_DATA_ERROR");
                case "QUOTA_PLAN_RUN_OUT":
                    return LocalizableString.Create<AmapLocationResource>("QUOTA_PLAN_RUN_OUT");
                case "GEOFENCE_MAX_COUNT_REACHED":
                    return LocalizableString.Create<AmapLocationResource>("GEOFENCE_MAX_COUNT_REACHED");
                case "SERVICE_EXPIRED":
                    return LocalizableString.Create<AmapLocationResource>("SERVICE_EXPIRED");
                case "ABROAD_QUOTA_PLAN_RUN_OUT":
                    return LocalizableString.Create<AmapLocationResource>("ABROAD_QUOTA_PLAN_RUN_OUT");
                default:
                    throw new AbpException($"{Info} - no error code define!");
            }
        }
    }
}
