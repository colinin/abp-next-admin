import 'package:core/models/abp.dto.dart';
import 'package:json_annotation/json_annotation.dart';

part 'notification.g.dart';

@JsonEnum()
enum NotificationLifetime {
  @JsonValue(0)
  persistent,
  @JsonValue(1)
  onlyOne
}

@JsonEnum()
enum NotificationType {
  @JsonValue(0)
  application,
  @JsonValue(10)
  system,
  @JsonValue(20)
  user,
  @JsonValue(30)
  serviceCallback;
}

@JsonEnum()
enum NotificationContentType {
  @JsonValue(0)
  text,
  @JsonValue(1)
  html,
  @JsonValue(2)
  markdown,
  @JsonValue(3)
  json;
}

@JsonEnum()
enum NotificationSeverity {
  @JsonValue(0)
  success,
  @JsonValue(10)
  info,
  @JsonValue(20)
  warn,
  @JsonValue(30)
  error,
  @JsonValue(40)
  fatal;
}

@JsonEnum()
enum NotificationReadState {
  @JsonValue(0)
  read,
  @JsonValue(1)
  unRead
}

@JsonSerializable()
class NotificationData {
  NotificationData({
    required this.type,
    required this.extraProperties,
  });
  String type;
  Map<String, dynamic> extraProperties;

  factory NotificationData.fromJson(Map<String, dynamic> json) => _$NotificationDataFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationDataToJson(this);
}

@JsonSerializable()
class NotificationInfo {
  NotificationInfo({
    this.tenantId,
    required this.id,
    required this.name,
    required this.creationTime,
    required this.lifetime,
    required this.type,
    required this.contentType,
    required this.severity,
    required this.data,
  });
  String? tenantId;
  String name;
  String id;
  DateTime creationTime;
  NotificationLifetime lifetime;
  NotificationType type;
  NotificationContentType contentType;
  NotificationSeverity severity;
  NotificationData data;

  factory NotificationInfo.fromJson(Map<String, dynamic> json) => _$NotificationInfoFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationInfoToJson(this);
}

@JsonSerializable()
class NotificationSendDto {
  NotificationSendDto({
    required this.name,
    this.templateName,
    required this.data,
    this.culture,
    required this.toUserId,
    this.toUserName,
    this.severity,
  });
  String name;
  String? templateName;
  Map<String, dynamic> data = {};
  String? culture;
  String? toUserId;
  String? toUserName;
  NotificationSeverity? severity;
  factory NotificationSendDto.fromJson(Map<String, dynamic> json) => _$NotificationSendDtoFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationSendDtoToJson(this);
}


@JsonSerializable()
class NotificationDto {
  NotificationDto({
    required this.name,
    this.displayName,
    this.description,
    required this.lifetime,
    required this.type,
    required this.contentType,
  });
  String name;
  String? displayName;
  String? description;
  NotificationLifetime lifetime;
  NotificationType type;
  NotificationContentType contentType;
  factory NotificationDto.fromJson(Map<String, dynamic> json) => _$NotificationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationDtoToJson(this);
}


@JsonSerializable()
class NotificationGroupDto {
  NotificationGroupDto({
    required this.name,
    this.displayName,
    this.notifications,
  });
  String name;
  String? displayName;
  List<NotificationDto>? notifications = [];
  factory NotificationGroupDto.fromJson(Map<String, dynamic> json) => _$NotificationGroupDtoFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationGroupDtoToJson(this);
}


@JsonSerializable()
class NotificationTemplateDto {
  NotificationTemplateDto({
    required this.name,
    this.description,
    this.title,
    this.content,
    this.culture,
  });
  String name;
  String? description;
  String? title;
  String? content;
  String? culture;
  factory NotificationTemplateDto.fromJson(Map<String, dynamic> json) => _$NotificationTemplateDtoFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationTemplateDtoToJson(this);
}

@JsonSerializable()
class UserSubscreNotificationDto {
  UserSubscreNotificationDto({
    required this.name,
  });
  String name;
  factory UserSubscreNotificationDto.fromJson(Map<String, dynamic> json) => _$UserSubscreNotificationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$UserSubscreNotificationDtoToJson(this);
}

class SetSubscriptionDto {
  SetSubscriptionDto(this.name);
  String name;
}

@JsonSerializable()
class UserNotificationDto {
  UserNotificationDto({
    required this.name,
    required this.id,
    required this.data,
    required this.creationTime,
    required this.type,
    required this.severity,
    required this.state,
    required this.contentType,
  });
  String name;
  String id;
  NotificationData data;
  DateTime creationTime;
  NotificationType type;
  NotificationSeverity severity;
  NotificationReadState state;
  NotificationContentType? contentType;

  factory UserNotificationDto.fromJson(Map<String, dynamic> json) => _$UserNotificationDtoFromJson(json);
  Map<String, dynamic> toJson() => _$UserNotificationDtoToJson(this);
}

class UserNotificationGetByPagedDto extends PagedAndSortedResultRequestDto {
  UserNotificationGetByPagedDto({
    super.sorting,
    super.skipCount,
    super.maxResultCount,
    this.filter,
    this.readState,
  });
  String? filter;
  NotificationReadState? readState;

  @override
  Map<String, dynamic> toJson() =>
    <String, dynamic> {
      'sorting': sorting,
      'skipCount': skipCount,
      'maxResultCount': maxResultCount,
      'filter': filter,
      'readState': readState,
    };
}
