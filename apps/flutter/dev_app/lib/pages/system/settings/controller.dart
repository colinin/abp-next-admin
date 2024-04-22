import 'package:core/services/index.dart';
import 'package:core/dependency/index.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'state.dart';

class SystemSettingsController extends GetxController {
  SessionService get sessionService => injector.get();
  ConfigStateService get configStateService => injector.get();

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
    _state.update((val) {
      val?.languages = configStateService.getSupportedLocales();
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