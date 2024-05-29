<template>
  <div class="lg:flex">
    <Avatar :src="userinfo.avatar || headerImg" :size="72" class="!mx-auto !block" />
    <div class="md:ml-6 flex flex-col justify-center md:mt-0 mt-2">
      <h1 class="md:text-lg text-md">{{ getWelcomeTitle }}</h1>
      <span class="text-secondary">{{ getWeatherInfo }}</span>
    </div>
    <div class="flex flex-1 justify-end md:mt-0 mt-4">
      <div class="flex flex-col justify-center text-right md:mr-10 mr-4">
        <span class="text-secondary">{{
          t('routes.dashboard.workbench.header.notifier.title')
        }}</span>
        <a href="javascript:void(0)" class="text-2xl">{{
          t('routes.dashboard.workbench.header.notifier.count', [unReadNotiferCount])
        }}</a>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { computed, onMounted, ref } from 'vue';
  import { Avatar } from 'ant-design-vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useUserStore } from '/@/store/modules/user';
  import { useWeatherStore } from '/@/store/modules/weather';
  import { getList as getNotifers } from '/@/api/messages/notifications';
  import { NotificationReadState } from '/@/api/messages/notifications/model';
  import headerImg from '/@/assets/images/header.jpg';

  const { t } = useI18n();
  const userStore = useUserStore();
  const weatherStore = useWeatherStore();
  const userinfo = computed(() => userStore.getUserInfo);
  const getUserInfo = computed(() => {
    const { avatar, desc, realName, username } = userStore.getUserInfo || {};
    let userName = realName ?? username;
    return {
      realName: userName,
      avatar: avatar || headerImg,
      desc,
    };
  });

  const getWelcomeTitle = computed(() => {
    const now = new Date();
    const hour = now.getHours();
    if (hour < 12) {
      return t('routes.dashboard.workbench.header.welcomeMorning', [getUserInfo.value.realName]);
    }
    if (hour < 14) {
      return t('routes.dashboard.workbench.header.welcomeAtoon', [getUserInfo.value.realName]);
    }
    if (hour < 17) {
      return t('routes.dashboard.workbench.header.welcomeAfternoon', [getUserInfo.value.realName]);
    }
    if (hour < 24) {
      return t('routes.dashboard.workbench.header.welcomeEvening', [getUserInfo.value.realName]);
    }
  });

  const getWeatherInfo = computed(() => {
    const weather = weatherStore.weather;
    if (!weather) {
      return '';
    }
    return t('routes.dashboard.workbench.header.todayWeather', {
      city: weather.real.station.city,
      weather: weather.real.weather.info,
      temperature: weather.real.weather.temperature,
      wind: `${weather.real.wind.direct} - ${weather.real.wind.power}`,
      speed: weather.real.wind.speed,
    });
  });

  const unReadNotiferCount = ref(0);

  onMounted(() => {
    fetchUnReadNotifiers();
    weatherStore.updateWeather();
  });

  function fetchUnReadNotifiers() {
    getNotifers({
      skipCount: 0,
      maxResultCount: 1,
      reverse: false,
      sorting: undefined,
      readState: NotificationReadState.UnRead,
    }).then((res) => {
      unReadNotiferCount.value = res.totalCount;
    });
  }
</script>
