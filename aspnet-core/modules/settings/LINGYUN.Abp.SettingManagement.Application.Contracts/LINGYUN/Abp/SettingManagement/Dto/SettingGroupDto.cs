using System.Collections.Generic;

namespace LINGYUN.Abp.SettingManagement
{
    public class SettingGroupDto
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<SettingDto> Settings { get; set; } = new List<SettingDto>();

        public SettingGroupDto()
        {

        }

        public SettingGroupDto(string displayName, string description = "")
        {
            DisplayName = displayName;
            Description = description;
        }

        public SettingDto AddSetting(string displayName, string description = "")
        {
            var setting = new SettingDto(displayName, description);
            Settings.Add(setting);
            return setting;
        }
    }
}
