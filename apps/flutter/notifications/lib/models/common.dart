import 'dart:convert';

import 'package:core/models/abp.dto.dart';
import 'package:core/utils/localization.utils.dart';

import 'notification.dart';

class NotificationPaylod {
  NotificationPaylod({
    this.id,
    required this.title,
    required this.body,
    this.type,
    this.state,
    this.severity,
    this.contentType,
    this.payload,
    this.formUser,
    this.createTime,
    this.description,
  });
  int? id;
  String title;
  String body;
  String? payload;
  String? formUser;
  String? description;
  DateTime? createTime;
  NotificationType? type;
  NotificationSeverity? severity;
  NotificationReadState? state;
  NotificationContentType? contentType;

  static String _parseProp(NotificationData data, String prop) {
    if (!data.extraProperties.containsKey(prop)) return '';
    if (data.extraProperties['L'] == true) {
      var formatProp = LocalizableStringInfo.fromJson(data.extraProperties[prop]);
      return formatProp.name.trFormat(formatProp.values?.map((key, value) => MapEntry(key, value?.toString() ?? '')) ?? {});
    } else {
      return data.extraProperties[prop] as String;
    }
  }

  factory NotificationPaylod.fromDto(UserNotificationDto dto) {
    var data = dto.data;
    var title = _parseProp(data, 'title');
    var message = _parseProp(data, 'message');
    var description = _parseProp(data, 'description');
    return NotificationPaylod(
      id: int.tryParse(dto.id),
      title: title,
      body: message,
      description: description,
      payload: jsonEncode(data.extraProperties),
      formUser: data.extraProperties['formUser'] as String?,
      createTime: data.extraProperties['createTime'] != null
        ? DateTime.tryParse(data.extraProperties['createTime'] as String)
        : null,
      type: dto.type,
      state: dto.state,
      severity: dto.severity,
      contentType: dto.contentType,
    );
  }

  factory NotificationPaylod.fromNotification(NotificationInfo notification) {
    var data = notification.data;
    var title = _parseProp(data, 'title');
    var message = _parseProp(data, 'message');
    var description = _parseProp(data, 'description');
    return NotificationPaylod(
      id: int.tryParse(notification.id),
      title: title,
      body: message,
      description: description,
      payload: jsonEncode(data.extraProperties),
      formUser: data.extraProperties['formUser'] as String?,
      createTime: data.extraProperties['createTime'] != null
        ? DateTime.tryParse(data.extraProperties['createTime'] as String)
        : null,
      type: notification.type,
      severity: notification.severity,
      contentType: notification.contentType,
    );
  }
}