import 'package:core/utils/string.extensions.dart';
import 'package:get/get.dart';

import '../proxy/volo/abp/asp-net-core/mvc/index.dart';

class LocalizationUtils {
  static String Function(String resourceName,
    String key,
    String? defaultValue) createLocalizer(ApplicationLocalizationConfigurationDto? localization) {
      return (String resourceName, String key, String? defaultValue) {
        if (localization == null || resourceName == '_') return key;
        var resource = localization.values?[resourceName];
        if (resource == null) return defaultValue ?? key;
        return resource[key] ?? defaultValue ?? key;
    };
  }

  static String Function(List<String> resourceNames, List<String> keys, String defaultValue) createLocalizerWithFallback(
    ApplicationLocalizationConfigurationDto localization) {
    var findLocalization = createLocalizationFinder(localization);
    return (List<String> resourceNames, List<String> keys, String defaultValue) {
      var localizedText = findLocalization(resourceNames, keys);
      return localizedText.localized ?? defaultValue;
    };
  }

  static LocalizedText Function(List<String> resourceNames, List<String> keys) createLocalizationFinder(
    ApplicationLocalizationConfigurationDto localization) {
    var localize = createLocalizer(localization);
    return (List<String> resourceNames, List<String> keys) {
      if (localization.defaultResourceName.isNullOrWhiteSpace() == false &&
          resourceNames.contains(localization.defaultResourceName!)) {
        resourceNames.add(localization.defaultResourceName!);
      }
      var resourceCount = resourceNames.length;
      var keyCount = keys.length;

      for (var i = 0; i < resourceCount; i++) {
        var resourceName = resourceNames[i];

        for (var j = 0; j < keyCount; j++) {
          var key = keys[j];
          var localized = localize(resourceName, key, null);
          if (!localized.isNullOrWhiteSpace()) {
            return LocalizedText(
              resourceName: resourceName,
              key: key,
              localized: localized,
            );
          }
        }
      }

      return LocalizedText();
    };
  }
}

class LocalizedText {
  LocalizedText({
    this.resourceName,
    this.key,
    this.localized,
  });
  String? resourceName;
  String? key;
  String? localized;
}

extension AbpTranslationsExtensions on String {
  String trFormat([Map<String, String> params = const {}]) {
    var trans = tr;
    if (params.isNotEmpty) {
      params.forEach((key, value) {
        trans = trans.replaceAll('{$key}', value);
      });
    }
    return trans;
  }
}