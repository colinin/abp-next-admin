using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.SettingManagement
{
    public class SettingDto
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<SettingDetailsDto> Details { get; set; } = new List<SettingDetailsDto>();

        

        public SettingDto()
        {

        }

        public SettingDto(string displayName, string description = "")
        {
            DisplayName = displayName;
            Description = description;
        }

#nullable enable
        public SettingDetailsDto? AddDetail(
            SettingDefinition setting, 
            IStringLocalizerFactory factory, 
            string value, 
            ValueType type,
            string keepProvider = "")
        {
            if (setting.Providers.Any() &&
                !keepProvider.IsNullOrWhiteSpace() &&
                !setting.Providers.Any(p => p.Equals(keepProvider)))
            {
                return null;
            }
            var detail = new SettingDetailsDto()
            {
                DefaultValue = setting.DefaultValue,
                IsEncrypted = setting.IsEncrypted,
                Description = setting.Description.Localize(factory),
                DisplayName = setting.DisplayName.Localize(factory),
                Name = setting.Name,
                Value = value,
                ValueType = type
            };
            detail.Providers.AddRange(setting.Providers);
            Details.Add(detail);

            return detail;
        }
    }
#nullable disable
}
