import 'package:core/services/localization.service.dart';
import 'package:get/get.dart';

class AbpTranslations extends Translations {
  AbpTranslations(this.localizationService);
  final LocalizationService localizationService;
  @override
  Map<String, Map<String, String>> get keys => localizationService.getResources();
}