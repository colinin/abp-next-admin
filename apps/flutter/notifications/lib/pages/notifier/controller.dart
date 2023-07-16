import 'package:core/services/index.dart';
import 'package:get/get.dart';

import '../../services/index.dart';
import '../../tokens/index.dart';
import './state.dart';

class NotifierManageController extends GetxController {
  NotificationService get notificationService => Get.find();
  SignalrService get signalrService => Get.find(tag: NotificationTokens.producer);
  SessionService get sessionService => Get.find();

  final Rx<NotifierManageState> _state = Rx<NotifierManageState>(NotifierManageState(
    isEnabled: true,
    groups: []));
  NotifierManageState get state => _state.value;

  void _initNotifierConfig() async {
    if (!sessionService.isAuthenticated) return;
    var notifiers = await notificationService.getAssignableNotifiersAsync();
    var subscres = await notificationService.getMySubscribedListAsync();

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

    _state.update((val) {
      val!.groups = groups;
    });
  }

  void onSubscribed(Notification notification, bool isSubscribe) async {
    if (!isSubscribe) {
      await notificationService.unSubscribe(notification.name);
    } else {
      await notificationService.subscribe(notification.name);
    }
    _state.update((val) {
      var group = val!.find(notification.groupName);
      if (group == null) return;
      var findNotification = group.find(notification.name);
      if (findNotification == null) return;
      findNotification.isSubscribed = isSubscribe;
    });
  }

  void onNotificationEnabled(bool isEnabled) {
    if (!isEnabled) {
      signalrService.unsubscribe(NotificationTokens.receiver);
    } else {
      signalrService.subscribe(NotificationTokens.receiver);
    }
    _state.update((val) {
      val!.isEnabled = isEnabled;
    });
  }

  @override
  void onInit() {
    super.onInit();
    _initNotifierConfig();
  }
}