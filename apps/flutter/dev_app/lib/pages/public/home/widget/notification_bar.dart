import 'package:bruno/bruno.dart';
import 'package:components/widgets/empty/index.dart';
import 'package:flutter/material.dart';
import 'package:notifications/models/common.dart';
import 'package:notifications/models/notification.dart';

class NotificationBar extends StatelessWidget {
  const NotificationBar({
    super.key,
    required this.notifications
  });

  final List<NotificationPaylod> notifications;

  @override
  Widget build(BuildContext context) {
    if (notifications.isEmpty) {
      return Empty.none;
    }
    return SizedBox(
      height: 40,
      child: SingleChildScrollView(
        child: Column(
          children: notifications.map<BrnNoticeBar>((payload) {
              return BrnNoticeBar(
                padding: const EdgeInsets.only(left: 5, right: 5, top: 3),
                leftWidget: Image.asset(
                  'res/images/notification.png',
                  height: 30,
                  width: 30,
                ),
                content: payload.title,
                marquee: true,
                noticeStyle: _mapNoticeStyles(payload.severity),
              );
            }).toList(),
        ),
      ),
    );
  }

  NoticeStyle _mapNoticeStyles(NotificationSeverity? severity) {
    if (severity == null) return NoticeStyles.normalNoticeWithArrow;
    switch (severity) {
      case NotificationSeverity.info:
      case NotificationSeverity.success:
        return NoticeStyles.succeedWithArrow;
      case NotificationSeverity.fatal:
      case NotificationSeverity.error:
        return NoticeStyles.failWithArrow;
      case NotificationSeverity.warn:
        return NoticeStyles.warningWithArrow;
    }
  }
}