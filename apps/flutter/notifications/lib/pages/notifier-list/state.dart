import 'package:notifications/models/index.dart';

class NotificationsState {
  NotificationsState({
    this.notifications = const [],
    this.hasMore = true,
    this.skipCount = 0,
    this.maxResultCount = 10,
  });
  bool hasMore;
  int skipCount;
  int maxResultCount;
  List<UserNotificationDto> notifications;
}