import 'dart:async';
import 'package:core/utils/string.extensions.dart';
import 'package:rxdart/rxdart.dart';

import 'service.base.dart';
import 'package:core/utils/internal.store.dart';
import 'package:core/proxy/volo/abp/asp-net-core/mvc/application-configurations/index.dart';


class ConfigStateService extends ServiceBase {
  AbpApplicationConfigurationService get abpConfigService => find();
  AbpApplicationLocalizationService get abpApplicationLocalizationService => find();
  final bool? includeLocalizationResources = false;

  final InternalStore<ApplicationConfigurationDto> _store = InternalStore<ApplicationConfigurationDto>(state: ApplicationConfigurationDto());
  final PublishSubject<int> _updateSubject = PublishSubject();
  
  @override
  void onInit() {
    super.onInit();
    _initUpdateStream();
  }

  @override
  void onClose() {
    _updateSubject.close();
    super.onClose();
  }

  _initUpdateStream() {
    _updateSubject
      .switchMap((_) => Stream.fromFuture(getAbpConfig()))
      .switchMap((appState) => Stream.fromFuture(getLocalizationAndCombineWithAppState(appState)))
      .listen((appState) => _store.set(appState));
  }

  Future<ApplicationConfigurationDto> getLocalizationAndCombineWithAppState(ApplicationConfigurationDto appState) {
    if (appState.localization?.currentCulture?.cultureName?.isNullOrWhiteSpace() == true) {
      throw Exception('culture name should defined');
    }
    return getlocalizationResource(appState.localization!.currentCulture!.cultureName!)
      .then((localization) {
        var abpConfig = appState.cloneWith((state) {
          state.localization!.resources = localization.resources;
        });
        return abpConfig;
      });
  }

  Stream<ApplicationConfigurationDto> refreshAppState() {
    _updateSubject.add(1);
    return _store.sliceUpdate((state) => state).take(1);
  }

  Future<ApplicationLocalizationDto> getlocalizationResource(String cultureName) {
    return abpApplicationLocalizationService.get(ApplicationLocalizationRequestDto(
      cultureName: cultureName,
      onlyDynamics: false));
  }

  Future<ApplicationConfigurationDto> getAbpConfig() {
    return abpConfigService.get(ApplicationConfigurationRequestOptions(
      includeLocalizationResources: includeLocalizationResources
    ));
  }

  ApplicationConfigurationDto getAll() {
    return _store.state;
  }

  ApplicationLocalizationConfigurationDto? getLocalization() {
    return _store.state.localization;
  }

  Stream<ApplicationLocalizationConfigurationDto?> getLocalization$() {
    return _store.sliceState((state) => state.localization);
  }

  String? getFeature(String key) {
    return _store.state.features?.values?[key];
  }

  Map<String, String?> getFeatures(Iterable<String> keys) {
    Map<String, String?> mapFeatures = {};
    var features = _store.state.features;
    if (features == null) {
      return mapFeatures;
    }

    for (var key in keys) {
      mapFeatures[key] = features.values?[key];
    }

    return mapFeatures;
  }

  String? getSetting(String key) {
    return _store.state.setting?.values?[key];
  }

  Map<String, String?> getSettings(String? keyword) {
    var settings = _store.state.setting?.values ?? {};
    if (keyword == null) {
      return settings;
    }
    Map<String, String> mapSettings = {};
    var keysFound = settings.keys.where((key) => key.contains(keyword));

    for (var key in keysFound) {
      mapSettings[key] = settings[key] ?? '';
    }

    return mapSettings;
  }

  ApplicationGlobalFeatureConfigurationDto? getGlobalFeatures() {
    return _store.state.globalFeatures;
  }

  bool _isGlobalFeatureEnabled(
    String key,
    ApplicationGlobalFeatureConfigurationDto globalFeatures,
  ) {
    var features = globalFeatures.enabledFeatures ?? [];
    return features.any((fk) => fk == key);
  }

  bool getGlobalFeatureIsEnabled(String key) {
    return _isGlobalFeatureEnabled(key, _store.state.globalFeatures!);
  }
}