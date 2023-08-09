import 'dart:async';
import 'package:core/index.dart';
import 'package:get/get.dart';
import 'package:notifications/services/notification.state.service.dart';
import 'package:rxdart/rxdart.dart' hide Notification;

import 'package:core/models/notifications.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class FlutterLocalNotificationsSendService extends NotificationSendService {
  FlutterLocalNotificationsSendService(super._injector);
  final RxInt nid = 0.obs;
  final FlutterLocalNotificationsPlugin _flutterLocalNotificationsPlugin = FlutterLocalNotificationsPlugin();
  final Subject<Notification> _notifications$ = BehaviorSubject<Notification>();
  final Subject<String?> _selectedNotifications$ = BehaviorSubject<String?>();

  EnvironmentService get _environmentService => resolve<EnvironmentService>();
  NotificationStateService get _notificationStateService => resolve<NotificationStateService>();

  @override
  void onInit() {
    super.onInit();
    _notificationStateService
      .getNotifications$()
      .listen((payload) async {
        // 发布本地通知
        await send(
          payload.title,
          payload.body,
          payload.payload,
        );
      });
  }

  Future<void> initAsync() async {
    var environment = _environmentService.getEnvironment();
    const initializationSettingsAndroid = AndroidInitializationSettings('@mipmap/logo');
    var initializationSettingsLinux = LinuxInitializationSettings(
      defaultActionName: environment.notifications?.linux?.defaultActionName ?? 'Open notification',
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
    var environment = _environmentService.getEnvironment();
    var androidDetails = AndroidNotificationDetails(
      environment.notifications?.android?.channelId ?? 'abp-flutter', 
      environment.notifications?.android?.channelName ?? 'abp-flutter',
      channelDescription: environment.notifications?.android?.channelDescription);
    
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