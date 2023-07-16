class NotificationTokens {
  /// 通知生产者
  static const String producer = "producer";
  /// 通知消费者
  static const String consumer = "consumer";
  /// 通知订阅者（接收通知数据名称, 与Hub方法同名）, 定义便于订阅/取消
  static const String receiver = "get-notification";
}