
import 'package:get/get.dart';

import '../../models/notification.dart';

class NotifierManageState {
  NotifierManageState({
    required this.isEnabled,
    required this.groups,
  });
  bool isEnabled;
  List<NotificationGroup> groups;

  NotificationGroup? find(String name) {
    return groups.firstWhereOrNull((item) => item.name == name);
  }
}

class NotificationGroup {
  NotificationGroup({
    required this.name,
    required this.displayName,
    required this.notifications,
  });
  String name;
  String displayName;
  List<Notification> notifications = [];

  Notification? find(String name) {
    return notifications.firstWhereOrNull((item) => item.name == name);
  }
}

class Notification {
  Notification({
    required this.name,
    required this.groupName,
    required this.displayName,
    required this.isSubscribed,
    this.description,
    required this.lifetime,
    required this.type,
    required this.contentType,
  });
  String name;
  String groupName;
  String displayName;
  bool isSubscribed;
  String? description;
  NotificationLifetime lifetime;
  NotificationType type;
  NotificationContentType contentType;
}
