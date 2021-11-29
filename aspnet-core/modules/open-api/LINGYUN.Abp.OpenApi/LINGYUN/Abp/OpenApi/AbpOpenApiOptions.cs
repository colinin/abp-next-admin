using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINGYUN.Abp.OpenApi
{
    public class AbpOpenApiOptions
    {
        public bool IsEnabled { get; set; }
        public string[] WhiteIpAddress { get; set; }
        public string[] WhiteClient { get; set; }
        public AbpOpenApiOptions()
        {
            IsEnabled = true;
            WhiteIpAddress = new string[0];
            WhiteClient = new string[0];
        }

        public bool HasWhiteIpAddress(string ipAddress)
        {
            return WhiteIpAddress?.Contains(ipAddress) == true;
        }

        public bool HasWhiteClient(string clientId)
        {
            if (clientId.IsNullOrWhiteSpace())
            {
                return false;
            }
            return WhiteClient?.Contains(clientId) == true;
        }
    }
}
