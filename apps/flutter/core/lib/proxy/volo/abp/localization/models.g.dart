// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'models.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LanguageInfo _$LanguageInfoFromJson(Map<String, dynamic> json) => LanguageInfo(
      cultureName: json['cultureName'] as String?,
      uiCultureName: json['uiCultureName'] as String?,
      displayName: json['displayName'] as String?,
      twoLetterISOLanguageName: json['twoLetterISOLanguageName'] as String?,
      flagIcon: json['flagIcon'] as String?,
    );

Map<String, dynamic> _$LanguageInfoToJson(LanguageInfo instance) =>
    <String, dynamic>{
      'cultureName': instance.cultureName,
      'uiCultureName': instance.uiCultureName,
      'displayName': instance.displayName,
      'twoLetterISOLanguageName': instance.twoLetterISOLanguageName,
      'flagIcon': instance.flagIcon,
    };
