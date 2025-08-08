using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;

namespace LINGYUN.Abp.LocalizationManagement;
public class LocalizationResourceDictionary : ConcurrentDictionary<string, LocalizationCultureDictionary>
{
}

public class LocalizationCultureDictionary : ConcurrentDictionary<string, LocalizationTextDictionary>
{
}

public class LocalizationTextDictionary : ConcurrentDictionary<string, LocalizedString>
{
}
