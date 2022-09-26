import { defineStore } from 'pinia';
import { Position, WeatherInfo } from '/@/api/weather/model';
import { getPosition, getWeather } from '/@/api/weather';
import { useLocaleStoreWithOut } from './locale';

interface WeatherState {
  currentCity?: Position;
  weather?: WeatherInfo;
  lastUpdateHour: number;
}

export const useWeatherStore = defineStore({
  id: 'weather',
  state: (): WeatherState => ({
    currentCity: undefined,
    weather: undefined,
    lastUpdateHour: -1,
  }),
  actions: {
    async updateWeather() {
      const localeStore = useLocaleStoreWithOut();
      // 中文环境天气api有效
      if (localeStore.getLocale.includes('zh')) {
        const hour = new Date().getHours();
        if (hour - this.lastUpdateHour != 0) {
          const position = await getPosition();
          const weatherResult = await getWeather(position.code);
          if (weatherResult.code !== 0) {
            return Promise.reject(weatherResult.msg);
          }
          this.currentCity = position;
          this.weather = weatherResult.data;
          this.lastUpdateHour = hour;
          return Promise.resolve(this.weather);
        }
      }
      return Promise.resolve(this.weather);
    }
  }
});
