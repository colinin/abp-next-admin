// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'notification.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

NotificationData _$NotificationDataFromJson(Map<String, dynamic> json) =>
    NotificationData(
      type: json['type'] as String,
      extraProperties: json['extraProperties'] as Map<String, dynamic>,
    );

Map<String, dynamic> _$NotificationDataToJson(NotificationData instance) =>
    <String, dynamic>{
      'type': instance.type,
      'extraProperties': instance.extraProperties,
    };

NotificationInfo _$NotificationInfoFromJson(Map<String, dynamic> json) =>
    NotificationInfo(
      tenantId: json['tenantId'] as String?,
      id: json['id'] as String,
      name: json['name'] as String,
      creationTime: DateTime.parse(json['creationTime'] as String),
      lifetime: $enumDecode(_$NotificationLifetimeEnumMap, json['lifetime']),
      type: $enumDecode(_$NotificationTypeEnumMap, json['type']),
      contentType:
          $enumDecode(_$NotificationContentTypeEnumMap, json['contentType']),
      severity: $enumDecode(_$NotificationSeverityEnumMap, json['severity']),
      data: NotificationData.fromJson(json['data'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$NotificationInfoToJson(NotificationInfo instance) =>
    <String, dynamic>{
      'tenantId': instance.tenantId,
      'name': instance.name,
      'id': instance.id,
      'creationTime': instance.creationTime.toIso8601String(),
      'lifetime': _$NotificationLifetimeEnumMap[instance.lifetime]!,
      'type': _$NotificationTypeEnumMap[instance.type]!,
      'contentType': _$NotificationContentTypeEnumMap[instance.contentType]!,
      'severity': _$NotificationSeverityEnumMap[instance.severity]!,
      'data': instance.data,
    };

const _$NotificationLifetimeEnumMap = {
  NotificationLifetime.persistent: 0,
  NotificationLifetime.onlyOne: 1,
};

const _$NotificationTypeEnumMap = {
  NotificationType.application: 0,
  NotificationType.system: 10,
  NotificationType.user: 20,
  NotificationType.serviceCallback: 30,
};

const _$NotificationContentTypeEnumMap = {
  NotificationContentType.text: 0,
  NotificationContentType.html: 1,
  NotificationContentType.markdown: 2,
  NotificationContentType.json: 3,
};

const _$NotificationSeverityEnumMap = {
  NotificationSeverity.success: 0,
  NotificationSeverity.info: 10,
  NotificationSeverity.warn: 20,
  NotificationSeverity.error: 30,
  NotificationSeverity.fatal: 40,
};

NotificationSendDto _$NotificationSendDtoFromJson(Map<String, dynamic> json) =>
    NotificationSendDto(
      name: json['name'] as String,
      templateName: json['templateName'] as String?,
      data: json['data'] as Map<String, dynamic>,
      culture: json['culture'] as String?,
      toUserId: json['toUserId'] as String?,
      toUserName: json['toUserName'] as String?,
      severity:
          $enumDecodeNullable(_$NotificationSeverityEnumMap, json['severity']),
    );

Map<String, dynamic> _$NotificationSendDtoToJson(
        NotificationSendDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'templateName': instance.templateName,
      'data': instance.data,
      'culture': instance.culture,
      'toUserId': instance.toUserId,
      'toUserName': instance.toUserName,
      'severity': _$NotificationSeverityEnumMap[instance.severity],
    };

NotificationDto _$NotificationDtoFromJson(Map<String, dynamic> json) =>
    NotificationDto(
      name: json['name'] as String,
      displayName: json['displayName'] as String?,
      description: json['description'] as String?,
      lifetime: $enumDecode(_$NotificationLifetimeEnumMap, json['lifetime']),
      type: $enumDecode(_$NotificationTypeEnumMap, json['type']),
      contentType:
          $enumDecode(_$NotificationContentTypeEnumMap, json['contentType']),
    );

Map<String, dynamic> _$NotificationDtoToJson(NotificationDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'displayName': instance.displayName,
      'description': instance.description,
      'lifetime': _$NotificationLifetimeEnumMap[instance.lifetime]!,
      'type': _$NotificationTypeEnumMap[instance.type]!,
      'contentType': _$NotificationContentTypeEnumMap[instance.contentType]!,
    };

NotificationGroupDto _$NotificationGroupDtoFromJson(
        Map<String, dynamic> json) =>
    NotificationGroupDto(
      name: json['name'] as String,
      displayName: json['displayName'] as String?,
      notifications: (json['notifications'] as List<dynamic>?)
          ?.map((e) => NotificationDto.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$NotificationGroupDtoToJson(
        NotificationGroupDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'displayName': instance.displayName,
      'notifications': instance.notifications,
    };

NotificationTemplateDto _$NotificationTemplateDtoFromJson(
        Map<String, dynamic> json) =>
    NotificationTemplateDto(
      name: json['name'] as String,
      description: json['description'] as String?,
      title: json['title'] as String?,
      content: json['content'] as String?,
      culture: json['culture'] as String?,
    );

Map<String, dynamic> _$NotificationTemplateDtoToJson(
        NotificationTemplateDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'description': instance.description,
      'title': instance.title,
      'content': instance.content,
      'culture': instance.culture,
    };

UserSubscreNotificationDto _$UserSubscreNotificationDtoFromJson(
        Map<String, dynamic> json) =>
    UserSubscreNotificationDto(
      name: json['name'] as String,
    );

Map<String, dynamic> _$UserSubscreNotificationDtoToJson(
        UserSubscreNotificationDto instance) =>
    <String, dynamic>{
      'name': instance.name,
    };

UserNotificationDto _$UserNotificationDtoFromJson(Map<String, dynamic> json) =>
    UserNotificationDto(
      name: json['name'] as String,
      id: json['id'] as String,
      data: NotificationData.fromJson(json['data'] as Map<String, dynamic>),
      creationTime: DateTime.parse(json['creationTime'] as String),
      type: $enumDecode(_$NotificationTypeEnumMap, json['type']),
      severity: $enumDecode(_$NotificationSeverityEnumMap, json['severity']),
      state: $enumDecode(_$NotificationReadStateEnumMap, json['state']),
      contentType: json['contentType'] == null
          ? null
          : $enumDecode(_$NotificationContentTypeEnumMap, json['contentType']),
    );

Map<String, dynamic> _$UserNotificationDtoToJson(
        UserNotificationDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'id': instance.id,
      'data': instance.data,
      'creationTime': instance.creationTime.toIso8601String(),
      'type': _$NotificationTypeEnumMap[instance.type]!,
      'severity': _$NotificationSeverityEnumMap[instance.severity]!,
      'state': _$NotificationReadStateEnumMap[instance.state]!,
      'contentType': _$NotificationContentTypeEnumMap[instance.contentType],
    };

const _$NotificationReadStateEnumMap = {
  NotificationReadState.read: 0,
  NotificationReadState.unRead: 1,
};
