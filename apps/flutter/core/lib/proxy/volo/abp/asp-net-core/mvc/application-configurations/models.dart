import 'package:core/abstracts/copy.with.dart';
import 'package:core/proxy/volo/abp/asp-net-core/mvc/application-configurations/object-extending/index.dart';
import 'package:core/proxy/volo/abp/asp-net-core/mvc/multi-tenancy/index.dart';
import 'package:core/proxy/volo/abp/localization/index.dart';
import 'package:core/proxy/volo/abp/models.dart';
import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

@JsonSerializable()
class ApplicationConfigurationRequestOptions {
  ApplicationConfigurationRequestOptions({
    this.includeLocalizationResources,
  });
  bool? includeLocalizationResources = false;

  factory ApplicationConfigurationRequestOptions.fromJson(Map<String, dynamic> json) => _$ApplicationConfigurationRequestOptionsFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationConfigurationRequestOptionsToJson(this);
}

@JsonSerializable()
class ApplicationLocalizationRequestDto {
  ApplicationLocalizationRequestDto({
    required this.cultureName,
    this.onlyDynamics,
  });
  late String cultureName;
  bool? onlyDynamics = false;

  factory ApplicationLocalizationRequestDto.fromJson(Map<String, dynamic> json) => _$ApplicationLocalizationRequestDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationLocalizationRequestDtoToJson(this);
}

@JsonSerializable()
class ApplicationConfigurationDto extends CloneObject<ApplicationConfigurationDto> {
  ApplicationConfigurationDto({
    this.timing,
    this.clock,
    this.localization,
    this.auth,
    this.setting,
    this.currentUser,
    this.features,
    this.globalFeatures,
    this.multiTenancy,
    this.currentTenant,
    this.objectExtensions,
    this.extraProperties,
  });
  TimingDto? timing = TimingDto();
  ClockDto? clock = ClockDto();
  ApplicationLocalizationConfigurationDto? localization = ApplicationLocalizationConfigurationDto();
  ApplicationAuthConfigurationDto? auth;
  ApplicationSettingConfigurationDto? setting;
  CurrentUserDto? currentUser;
  ApplicationFeatureConfigurationDto? features;
  ApplicationGlobalFeatureConfigurationDto? globalFeatures;
  MultiTenancyInfoDto? multiTenancy;
  CurrentTenantDto? currentTenant;
  ObjectExtensionsDto? objectExtensions;
  Map<String, Object>? extraProperties;

  factory ApplicationConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationConfigurationDtoToJson(this);

  @override
  ApplicationConfigurationDto clone() => ApplicationConfigurationDto(
    timing: timing,
    clock: clock,
    localization: localization,
    auth: auth,
    setting: setting,
    currentTenant: currentTenant,
    currentUser: currentUser,
    features: features,
    globalFeatures: globalFeatures,
    multiTenancy: multiTenancy,
    objectExtensions: objectExtensions,
    extraProperties: extraProperties,
  );
}

@JsonSerializable()
class ApplicationLocalizationDto {
  ApplicationLocalizationDto({
    this.resources,
  });
  Map<String, ApplicationLocalizationResourceDto>? resources = {};

  factory ApplicationLocalizationDto.fromJson(Map<String, dynamic> json) => _$ApplicationLocalizationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationLocalizationDtoToJson(this);
}

@JsonSerializable()
class ApplicationGlobalFeatureConfigurationDto {
  ApplicationGlobalFeatureConfigurationDto({
    this.enabledFeatures,
  });
  List<String>? enabledFeatures = [];

  factory ApplicationGlobalFeatureConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationGlobalFeatureConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationGlobalFeatureConfigurationDtoToJson(this);
}

@JsonSerializable()
class ApplicationFeatureConfigurationDto {
  ApplicationFeatureConfigurationDto({
    this.values,
  });
  Map<String, String?>? values = {};

  factory ApplicationFeatureConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationFeatureConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationFeatureConfigurationDtoToJson(this);
}

@JsonSerializable()
class CurrentUserDto {
  CurrentUserDto({
    this.isAuthenticated,
    this.id,
    this.tenantId,
    this.impersonatorUserId,
    this.impersonatorTenantId,
    this.impersonatorUserName,
    this.impersonatorTenantName,
    this.userName,
    this.name,
    this.surName,
    this.email,
    this.emailVerified,
    this.phoneNumber,
    this.phoneNumberVerified,
    this.roles,
  });
  bool? isAuthenticated;
  String? id;
  String? tenantId;
  String? impersonatorUserId;
  String? impersonatorTenantId;
  String? impersonatorUserName;
  String? impersonatorTenantName;
  String? userName;
  String? name;
  String? surName;
  String? email;
  bool? emailVerified;
  String? phoneNumber;
  bool? phoneNumberVerified;
  List<String>? roles = [];

  factory CurrentUserDto.fromJson(Map<String, dynamic> json) => _$CurrentUserDtoFromJson(json);
  Map<String, dynamic> toJson() => _$CurrentUserDtoToJson(this);
}

@JsonSerializable()
class ApplicationSettingConfigurationDto {
  ApplicationSettingConfigurationDto({
    this.values,
  });
  Map<String, String?>? values = {};

  factory ApplicationSettingConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationSettingConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationSettingConfigurationDtoToJson(this);
}

@JsonSerializable()
class ApplicationAuthConfigurationDto {
  ApplicationAuthConfigurationDto({
    this.grantedPolicies,
  });
  Map<String, bool>? grantedPolicies = {};

  factory ApplicationAuthConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationAuthConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationAuthConfigurationDtoToJson(this);
}

@JsonSerializable()
class ApplicationLocalizationConfigurationDto {
  ApplicationLocalizationConfigurationDto({
    this.values,
    this.resources,
    this.languages,
    this.currentCulture,
    this.defaultResourceName,
    this.languagesMap,
    this.languageFilesMap,
  });
  Map<String, Map<String, String?>>? values = {};
  Map<String, ApplicationLocalizationResourceDto>? resources = {};
  List<LanguageInfo>? languages = [];
  CurrentCultureDto? currentCulture = CurrentCultureDto();
  String? defaultResourceName = '';
  Map<String, List<StringValue>>? languagesMap = {};
  Map<String, List<StringValue>>? languageFilesMap = {};

  factory ApplicationLocalizationConfigurationDto.fromJson(Map<String, dynamic> json) => _$ApplicationLocalizationConfigurationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationLocalizationConfigurationDtoToJson(this);
}


@JsonSerializable()
class CurrentCultureDto {
  CurrentCultureDto({
    this.name,
    this.cultureName,
    this.displayName,
    this.englishName,
    this.threeLetterIsoLanguageName,
    this.twoLetterIsoLanguageName,
    this.isRightToLeft,
    this.nativeName,
    this.dateTimeFormat,
  });
  String? name;
  String? cultureName;
  String? displayName;
  String? englishName;
  String? threeLetterIsoLanguageName;
  String? twoLetterIsoLanguageName;
  bool? isRightToLeft;
  String? nativeName;
  DateTimeFormatDto? dateTimeFormat;

  factory CurrentCultureDto.fromJson(Map<String, dynamic> json) => _$CurrentCultureDtoFromJson(json);
  Map<String, dynamic> toJson() => _$CurrentCultureDtoToJson(this);
}

@JsonSerializable()
class DateTimeFormatDto {
  DateTimeFormatDto({
    this.calendarAlgorithmType,
    this.dateTimeFormatLong,
    this.shortDatePattern,
    this.fullDateTimePattern,
    this.dateSeparator,
    this.shortTimePattern,
    this.longTimePattern,
  });
  String? calendarAlgorithmType;
  String? dateTimeFormatLong;
  String? shortDatePattern;
  String? fullDateTimePattern;
  String? dateSeparator;
  String? shortTimePattern;
  String? longTimePattern;

  factory DateTimeFormatDto.fromJson(Map<String, dynamic> json) => _$DateTimeFormatDtoFromJson(json);
  Map<String, dynamic> toJson() => _$DateTimeFormatDtoToJson(this);
}

@JsonSerializable()
class ApplicationLocalizationResourceDto {
  ApplicationLocalizationResourceDto({
    this.texts,
    this.baseResources,
  });
  Map<String, String?>? texts = {};
  List<String>? baseResources = [];

  factory ApplicationLocalizationResourceDto.fromJson(Map<String, dynamic> json) => _$ApplicationLocalizationResourceDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ApplicationLocalizationResourceDtoToJson(this);
}

@JsonSerializable()
class ClockDto {
  ClockDto({
    this.kind,
  });
  String? kind;

  factory ClockDto.fromJson(Map<String, dynamic> json) => _$ClockDtoFromJson(json);
  Map<String, dynamic> toJson() => _$ClockDtoToJson(this);
}

@JsonSerializable()
class TimingDto {
  TimingDto({
    this.timeZone,
  });
  TimeZone? timeZone;

  factory TimingDto.fromJson(Map<String, dynamic> json) => _$TimingDtoFromJson(json);
  Map<String, dynamic> toJson() => _$TimingDtoToJson(this);
}

@JsonSerializable()
class TimeZone {
  TimeZone({
    this.iana,
    this.windows,
  });

  IanaTimeZone? iana;
  WindowsTimeZone? windows;

  factory TimeZone.fromJson(Map<String, dynamic> json) => _$TimeZoneFromJson(json);
  Map<String, dynamic> toJson() => _$TimeZoneToJson(this);
}

@JsonSerializable()
class WindowsTimeZone {
  WindowsTimeZone({
    this.timeZoneId,
  });

  String? timeZoneId;

  factory WindowsTimeZone.fromJson(Map<String, dynamic> json) => _$WindowsTimeZoneFromJson(json);
  Map<String, dynamic> toJson() => _$WindowsTimeZoneToJson(this);
}

@JsonSerializable()
class IanaTimeZone {
  IanaTimeZone({
    this.timeZoneName,
  });
  String? timeZoneName;

  factory IanaTimeZone.fromJson(Map<String, dynamic> json) => _$IanaTimeZoneFromJson(json);
  Map<String, dynamic> toJson() => _$IanaTimeZoneToJson(this);
}
