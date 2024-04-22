import 'package:core/dependency/injector.dart';
import 'package:core/models/common.dart';
import 'package:core/services/index.dart';
import 'package:core/utils/localization.utils.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:flutter/widgets.dart';
import 'package:get/get.dart' hide Rx;
import 'package:rxdart/rxdart.dart';
import 'package:rxdart_ext/rxdart_ext.dart';

import '../proxy/volo/abp/asp-net-core/mvc/index.dart';
import 'service.base.dart';

class LocalizationService extends ServiceBase implements TranslationService {
  LocalizationService(super._injector);

  SessionService get _sessionService => resolve<SessionService>();
  ConfigStateService get _configStateService => resolve<ConfigStateService>();
  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  
  String? get currentLang => _sessionService.currentLanguage;

  static LocalizationService get to => Injector.instance.get<LocalizationService>();

  final Subject<Localization> _localization$ = BehaviorSubject<Localization>();
  final BehaviorSubject<Map<String, Map<String, String>>> _localizations$ = BehaviorSubject<Map<String, Map<String, String>>>.seeded({});

  @override
  void onInit() {
    super.onInit();
    _listenToSetLanguage();
    _initLocalizationValues();
  }

  void _initLocalizationValues() {
    // var legacyResources$ = configStateService.sliceConfig((config) => config.localization?.values);
    // var remoteLocalizations$ = configStateService.sliceConfig((config) => config.localization?.resources);
    var currentLanguage$ = _sessionService.getLanguage$();
    var localizations$ = _configStateService.getLocalization$();

    // 本地化
    Rx.combineLatest2(currentLanguage$, localizations$, (currentLang, localizations) {
      if (currentLang == null || localizations == null) return null;
      if (currentLang.isNullOrWhiteSpace() || localizations.resources == null) return null;
      var remote = combineLegacyandNewResources(localizations.values, localizations.resources!);
      if (remote == null) return null;
      List<LocalizationResource> resourceList = [];
      remote.forEach((resourceName, resources) {
        Map<String, String> texts = {};
        resources.forEach((key, value) {
          if (value == null) return;
          texts.putIfAbsent(key, () => value);
        });
        resourceList.add(LocalizationResource(resourceName, texts));
      });
      return Localization(currentLang, resourceList);
    }).whereNotNull()
      .listen((localization) => _localization$.add(localization));

    _localization$
      .where((localization) {
        var environment = _environmentService.getEnvironment();
        return environment.localization.useLocalResources == false;
      })
      .listen((localization) async { 
        Map<String, Map<String, String>> localizations = {};
        Map<String, String> cultureMap = {};
        for (var resource in localization.resources) {
          resource.texts.forEach((key, value) {
            cultureMap.putIfAbsent(key, () => value);
          });
        }
        localizations.putIfAbsent(localization.culture, () => cultureMap);
        
        Get.clearTranslations();
        Get.addTranslations(localizations);
        await Get.updateLocale(Locale.fromSubtags(languageCode: localization.culture));
        _localizations$.add(localizations);
      });
  }

  void _listenToSetLanguage() {
    var lanuage$ = _sessionService.onLanguageChange$();
    var localization$ = _configStateService.getLocalization$();

    Rx.combineLatest2(lanuage$, localization$, (lang, localization) {
      if (localization?.currentCulture?.cultureName == null) return null;
      if (lang == localization?.currentCulture?.cultureName) return null;
      return lang;
    }).whereNotNull()
      .listen((_) => _configStateService.refreshAppState());
  }

  Stream<String> localize(String resourceName, String key, {String? defaultValue}) {
    return _configStateService.getLocalization$()
      .map((localization) => LocalizationUtils.createLocalizer(localization))
      .map((localize) => localize(resourceName, key, defaultValue ?? key));
  }

  String localizeSync(String resourceName, String key, {String? defaultValue}) {
    var localization = _configStateService.getLocalization();
    return LocalizationUtils.createLocalizer(localization)(resourceName, key, defaultValue ?? key);
  }

  Stream<String> localizeWithFallback(
    List<String> resourceNames,
    List<String> keys,
    String defaultValue,
  ) {
    return _configStateService.getLocalization$()
      .map((localization) => LocalizationUtils.createLocalizerWithFallback(localization!))
      .map((localizeWithFallback) => localizeWithFallback(resourceNames, keys, defaultValue),
    );
  }

  String localizeWithFallbackSync(
    List<String> resourceNames,
    List<String> keys,
    String defaultValue,
  ) {
    var localization = _configStateService.getLocalization();
    return LocalizationUtils.createLocalizerWithFallback(localization!)(resourceNames, keys, defaultValue);
  }

  @override
  Map<String, Map<String, String>> getResources() {
    return _localizations$.value;
  }

  Map<String, String>? getResource(String resourceName) {
    return _localizations$.value[resourceName];
  }

  Stream<Map<String, String>?> getResource$(String resourceName) {
    return _localizations$.map((res) => res[resourceName]);
  }

  ApplicationLocalizationResourceDto recursivelyMergeBaseResources(String baseResourceName, Map<String, ApplicationLocalizationResourceDto> resource) {
    var item = resource[baseResourceName];
    if (item == null) {
      return ApplicationLocalizationResourceDto();
    }
    item.texts ??= {};
    if (item.baseResources?.isEmpty == true) {
      return item;
    }
    for (var baseResource in item.baseResources!) {
      var baseItem = recursivelyMergeBaseResources(baseResource, resource);
      baseItem.texts ??= {};
      item.texts!.addAll(baseItem.texts!);
    }

    return item;
  }

  Map<String, ApplicationLocalizationResourceDto> mergeResourcesWithBaseResource(Map<String, ApplicationLocalizationResourceDto> resource) {
    Map<String, ApplicationLocalizationResourceDto> entities = {};
    for (var key in resource.keys) {
      var newValue = recursivelyMergeBaseResources(key, resource);
      entities.putIfAbsent(key, () => newValue);
    }
    return entities;
  }

  Map<String, Map<String, String?>>? combineLegacyandNewResources(
    Map<String, Map<String, String?>>? legacy,
    Map<String, ApplicationLocalizationResourceDto> resource,
  ) {
    Map<String, Map<String, String?>>? legacyMerged = legacy;
    var mergedResource = mergeResourcesWithBaseResource(resource);
    mergedResource.forEach((key, value) {
      legacyMerged?.putIfAbsent(key, () => value.texts!);
    });
    return legacyMerged;
  }
}
