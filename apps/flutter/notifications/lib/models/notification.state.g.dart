// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'notification.state.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

NotificationState _$NotificationStateFromJson(Map<String, dynamic> json) =>
    NotificationState(
      isEnabled: json['isEnabled'] as bool,
      groups: (json['groups'] as List<dynamic>)
          .map((e) => NotificationGroup.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$NotificationStateToJson(NotificationState instance) =>
    <String, dynamic>{
      'isEnabled': instance.isEnabled,
      'groups': instance.groups,
    };

NotificationGroup _$NotificationGroupFromJson(Map<String, dynamic> json) =>
    NotificationGroup(
      name: json['name'] as String,
      displayName: json['displayName'] as String,
      notifications: (json['notifications'] as List<dynamic>)
          .map((e) => Notification.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$NotificationGroupToJson(NotificationGroup instance) =>
    <String, dynamic>{
      'name': instance.name,
      'displayName': instance.displayName,
      'notifications': instance.notifications,
    };

Notification _$NotificationFromJson(Map<String, dynamic> json) => Notification(
      name: json['name'] as String,
      groupName: json['groupName'] as String,
      displayName: json['displayName'] as String,
      isSubscribed: json['isSubscribed'] as bool,
      description: json['description'] as String?,
      lifetime: $enumDecode(_$NotificationLifetimeEnumMap, json['lifetime']),
      type: $enumDecode(_$NotificationTypeEnumMap, json['type']),
      contentType:
          $enumDecode(_$NotificationContentTypeEnumMap, json['contentType']),
    );

Map<String, dynamic> _$NotificationToJson(Notification instance) =>
    <String, dynamic>{
      'name': instance.name,
      'groupName': instance.groupName,
      'displayName': instance.displayName,
      'isSubscribed': instance.isSubscribed,
      'description': instance.description,
      'lifetime': _$NotificationLifetimeEnumMap[instance.lifetime]!,
      'type': _$NotificationTypeEnumMap[instance.type]!,
      'contentType': _$NotificationContentTypeEnumMap[instance.contentType]!,
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
