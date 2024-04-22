import 'dart:ui';

import 'package:core/services/environment.service.dart';
import 'package:core/services/localization.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/translation.service.dart';
import 'package:core/utils/index.dart';
import 'package:dev_app/models/translation.state.dart';
import 'package:get/get.dart';
import 'package:rxdart/rxdart.dart';
import 'package:flutter/services.dart' show rootBundle;
import 'dart:convert' show jsonDecode;

class TranslationResService extends ServiceBase implements TranslationService {
  TranslationResService(super.injector);

  final InternalStore<TranslationState> _store = InternalStore<TranslationState>(state: TranslationState());

  SessionService get _sessionService => resolve<SessionService>();
  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  LocalizationService get _localizationService => resolve<LocalizationService>();

  @override
  void onInit() {
    super.onInit();
    _initTranslations();
  }

  void _initTranslations() {
    var translation$ = _store.sliceState((state) => state);
    translation$.listen((state) async {
      //Get.clearTranslations();
      if (state.translations.isEmpty) return;
      Get.appendTranslations(state.translations);
      await Get.updateLocale(Locale.fromSubtags(languageCode: state.language!));
    });
    _store.patch((state) {
      state.translations = _localizationService.getResources();
    });
    _sessionService.onLanguageChange$()
      .whereNotNull()
      .switchMap((language) => Stream.fromFuture(_mapTranslationsMap(language)))
      .listen(_store.set);
  }

  Future<TranslationState> _mapTranslationsMap(String language) async {
    Map<String, Map<String, String>> translationsMap = {};
    var environment = _environmentService.getEnvironment();
    var translationFiles = environment.localization.translationFiles?[language] ?? ['$language.json'];
    
    for (var translationFile in translationFiles) {
      try {
        var filePath = 'res/translations/$translationFile';
        var content = await rootBundle.loadString(filePath);
        var translationsObject = jsonDecode(content) as Map<String, dynamic>;
        var translations = translationsMap[language] ?? {};
        translations.addAll(translationsObject.map((key, value) => MapEntry(key, value)));
        translationsMap.putIfAbsent(language, () => translations);
      } catch (e) {
        logger.error(e);
      }
    }
    return TranslationState(
      language: language,
      translations: translationsMap,
    );
  }

  @override
  Map<String, Map<String, String>> getResources() {
    return _store.state.translations;
  }
}