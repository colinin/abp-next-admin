import 'package:core/dependency/index.dart';
import 'package:core/services/translation.service.dart';
import 'package:get/get.dart';

class AbpTranslations extends Translations {
  AbpTranslations(this._injector);
  final Injector _injector;

  TranslationService get _translationService => _injector.get<TranslationService>();

  @override
  Map<String, Map<String, String>> get keys => _translationService.getResources();
}