import 'package:core/services/service.base.dart';

abstract class NotificationSendService extends ServiceBase {
  NotificationSendService(super._injector);
  Future<void> send(String title, [String? body, String? payload]);
}