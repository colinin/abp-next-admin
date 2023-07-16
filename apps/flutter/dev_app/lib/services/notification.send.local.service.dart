import 'dart:async';
import 'package:core/config/index.dart';
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
  final EnvConfig _env = Environment.current;

  Future<void> initAsync() async {
    const initializationSettingsAndroid = AndroidInitializationSettings('@mipmap/logo');
    var initializationSettingsLinux = LinuxInitializationSettings(
      defaultActionName: _env.notifications?.linux?.defaultActionName ?? 'Open notification',
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
    
    await _flutterLocalNotificationsPlugin.show(
      nid.value, title, body, _buildDetails(), payload: payload);
  }

  NotificationDetails _buildDetails() {
    var androidDetails = AndroidNotificationDetails(
      _env.notifications?.android?.channelId ?? 'abp-flutter', 
      _env.notifications?.android?.channelName ?? 'abp-flutter',
      channelDescription: _env.notifications?.android?.channelDescription);
    
    const darwinDetails = DarwinNotificationDetails();

    const linuxDetails = LinuxNotificationDetails();

    var details = NotificationDetails(
      android: androidDetails,
      iOS: darwinDetails,
      macOS: darwinDetails,
      linux: linuxDetails,
    );

    return details;
  }
}