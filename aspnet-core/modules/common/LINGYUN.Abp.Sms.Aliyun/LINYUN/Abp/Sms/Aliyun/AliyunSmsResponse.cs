using LINYUN.Abp.Sms.Aliyun.Localization;
using System;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string RequestId { get; set; }

        public bool IsSuccess()
        {
            return "ok".Equals(Code, StringComparison.CurrentCultureIgnoreCase);
        }

        public static ILocalizableString GetErrorMessage(string code, string message)
        {
            // TODO: 把前缀写入本地化文档里面?
            Check.NotNullOrWhiteSpace(code, nameof(code));
            switch (code)
            {
                case "isv.SMS_SIGNATURE_SCENE_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SMS_SIGNATURE_SCENE_ILLEGAL");
                case "isv.DENY_IP_RANGE":
                    return LocalizableString.Create<AliyunSmsResource>("DENY_IP_RANGE");
                case "isv.MOBILE_COUNT_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("MOBILE_COUNT_OVER_LIMIT");
                case "isv.BUSINESS_LIMIT_CONTROL":
                    return LocalizableString.Create<AliyunSmsResource>("BUSINESS_LIMIT_CONTROL");
                case "SignatureDoesNotMatch":
                    return LocalizableString.Create<AliyunSmsResource>("SignatureDoesNotMatch");
                case "InvalidTimeStamp.Expired":
                    return LocalizableString.Create<AliyunSmsResource>("InvalidTimeStampExpired");
                case "SignatureNonceUsed":
                    return LocalizableString.Create<AliyunSmsResource>("SignatureNonceUsed");
                case "InvalidVersion":
                    return LocalizableString.Create<AliyunSmsResource>("InvalidVersion");
                case "InvalidAction.NotFound":
                    return LocalizableString.Create<AliyunSmsResource>("InvalidActionNotFound");
                case "isv.SIGN_COUNT_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("SIGN_COUNT_OVER_LIMIT");
                case "isv.TEMPLATE_COUNT_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("TEMPLATE_COUNT_OVER_LIMIT");
                case "isv.SIGN_NAME_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SIGN_NAME_ILLEGAL");
                case "isv.SIGN_FILE_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("SIGN_FILE_LIMIT");
                case "isv.SIGN_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("SIGN_OVER_LIMIT");
                case "isv.TEMPLATE_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("TEMPLATE_OVER_LIMIT");
                case "SIGNATURE_BLACKLIST":
                    return LocalizableString.Create<AliyunSmsResource>("SIGNATURE_BLACKLIST");
                case "isv.SHORTURL_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("SHORTURL_OVER_LIMIT");
                case "isv.NO_AVAILABLE_SHORT_URL":
                    return LocalizableString.Create<AliyunSmsResource>("NO_AVAILABLE_SHORT_URL");
                case "isv.SHORTURL_NAME_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SHORTURL_NAME_ILLEGAL");
                case "isv.SOURCEURL_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("SOURCEURL_OVER_LIMIT");
                case "isv.SHORTURL_TIME_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SHORTURL_TIME_ILLEGAL");
                case "isv.PHONENUMBERS_OVER_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("PHONENUMBERS_OVER_LIMIT");
                case "isv.SHORTURL_STILL_AVAILABLE":
                    return LocalizableString.Create<AliyunSmsResource>("SHORTURL_STILL_AVAILABLE");
                case "isv.SHORTURL_NOT_FOUND":
                    return LocalizableString.Create<AliyunSmsResource>("SHORTURL_NOT_FOUND");
                case "isv.SMS_TEMPLATE_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SMS_TEMPLATE_ILLEGAL");
                case "isv.SMS_SIGNATURE_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SMS_SIGNATURE_ILLEGAL");
                case "isv.MOBILE_NUMBER_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("MOBILE_NUMBER_ILLEGAL");
                case "isv.TEMPLATE_MISSING_PARAMETERS":
                    return LocalizableString.Create<AliyunSmsResource>("TEMPLATE_MISSING_PARAMETERS");
                case "isv.EXTEND_CODE_ERROR":
                    return LocalizableString.Create<AliyunSmsResource>("EXTEND_CODE_ERROR");
                case "isv.DOMESTIC_NUMBER_NOT_SUPPORTED":
                    return LocalizableString.Create<AliyunSmsResource>("DOMESTIC_NUMBER_NOT_SUPPORTED");
                case "isv.DAY_LIMIT_CONTROL":
                    return LocalizableString.Create<AliyunSmsResource>("DAY_LIMIT_CONTROL");
                case "isv.SMS_CONTENT_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SMS_CONTENT_ILLEGAL");
                case "isv.SMS_SIGN_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("SMS_SIGN_ILLEGAL");
                case "isp.RAM_PERMISSION_DENY":
                    return LocalizableString.Create<AliyunSmsResource>("RAM_PERMISSION_DENY");
                case "isp.OUT_OF_SERVICE":
                    return LocalizableString.Create<AliyunSmsResource>("OUT_OF_SERVICE");
                case "isv.PRODUCT_UN_SUBSCRIPT":
                    return LocalizableString.Create<AliyunSmsResource>("PRODUCT_UN_SUBSCRIPT");
                case "isv.PRODUCT_UNSUBSCRIBE":
                    return LocalizableString.Create<AliyunSmsResource>("PRODUCT_UNSUBSCRIBE");
                case "isv.ACCOUNT_NOT_EXISTS":
                    return LocalizableString.Create<AliyunSmsResource>("ACCOUNT_NOT_EXISTS");
                case "isv.ACCOUNT_ABNORMAL":
                    return LocalizableString.Create<AliyunSmsResource>("ACCOUNT_ABNORMAL");
                case "isv.INVALID_PARAMETERS":
                    return LocalizableString.Create<AliyunSmsResource>("INVALID_PARAMETERS");
                case "isv.SYSTEM_ERROR":
                    return LocalizableString.Create<AliyunSmsResource>("SYSTEM_ERROR");
                case "isv.INVALID_JSON_PARAM":
                    return LocalizableString.Create<AliyunSmsResource>("INVALID_JSON_PARAM");
                case "isv.BLACK_KEY_CONTROL_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("BLACK_KEY_CONTROL_LIMIT");
                case "isv.PARAM_LENGTH_LIMIT":
                    return LocalizableString.Create<AliyunSmsResource>("PARAM_LENGTH_LIMIT");
                case "isv.PARAM_NOT_SUPPORT_URL":
                    return LocalizableString.Create<AliyunSmsResource>("PARAM_NOT_SUPPORT_URL");
                case "isv.AMOUNT_NOT_ENOUGH":
                    return LocalizableString.Create<AliyunSmsResource>("AMOUNT_NOT_ENOUGH");
                case "isv.TEMPLATE_PARAMS_ILLEGAL":
                    return LocalizableString.Create<AliyunSmsResource>("TEMPLATE_PARAMS_ILLEGAL");
                default:
                    throw new AbpException($"no error code: {code} define, message: {message}");

            }
        }
    }
}
