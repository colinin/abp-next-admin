using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Volo.Abp;

namespace LINGYUN.Platform.Datas;

public class DataItemMappingOptions
{
    public Dictionary<ValueType, Func<object, string>> DataItemMaps { get; }

    public DataItemMappingOptions()
    {
        DataItemMaps = new Dictionary<ValueType, Func<object, string>>();
    }

    internal void SetDefaultMapping()
    {
        SetMapping(ValueType.Array, value =>
        {
            if (value == null)
            {
                return "";
            }

            if (value is Array array)
            {
                var joinString = string.Empty;
                foreach (var obj in array)
                {
                    joinString += obj.ToString() + ",";
                }
                return joinString.EndsWith(",") ? joinString[..^1] : joinString;
            }

            if (value is JsonArray jsonArray)
            {
                var joinString = string.Empty;
                foreach (var node in jsonArray)
                {
                    joinString += node.ToString() + ",";
                }
                return joinString.EndsWith(",") ? joinString[..^1] : joinString;
            }

            if (value is JArray jArray)
            {
                var joinString = string.Empty;
                foreach (var token in jArray.Children())
                {
                    joinString += token.ToString() + ",";
                }
                return joinString.EndsWith(",") ? joinString[..^1] : joinString;
            }
            throw new BusinessException(PlatformErrorCodes.MetaFormatMissMatch);
        });
        SetMapping(ValueType.Boolean, value =>
        {
            if (value != null)
            {
                if (value is bool bo)
                {
                    return bo.ToString().ToLower();
                }
                else
                {
                    var boInput = value.ToString().ToLower();
                    if (boInput == "true" ||
                        boInput == "false")
                    {
                        return boInput;
                    } 
                }
            }
            throw new BusinessException(PlatformErrorCodes.MetaFormatMissMatch);
        });
        SetMapping(ValueType.Date, value =>
        {
            if (value != null && value is DateTime date)
            {
                return date.ToString("yyyy-MM-dd");
            }
            throw new BusinessException(PlatformErrorCodes.MetaFormatMissMatch);
        });
        SetMapping(ValueType.DateTime, value =>
        {
            if (value != null && value is DateTime date)
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            throw new BusinessException(PlatformErrorCodes.MetaFormatMissMatch);
        });
        SetMapping(ValueType.Numeic, value =>
        {
            if (value != null)
            {
                var valueType = value.GetType();
                if (!valueType.IsClass && !valueType.IsInterface && typeof(IFormattable).IsAssignableFrom(valueType))
                {
                    return value.ToString();
                }
            }
            throw new BusinessException(PlatformErrorCodes.MetaFormatMissMatch);
        });
        SetMapping(ValueType.String, value =>
        {
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        });
        SetMapping(ValueType.Object, value =>
        {
            if (value == null)
            {
                return "{}";
            }

            return JsonConvert.SerializeObject(value);
        });
    }

    public void SetMapping(ValueType valueType, Func<object, string> func)
    {
        DataItemMaps[valueType] = func;
    }

    public string MapToString(ValueType valueType, object inputValue)
    {
        return DataItemMaps[valueType](inputValue);
    }
}
