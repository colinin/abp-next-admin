import 'dart:convert';

import 'package:core/services/session.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/services/signalr.service.dart';
import 'package:core/services/storage.service.dart';
import 'package:core/utils/internal.store.dart';
import 'package:notifications/models/notification.state.dart';

import '../tokens/notifications.token.dart';
import 'notification.service.dart';

class NotificationStateService extends ServiceBase {
  static const String configKey = '_abp_notification_';
  SessionService get _sessionService => find();
  NotificationService get _notificationService => find();
  StorageService get _storageService => find();
  SignalrService get _signalrService => find(tag: NotificationTokens.producer);

  final InternalStore<NotificationState> _store = InternalStore<NotificationState>(
    state: _initState()
  );

  NotificationState get state => _store.state;

  void subscribeAll(bool isEnabled) {
    if (isEnabled) {
      _signalrService.subscribe(NotificationTokens.receiver);
    } else {
      _signalrService.unsubscribe(NotificationTokens.receiver);
    }
    _store.patch((val) {
      val.isEnabled = isEnabled;
    });
  }

  Stream<NotificationState> getNotificationState$() {
    return _store.sliceUpdate((state) => state);
  }

  NotificationGroup? findGroup(String name) {
    return _store.state.findGroup(name);
  }

  Notification? findNotification(String name) {
    return _store.state.findNotification(name);
  }

  Future<void> subscribe(Notification notification, bool isSubscribe) async {
    if (isSubscribe) {
      await _notificationService.subscribe(notification.name);
    } else {
      await _notificationService.unSubscribe(notification.name);
    }
    _changeSubscribedState(notification, isSubscribe);
  }

  @override
  void onInit() {
    super.onInit();
    _sessionService.getProfile$()
      .listen((profile) async {
        if (_sessionService.isAuthenticated) {
          await _refreshState();
        }
      });
    var notification$ = _store.sliceUpdate((state) => state);
    notification$.listen((notification) {
      _storageService.setItem(configKey, jsonEncode(notification.toJson()));
    });
  }

  static NotificationState _initState() {
    var configState = StorageService.initStorage(configKey,
      (value) => NotificationState.fromJson(jsonDecode(value)));
    return configState ?? NotificationState(isEnabled: true, groups: []);
  }

  Future<void> _refreshState() async {
    var notifiers = await _notificationService.getAssignableNotifiersAsync();
    var subscres = await _notificationService.getMySubscribedListAsync();
    List<NotificationGroup> groups = [];

    for (var notifierGroup in notifiers.items) {
      if (notifierGroup.notifications == null && notifierGroup.notifications?.isEmpty == true) {
        continue;
      }
      
      List<Notification> notifications = [];
      for (var notifier in notifierGroup.notifications!) {
        notifications.add(Notification(
          name: notifier.name,
          groupName: notifierGroup.name,
          displayName: notifier.displayName ?? notifier.name,
          description: notifier.description,
          type: notifier.type,
          contentType: notifier.contentType,
          lifetime: notifier.lifetime,
          isSubscribed: subscres.items.any((item) => item.name == notifier.name),
        ));
      }

      groups.add(NotificationGroup(
        name: notifierGroup.name,
        displayName: notifierGroup.displayName ?? notifierGroup.name,
        notifications: notifications,
      ));
    }

    _store.patch((state) {
      state.groups = groups;
    });
  }

  void _changeSubscribedState(Notification notification, bool isSubscribe) {
    _store.patch((val) {
      var group = val.findGroup(notification.groupName);
      if (group == null) return;
      var findNotification = group.find(notification.name);
      if (findNotification == null) return;
      findNotification.isSubscribed = true;
    });
  }
}