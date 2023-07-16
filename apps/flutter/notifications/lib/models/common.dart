import 'dart:convert';

import 'package:core/models/abp.dto.dart';
import 'package:core/utils/localization.utils.dart';

import 'notification.dart';

class NotificationPaylod {
  NotificationPaylod({
    this.id,
    required this.title,
    required this.body,
    this.payload,
  });
  int? id;
  String title;
  String body;
  String? payload;

  factory NotificationPaylod.fromData(NotificationData data) {
    if (data.extraProperties['L'] == true) {
      var title = LocalizableStringInfo.fromJson(data.extraProperties['title']);
      var message = LocalizableStringInfo.fromJson(data.extraProperties['message']);
      return NotificationPaylod(
        title: title.name.trFormat(title.values?.map((key, value) => MapEntry(key, value?.toString() ?? '')) ?? {}),
        body: message.name.trFormat(message.values?.map((key, value) => MapEntry(key, value?.toString() ?? '')) ?? {}),
        payload: jsonEncode(data.extraProperties),
      );
    }
    return NotificationPaylod(
      title: data.extraProperties['title'] as String,
      body: data.extraProperties['message'] as String,
      payload: jsonEncode(data.extraProperties),
    );
  }
}