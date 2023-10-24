using LINGYUN.Abp.Location.Baidu.Localization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Location.Baidu.Response
{
    public abstract class BaiduLocationResponse
    {
        public int Status { get; set; }

        public bool IsSuccess()
        {
            return Status == 0;
        }

        public ILocalizableString GetErrorMessage(bool throwToClient = false)
        {
            switch (Status)
            {
                case 0:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_0");
                case 1:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_1");
                case 10:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_10");
                case 101:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_101");
                case 102:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_102");
                case 200:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_200");
                case 201:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_201");
                case 202:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_202");
                case 203:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_203");
                case 210:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_210");
                case 211:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_211");
                case 220:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_220");
                case 230:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_230");
                case 240:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_240");
                case 250:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_250");
                case 251:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_251");
                case 252:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_252");
                case 260:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_260");
                case 261:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_261");
                case 301:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_301");
                case 302:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_302");
                case 401:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_401");
                case 402:
                    return LocalizableString.Create<BaiduLocationResource>("Message:RETURN_402");
                default:
                    if (throwToClient)
                    {
                        throw new LocationResolveException($"{Status} - no error code define!");
                    }
                    throw new AbpException($"{Status} - no error code define!");
            }
        }

        public ILocalizableString GetErrorDescription(bool throwToClient = false)
        {
            switch (Status)
            {
                case 0:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_0");
                case 1:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_1");
                case 10:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_10");
                case 101:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_101");
                case 102:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_102");
                case 200:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_200");
                case 201:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_201");
                case 202:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_202");
                case 203:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_203");
                case 210:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_210");
                case 211:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_211");
                case 220:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_220");
                case 230:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_230");
                case 240:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_240");
                case 250:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_250");
                case 251:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_251");
                case 252:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_252");
                case 260:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_260");
                case 261:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_261");
                case 301:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_301");
                case 302:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_302");
                case 401:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_401");
                case 402:
                    return LocalizableString.Create<BaiduLocationResource>("Description:RETURN_402");
                default:
                    if (throwToClient)
                    {
                        throw new LocationResolveException($"{Status} - no error code define!");
                    }
                    throw new AbpException($"{Status} - no error code define!");
            }
        }
    }
}
