// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'models.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ApplicationConfigurationRequestOptions
    _$ApplicationConfigurationRequestOptionsFromJson(
            Map<String, dynamic> json) =>
        ApplicationConfigurationRequestOptions(
          includeLocalizationResources:
              json['includeLocalizationResources'] as bool?,
        );

Map<String, dynamic> _$ApplicationConfigurationRequestOptionsToJson(
        ApplicationConfigurationRequestOptions instance) =>
    <String, dynamic>{
      'includeLocalizationResources': instance.includeLocalizationResources,
    };

ApplicationLocalizationRequestDto _$ApplicationLocalizationRequestDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationLocalizationRequestDto(
      cultureName: json['cultureName'] as String,
      onlyDynamics: json['onlyDynamics'] as bool?,
    );

Map<String, dynamic> _$ApplicationLocalizationRequestDtoToJson(
        ApplicationLocalizationRequestDto instance) =>
    <String, dynamic>{
      'cultureName': instance.cultureName,
      'onlyDynamics': instance.onlyDynamics,
    };

ApplicationConfigurationDto _$ApplicationConfigurationDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationConfigurationDto(
      timing: json['timing'] == null
          ? null
          : TimingDto.fromJson(json['timing'] as Map<String, dynamic>),
      clock: json['clock'] == null
          ? null
          : ClockDto.fromJson(json['clock'] as Map<String, dynamic>),
      localization: json['localization'] == null
          ? null
          : ApplicationLocalizationConfigurationDto.fromJson(
              json['localization'] as Map<String, dynamic>),
      auth: json['auth'] == null
          ? null
          : ApplicationAuthConfigurationDto.fromJson(
              json['auth'] as Map<String, dynamic>),
      setting: json['setting'] == null
          ? null
          : ApplicationSettingConfigurationDto.fromJson(
              json['setting'] as Map<String, dynamic>),
      currentUser: json['currentUser'] == null
          ? null
          : CurrentUserDto.fromJson(
              json['currentUser'] as Map<String, dynamic>),
      features: json['features'] == null
          ? null
          : ApplicationFeatureConfigurationDto.fromJson(
              json['features'] as Map<String, dynamic>),
      globalFeatures: json['globalFeatures'] == null
          ? null
          : ApplicationGlobalFeatureConfigurationDto.fromJson(
              json['globalFeatures'] as Map<String, dynamic>),
      multiTenancy: json['multiTenancy'] == null
          ? null
          : MultiTenancyInfoDto.fromJson(
              json['multiTenancy'] as Map<String, dynamic>),
      currentTenant: json['currentTenant'] == null
          ? null
          : CurrentTenantDto.fromJson(
              json['currentTenant'] as Map<String, dynamic>),
      objectExtensions: json['objectExtensions'] == null
          ? null
          : ObjectExtensionsDto.fromJson(
              json['objectExtensions'] as Map<String, dynamic>),
      extraProperties: (json['extraProperties'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as Object),
      ),
    );

Map<String, dynamic> _$ApplicationConfigurationDtoToJson(
        ApplicationConfigurationDto instance) =>
    <String, dynamic>{
      'timing': instance.timing,
      'clock': instance.clock,
      'localization': instance.localization,
      'auth': instance.auth,
      'setting': instance.setting,
      'currentUser': instance.currentUser,
      'features': instance.features,
      'globalFeatures': instance.globalFeatures,
      'multiTenancy': instance.multiTenancy,
      'currentTenant': instance.currentTenant,
      'objectExtensions': instance.objectExtensions,
      'extraProperties': instance.extraProperties,
    };

ApplicationLocalizationDto _$ApplicationLocalizationDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationLocalizationDto(
      resources: (json['resources'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(
            k,
            ApplicationLocalizationResourceDto.fromJson(
                e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$ApplicationLocalizationDtoToJson(
        ApplicationLocalizationDto instance) =>
    <String, dynamic>{
      'resources': instance.resources,
    };

ApplicationGlobalFeatureConfigurationDto
    _$ApplicationGlobalFeatureConfigurationDtoFromJson(
            Map<String, dynamic> json) =>
        ApplicationGlobalFeatureConfigurationDto(
          enabledFeatures: (json['enabledFeatures'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList(),
        );

Map<String, dynamic> _$ApplicationGlobalFeatureConfigurationDtoToJson(
        ApplicationGlobalFeatureConfigurationDto instance) =>
    <String, dynamic>{
      'enabledFeatures': instance.enabledFeatures,
    };

ApplicationFeatureConfigurationDto _$ApplicationFeatureConfigurationDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationFeatureConfigurationDto(
      values: (json['values'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String?),
      ),
    );

Map<String, dynamic> _$ApplicationFeatureConfigurationDtoToJson(
        ApplicationFeatureConfigurationDto instance) =>
    <String, dynamic>{
      'values': instance.values,
    };

CurrentUserDto _$CurrentUserDtoFromJson(Map<String, dynamic> json) =>
    CurrentUserDto(
      isAuthenticated: json['isAuthenticated'] as bool?,
      id: json['id'] as String?,
      tenantId: json['tenantId'] as String?,
      impersonatorUserId: json['impersonatorUserId'] as String?,
      impersonatorTenantId: json['impersonatorTenantId'] as String?,
      impersonatorUserName: json['impersonatorUserName'] as String?,
      impersonatorTenantName: json['impersonatorTenantName'] as String?,
      userName: json['userName'] as String?,
      name: json['name'] as String?,
      surName: json['surName'] as String?,
      email: json['email'] as String?,
      emailVerified: json['emailVerified'] as bool?,
      phoneNumber: json['phoneNumber'] as String?,
      phoneNumberVerified: json['phoneNumberVerified'] as bool?,
      roles:
          (json['roles'] as List<dynamic>?)?.map((e) => e as String).toList(),
    );

Map<String, dynamic> _$CurrentUserDtoToJson(CurrentUserDto instance) =>
    <String, dynamic>{
      'isAuthenticated': instance.isAuthenticated,
      'id': instance.id,
      'tenantId': instance.tenantId,
      'impersonatorUserId': instance.impersonatorUserId,
      'impersonatorTenantId': instance.impersonatorTenantId,
      'impersonatorUserName': instance.impersonatorUserName,
      'impersonatorTenantName': instance.impersonatorTenantName,
      'userName': instance.userName,
      'name': instance.name,
      'surName': instance.surName,
      'email': instance.email,
      'emailVerified': instance.emailVerified,
      'phoneNumber': instance.phoneNumber,
      'phoneNumberVerified': instance.phoneNumberVerified,
      'roles': instance.roles,
    };

ApplicationSettingConfigurationDto _$ApplicationSettingConfigurationDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationSettingConfigurationDto(
      values: (json['values'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String?),
      ),
    );

Map<String, dynamic> _$ApplicationSettingConfigurationDtoToJson(
        ApplicationSettingConfigurationDto instance) =>
    <String, dynamic>{
      'values': instance.values,
    };

ApplicationAuthConfigurationDto _$ApplicationAuthConfigurationDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationAuthConfigurationDto(
      grantedPolicies: (json['grantedPolicies'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as bool),
      ),
    );

Map<String, dynamic> _$ApplicationAuthConfigurationDtoToJson(
        ApplicationAuthConfigurationDto instance) =>
    <String, dynamic>{
      'grantedPolicies': instance.grantedPolicies,
    };

ApplicationLocalizationConfigurationDto
    _$ApplicationLocalizationConfigurationDtoFromJson(
            Map<String, dynamic> json) =>
        ApplicationLocalizationConfigurationDto(
          values: (json['values'] as Map<String, dynamic>?)?.map(
            (k, e) => MapEntry(k, Map<String, String?>.from(e as Map)),
          ),
          resources: (json['resources'] as Map<String, dynamic>?)?.map(
            (k, e) => MapEntry(
                k,
                ApplicationLocalizationResourceDto.fromJson(
                    e as Map<String, dynamic>)),
          ),
          languages: (json['languages'] as List<dynamic>?)
              ?.map((e) => LanguageInfo.fromJson(e as Map<String, dynamic>))
              .toList(),
          currentCulture: json['currentCulture'] == null
              ? null
              : CurrentCultureDto.fromJson(
                  json['currentCulture'] as Map<String, dynamic>),
          defaultResourceName: json['defaultResourceName'] as String?,
          languagesMap: (json['languagesMap'] as Map<String, dynamic>?)?.map(
            (k, e) => MapEntry(
                k,
                (e as List<dynamic>)
                    .map((e) => StringValue.fromJson(e as Map<String, dynamic>))
                    .toList()),
          ),
          languageFilesMap:
              (json['languageFilesMap'] as Map<String, dynamic>?)?.map(
            (k, e) => MapEntry(
                k,
                (e as List<dynamic>)
                    .map((e) => StringValue.fromJson(e as Map<String, dynamic>))
                    .toList()),
          ),
        );

Map<String, dynamic> _$ApplicationLocalizationConfigurationDtoToJson(
        ApplicationLocalizationConfigurationDto instance) =>
    <String, dynamic>{
      'values': instance.values,
      'resources': instance.resources,
      'languages': instance.languages,
      'currentCulture': instance.currentCulture,
      'defaultResourceName': instance.defaultResourceName,
      'languagesMap': instance.languagesMap,
      'languageFilesMap': instance.languageFilesMap,
    };

CurrentCultureDto _$CurrentCultureDtoFromJson(Map<String, dynamic> json) =>
    CurrentCultureDto(
      name: json['name'] as String?,
      cultureName: json['cultureName'] as String?,
      displayName: json['displayName'] as String?,
      englishName: json['englishName'] as String?,
      threeLetterIsoLanguageName: json['threeLetterIsoLanguageName'] as String?,
      twoLetterIsoLanguageName: json['twoLetterIsoLanguageName'] as String?,
      isRightToLeft: json['isRightToLeft'] as bool?,
      nativeName: json['nativeName'] as String?,
      dateTimeFormat: json['dateTimeFormat'] == null
          ? null
          : DateTimeFormatDto.fromJson(
              json['dateTimeFormat'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$CurrentCultureDtoToJson(CurrentCultureDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'cultureName': instance.cultureName,
      'displayName': instance.displayName,
      'englishName': instance.englishName,
      'threeLetterIsoLanguageName': instance.threeLetterIsoLanguageName,
      'twoLetterIsoLanguageName': instance.twoLetterIsoLanguageName,
      'isRightToLeft': instance.isRightToLeft,
      'nativeName': instance.nativeName,
      'dateTimeFormat': instance.dateTimeFormat,
    };

DateTimeFormatDto _$DateTimeFormatDtoFromJson(Map<String, dynamic> json) =>
    DateTimeFormatDto(
      calendarAlgorithmType: json['calendarAlgorithmType'] as String?,
      dateTimeFormatLong: json['dateTimeFormatLong'] as String?,
      shortDatePattern: json['shortDatePattern'] as String?,
      fullDateTimePattern: json['fullDateTimePattern'] as String?,
      dateSeparator: json['dateSeparator'] as String?,
      shortTimePattern: json['shortTimePattern'] as String?,
      longTimePattern: json['longTimePattern'] as String?,
    );

Map<String, dynamic> _$DateTimeFormatDtoToJson(DateTimeFormatDto instance) =>
    <String, dynamic>{
      'calendarAlgorithmType': instance.calendarAlgorithmType,
      'dateTimeFormatLong': instance.dateTimeFormatLong,
      'shortDatePattern': instance.shortDatePattern,
      'fullDateTimePattern': instance.fullDateTimePattern,
      'dateSeparator': instance.dateSeparator,
      'shortTimePattern': instance.shortTimePattern,
      'longTimePattern': instance.longTimePattern,
    };

ApplicationLocalizationResourceDto _$ApplicationLocalizationResourceDtoFromJson(
        Map<String, dynamic> json) =>
    ApplicationLocalizationResourceDto(
      texts: (json['texts'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String?),
      ),
      baseResources: (json['baseResources'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList(),
    );

Map<String, dynamic> _$ApplicationLocalizationResourceDtoToJson(
        ApplicationLocalizationResourceDto instance) =>
    <String, dynamic>{
      'texts': instance.texts,
      'baseResources': instance.baseResources,
    };

ClockDto _$ClockDtoFromJson(Map<String, dynamic> json) => ClockDto(
      kind: json['kind'] as String?,
    );

Map<String, dynamic> _$ClockDtoToJson(ClockDto instance) => <String, dynamic>{
      'kind': instance.kind,
    };

TimingDto _$TimingDtoFromJson(Map<String, dynamic> json) => TimingDto(
      timeZone: json['timeZone'] == null
          ? null
          : TimeZone.fromJson(json['timeZone'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$TimingDtoToJson(TimingDto instance) => <String, dynamic>{
      'timeZone': instance.timeZone,
    };

TimeZone _$TimeZoneFromJson(Map<String, dynamic> json) => TimeZone(
      iana: json['iana'] == null
          ? null
          : IanaTimeZone.fromJson(json['iana'] as Map<String, dynamic>),
      windows: json['windows'] == null
          ? null
          : WindowsTimeZone.fromJson(json['windows'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$TimeZoneToJson(TimeZone instance) => <String, dynamic>{
      'iana': instance.iana,
      'windows': instance.windows,
    };

WindowsTimeZone _$WindowsTimeZoneFromJson(Map<String, dynamic> json) =>
    WindowsTimeZone(
      timeZoneId: json['timeZoneId'] as String?,
    );

Map<String, dynamic> _$WindowsTimeZoneToJson(WindowsTimeZone instance) =>
    <String, dynamic>{
      'timeZoneId': instance.timeZoneId,
    };

IanaTimeZone _$IanaTimeZoneFromJson(Map<String, dynamic> json) => IanaTimeZone(
      timeZoneName: json['timeZoneName'] as String?,
    );

Map<String, dynamic> _$IanaTimeZoneToJson(IanaTimeZone instance) =>
    <String, dynamic>{
      'timeZoneName': instance.timeZoneName,
    };
