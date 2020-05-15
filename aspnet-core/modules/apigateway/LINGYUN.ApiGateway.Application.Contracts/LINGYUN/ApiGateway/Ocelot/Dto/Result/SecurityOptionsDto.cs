using System;
using System.Collections.Generic;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class SecurityOptionsDto
    {
        public List<string> IPAllowedList { get; set; }

        public List<string> IPBlockedList { get; set; }
        public SecurityOptionsDto()
        {
            IPAllowedList = new List<string>();
            IPBlockedList = new List<string>();
        }
    }
}