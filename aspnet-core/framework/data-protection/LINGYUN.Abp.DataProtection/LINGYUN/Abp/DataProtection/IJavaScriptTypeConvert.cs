using System;

namespace LINGYUN.Abp.DataProtection;

public interface IJavaScriptTypeConvert
{
    JavaScriptTypeConvertResult Convert(Type propertyType);
}
