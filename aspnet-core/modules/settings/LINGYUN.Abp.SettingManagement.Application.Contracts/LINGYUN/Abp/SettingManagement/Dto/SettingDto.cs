using Microsoft.Extensions.Localization;
using System.Collections.Generic;
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

        public SettingDetailsDto AddDetail(SettingDefinition setting, IStringLocalizerFactory factory, string value, ValueType type)
        {
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
            Details.Add(detail);

            return detail;
        }
    }
}
