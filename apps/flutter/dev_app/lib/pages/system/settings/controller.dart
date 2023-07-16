import 'package:core/services/index.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'state.dart';

class SystemSettingsController extends GetxController {
  SessionService get sessionService => Get.find();
  ConfigStateService get configStateService => Get.find();

  final Rx<SettingsState> _state = Rx<SettingsState>(SettingsState(
    languages: [],
  ));
  SettingsState get state => _state.value;

  bool inProgress = false;
  GlobalKey<FormState> formKey = GlobalKey<FormState>();

  void onLanguageChange(String? language) {
    _state.update((val) { 
      val?.language = language;
    });
  }

  void submit() {
    if (formKey.currentState?.validate() == false || inProgress) {
      return;
    }
    if (!state.language.isNullOrWhiteSpace()) {
      sessionService.setLanguage(state.language!);
    }
  }

  void _initLanguages() {
    var localization = configStateService.getLocalization();
    _state.update((val) {
      val?.languages = localization?.languages;
    });
  }

  @override
  void onInit() {
    super.onInit();
    _initLanguages();
    _state.update((val) { 
      val?.language = sessionService.currentLanguage;
    });
  }

  @override
  void onClose() {
    _state.close();
    super.onClose();
  }
}