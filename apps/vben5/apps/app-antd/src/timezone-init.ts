import { setTimezoneHandler } from '@vben/stores';

import { useTimeZoneSettingsApi } from '@abp/settings';

/**
 * 初始化时区处理，通过API保存时区设置
 */
export function initTimezone() {
  const { getMyTimezoneApi, getTimezonesApi, updateMyTimezoneApi } =
    useTimeZoneSettingsApi();
  setTimezoneHandler({
    getTimezone() {
      return getMyTimezoneApi();
    },
    setTimezone(timezone: string) {
      return updateMyTimezoneApi(timezone);
    },
    async getTimezoneOptions() {
      const timezones = await getTimezonesApi();
      return timezones.map((timezone) => {
        return {
          label: timezone.name,
          value: timezone.value,
        };
      });
    },
  });
}
