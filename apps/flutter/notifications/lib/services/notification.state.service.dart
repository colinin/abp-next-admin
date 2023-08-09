import 'dart:convert';
import 'package:notifications/models/common.dart';
import 'package:rxdart/rxdart.dart' hide Notification;
import 'package:notifications/models/notification.dart';
import 'package:core/services/session.service.dart';
import 'package:core/services/service.base.dart';
import 'package:core/abstracts/signalr.service.dart';
import 'package:core/services/storage.service.dart';
import 'package:core/utils/internal.store.dart';
import 'package:notifications/models/notification.state.dart';

import '../tokens/notifications.token.dart';
import '../proxy/notification.service.dart';

class NotificationStateService extends ServiceBase {
  NotificationStateService(super._injector);

  static const String configKey = '_abp_notification_';
  SessionService get _sessionService => resolve<SessionService>();
  StorageService get _storageService => resolve<StorageService>();
  NotificationService get _notificationService => resolve<NotificationService>();
  SignalrService get _signalrService => resolve<SignalrService>(tag: NotificationTokens.producer);

  final BehaviorSubject<NotificationPaylod> _notifications = BehaviorSubject<NotificationPaylod>();

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

  Stream<NotificationPaylod> getNotifications$() {
    return _notifications;
  }

  NotificationGroup? findGroup(String name) {
    return _store.state.findGroup(name);
  }

  Notification? findNotification(String name) {
    return _store.state.findNotification(name);
  }

  @override
  void onInit() {
    super.onInit();
    _sessionService.onLanguageChange$()
      .switchMap((value) => _sessionService.getProfile$())
      .listen((profile) async {
        if (profile == null) {
          _store.patch((state) {
            state.groups = [];
          });
        } else {
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

  void addNotification(NotificationInfo notification) {
    _notifications.add(NotificationPaylod.fromNotification(notification));
  }

  Future<List<NotificationGroup>> getGroupAndCombineWithNotification(List<NotificationGroupDto> groupItems) {
    return _notificationService.getMySubscribedListAsync()
      .then((subscres) {
        List<NotificationGroup> groups = [];
        for (var notifierGroup in groupItems) {
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
        return groups;
      });
  }

  Future<void> _refreshState() async {
    var dto = await _notificationService.getAssignableNotifiersAsync();
    var groups = await getGroupAndCombineWithNotification(dto.items);
    _store.patch((state) {
      state.groups = groups;
    });
  }

  void changeSubscribedState(Notification notification, bool isSubscribe) {
    _store.patch((val) {
      var group = val.findGroup(notification.groupName);
      if (group == null) return;
      var findNotification = group.find(notification.name);
      if (findNotification == null) return;
      findNotification.isSubscribed = isSubscribe;
    });
  }

  void changeLoadingState(Notification notification, bool loading) {
    _store.patch((val) {
      var group = val.findGroup(notification.groupName);
      if (group == null) return;
      var findNotification = group.find(notification.name);
      if (findNotification == null) return;
      findNotification.loading = loading;
    });
  }
}