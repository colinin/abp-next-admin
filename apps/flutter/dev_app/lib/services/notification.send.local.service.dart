import 'dart:async';
import 'package:get/get.dart';
import 'package:rxdart/rxdart.dart' hide Notification;

import 'package:core/models/notifications.dart';
import 'package:core/services/notification.send.service.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class FlutterLocalNotificationsSendService extends NotificationSendService {
  final RxInt nid = 0.obs;
  final FlutterLocalNotificationsPlugin _flutterLocalNotificationsPlugin = FlutterLocalNotificationsPlugin();
  final Subject<Notification> _notifications$ = BehaviorSubject<Notification>();
  final Subject<String?> _selectedNotifications$ = BehaviorSubject<String?>();

  Future<void> initAsync() async {
    const initializationSettingsAndroid = AndroidInitializationSettings('@mipmap/logo');
    const initializationSettingsLinux = LinuxInitializationSettings(
      defaultActionName: 'Open notification',
    );
    var initializationSettingsDarwin  = DarwinInitializationSettings(
      onDidReceiveLocalNotification: (id, title, body, payload) {
        _notifications$.add(Notification(id: id, title: title, body: body, payload: payload));
      },
    );
    var initializationSettings = InitializationSettings(
      android: initializationSettingsAndroid,
      iOS: initializationSettingsDarwin,
      macOS: initializationSettingsDarwin,
      linux: initializationSettingsLinux,
    );
    await _flutterLocalNotificationsPlugin.initialize(
      initializationSettings,
      onDidReceiveNotificationResponse: (notificationResponse) {
        switch (notificationResponse.notificationResponseType) {
          case NotificationResponseType.selectedNotification:
            _selectedNotifications$.add(notificationResponse.payload);
            break;
          case NotificationResponseType.selectedNotificationAction:
            _selectedNotifications$.add(notificationResponse.payload);
          break;
        }
      },
    );
  }

  @override
  Future<void> send(String title, [String? body, String? payload]) async {
    nid.value += 1;
    const androidDetails = AndroidNotificationDetails(
      'abp-flutter', 
      'abp-flutter');
    const details = NotificationDetails(
      android: androidDetails,
    );
    await _flutterLocalNotificationsPlugin.show(
      nid.value,
      title,
      body,
      details,
      payload: payload);
  }
}