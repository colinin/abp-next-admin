
import 'package:get/get.dart';
import 'package:json_annotation/json_annotation.dart';

import 'notification.dart';

part 'notification.state.g.dart';

@JsonSerializable()
class NotificationState {
  NotificationState({
    required this.isEnabled,
    required this.groups,
  });
  bool isEnabled;
  List<NotificationGroup> groups;
  

  NotificationGroup? findGroup(String name) {
    return groups.firstWhereOrNull((item) => item.name == name);
  }

  Notification? findNotification(String name) {
    for (var group in groups) {
      var find = group.find(name);
      if (find != null) return find;
    }
    return null;
  }

  factory NotificationState.fromJson(Map<String, dynamic> json) => _$NotificationStateFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationStateToJson(this);
}

@JsonSerializable()
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

  factory NotificationGroup.fromJson(Map<String, dynamic> json) => _$NotificationGroupFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationGroupToJson(this);
}

@JsonSerializable()
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
    this.loading = false,
  });
  String name;
  String groupName;
  String displayName;
  bool loading;
  bool isSubscribed;
  String? description;
  NotificationLifetime lifetime;
  NotificationType type;
  NotificationContentType contentType;

  factory Notification.fromJson(Map<String, dynamic> json) => _$NotificationFromJson(json);
  Map<String, dynamic> toJson() => _$NotificationToJson(this);
}
