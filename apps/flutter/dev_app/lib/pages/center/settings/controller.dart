import 'package:dev_app/pages/center/settings/state.dart';
import 'package:core/dependency/index.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/theme.service.dart';
import 'package:flutter/material.dart';
import 'package:flutter_picker/flutter_picker.dart';
import 'package:get/get.dart';

class CenterSettingsController extends GetxController {
  SessionService get sessionService => injector.get();
  ThemeService get themeService => injector.get();

  final Rx<CenterState> _state = Rx<CenterState>(CenterState(false));
  CenterState get state => _state.value;

  @override
  void onInit() {
    super.onInit();
    sessionService.getProfile$()
      .listen((profile) {
        _state.update((val) {
          val!.isAuthenticated = sessionService.isAuthenticated;
        });
      });
  }

  void logout() {
    sessionService.resetSession();
  }

  void navigateTo(String route) {
    Get.toNamed(route);
  }

  /// 主题索引映射
  final Map<int, ThemeMode> themeModeMap = {
    0: ThemeMode.light,
    1: ThemeMode.dark,
    2: ThemeMode.system,
  };
  /// 当前主题索引
  int get themeModeIndex {
    switch (themeService.currentThemeMode) {
      case Brightness.light: return 0;
      case Brightness.dark: return 1;
      default: return 2;
    }
  }

  /// 切换主题色对话框
  void showThemeModalPicker(BuildContext context) {
    var picker = Picker(
      adapter: PickerDataAdapter(
        pickerData: ['Theme:Light'.tr, 'Theme:Dark'.tr, 'Theme:System'.tr ],
        isArray: false,
      ),
      selecteds: [themeModeIndex],
      title: Text('Label:Theme'.tr),
      cancelText: 'Label:Cancel'.tr,
      confirmText: 'Label:Confirm'.tr,
      height: 210,
      itemExtent: 40,
      selectedTextStyle: const TextStyle(color: Colors.blue),
      onConfirm: (picker, indexs) {
        themeService.changeThemeMode(themeModeMap[indexs[0]]!);
      },
      backgroundColor: Theme.of(context).colorScheme.background,
    );
    picker.showModal(context);
  }
}