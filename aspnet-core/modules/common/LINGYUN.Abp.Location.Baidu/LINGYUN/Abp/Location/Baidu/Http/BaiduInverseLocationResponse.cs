using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.Location.Baidu.Http
{
    public class BaiduInverseLocationResponse : BaiduLocationResponse
    {
        [JsonProperty("formatted_address")]
        public string Address { get; set; }


    }
}
