import 'package:core/utils/internal.store.dart';
import 'package:core/utils/theme.utils.dart';
import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart';
import 'package:get/get.dart';

import 'service.base.dart';
import 'storage.service.dart';

class ThemeState {
  ThemeState(
    this.brightness,
    this.colorScheme,
  );
  Brightness brightness;
  ColorScheme colorScheme;
}

class ThemeService extends ServiceBase {
  ThemeService(super._injector);
  static const String themeKey = '_abp_theme_';

  StorageService get _storageService => resolve<StorageService>();
  
  final InternalStore<ThemeState> _store = InternalStore<ThemeState>(state: ThemeState(Brightness.light, ThemeUtils.lightColorScheme));

  Brightness get currentThemeMode => _store.state.brightness;
  ColorScheme get themeColor => _store.state.colorScheme;

  static Brightness getSystemTheme() {
    return SchedulerBinding.instance.platformDispatcher.platformBrightness;
  }

  void _initState() {
    var theme = StorageService.initStorage(themeKey, (value) => value);

    switch (theme)
    {
      case 'light': return changeThemeMode(ThemeMode.light);
      case 'dark': return changeThemeMode(ThemeMode.dark);
      case 'system': return changeThemeMode(ThemeMode.system);
    }
  }
  
  @override
  void onInit() {
    super.onInit();
    _initUpdateStream();
    _initState();
  }

  void _initUpdateStream() {
    _store.sliceUpdate((state) => state.brightness)
      .listen((brightness) { 
        switch (brightness)
        {
          case Brightness.dark:
            ThemeUtils.currentTheme = ThemeUtils.darkTheme;
            ThemeUtils.currentColor = ThemeUtils.darkColorScheme;
            _storageService.setItem(themeKey, 'dark');
            Get.changeThemeMode(ThemeMode.dark);
            return;
          case Brightness.light:
            ThemeUtils.currentTheme = ThemeUtils.lightTheme;
            ThemeUtils.currentColor = ThemeUtils.lightColorScheme;
            _storageService.setItem(themeKey, 'light');
            Get.changeThemeMode(ThemeMode.light);
            return;
        }
      });
  }

  void changeThemeMode(ThemeMode mode) {
    switch (mode)
    {
      case ThemeMode.dark: _store.patch((state) => state.brightness = Brightness.dark);
      case ThemeMode.light: _store.patch((state) => state.brightness = Brightness.light);
      case ThemeMode.system: _store.patch((state) => state.brightness = getSystemTheme());
    }
  }
}