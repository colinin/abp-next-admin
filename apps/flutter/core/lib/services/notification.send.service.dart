import 'package:core/services/service.base.dart';

abstract class NotificationSendService extends ServiceBase {
  Future<void> send(String title, [String? body, String? payload]);
}