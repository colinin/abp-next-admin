import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

@JsonSerializable()
class LanguageInfo {
  LanguageInfo({
    this.cultureName,
    this.uiCultureName,
    this.displayName,
    this.twoLetterISOLanguageName,
    this.flagIcon,
  });
  String? cultureName;
  String? uiCultureName;
  String? displayName;
  String? twoLetterISOLanguageName;
  String? flagIcon;

  factory LanguageInfo.fromJson(Map<String, dynamic> json) => _$LanguageInfoFromJson(json);
  Map<String, dynamic> toJson() => _$LanguageInfoToJson(this);
}
