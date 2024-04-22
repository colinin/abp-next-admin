import 'package:core/abstracts/copy.with.dart';
import 'package:core/proxy/volo/abp/localization/index.dart';

class SettingsState extends CloneObject<SettingsState> {
  SettingsState({
    this.language,
    this.languages,
  });
  String? language;
  List<LanguageInfo>? languages;
  
  @override
  SettingsState clone() => SettingsState(
    language: language,
  );
}