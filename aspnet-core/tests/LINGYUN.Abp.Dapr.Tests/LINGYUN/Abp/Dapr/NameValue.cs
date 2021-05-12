using System.Collections.Generic;

namespace LINGYUN.Abp.Dapr
{
    [System.Serializable]
    public class NameValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public NameValue() { }
        public NameValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
