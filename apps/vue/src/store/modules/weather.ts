import { defineStore } from 'pinia';
import { Position, WeatherInfo } from '/@/api/weather/model';
import { getPosition, getWeather } from '/@/api/weather';

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
      } else {
        return Promise.resolve(this.weather);
      }
    }
  }
});
