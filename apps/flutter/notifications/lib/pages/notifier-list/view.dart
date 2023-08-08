import 'package:components/index.dart';
import 'package:core/utils/date.format.utils.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:notifications/models/index.dart';
import 'controller.dart';

class NotificationsPage extends BasePage<NotificationsController> {
  const NotificationsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Page:Notifications'.tr),
      ),
      body: SafeArea(
        child: RefreshIndicator(
          onRefresh: bloc.onTopScroll,
          child: Stack(
            children: [
              Obx(() => _buildNotifications()),
              BackToTop(controller: bloc.scroll),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildNotifications() {
    return ListView.builder(
      itemCount: bloc.state.hasMore 
        ? bloc.state.notifications.length + 1
        : bloc.state.notifications.length,
      itemBuilder: (context, index) {
        if (index == bloc.state.notifications.length) {
          return Empty.none;
        }
        var notification = bloc.state.notifications[index];
        var payload = NotificationPaylod.fromDto(notification);
        return InkWell(
          onTap: () => bloc.showPayload(payload),
          child: Container(
            padding: const EdgeInsets.only(bottom: 10, left: 10, right: 10),
            child: Column(
              children: [
                Center(
                  child: Text(notification.creationTime
                    .format(['yyyy', '-', 'mm', '-', 'dd', ' ', 'HH', ':', 'nn', ':', 'ss'])),
                ),
                Card(
                  child: Column(
                    children: [
                      ListTile(
                        title: Padding(
                          padding: const EdgeInsets.only(bottom: 10),
                          child: Text(payload.title),
                        ),
                        subtitle: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text('通知来源: ${payload.formUser ?? ''}', textAlign: TextAlign.left,),
                            Text('通知类型: ${payload.title}', textAlign: TextAlign.left,),
                            Text('发布时间: ${notification.creationTime
                                .format(['yyyy', '-', 'mm', '-', 'dd', ' ', 'HH', ':', 'nn', ':', 'ss'])}',
                              textAlign: TextAlign.left
                            ),
                          ],
                        ),
                      ),
                      const Divider(),
                      const SizedBox(
                        height: 26,
                        child: Stack(
                          children: [
                            Positioned(
                              left: 10,
                              child: Center(child: Text('查看详情')),
                            ),
                            Positioned(
                              right: 10,
                              child: Center(child: Icon(Icons.arrow_forward_ios,
                                size: 13,
                                color: Colors.grey,
                              )),
                            ),
                          ],
                        ),
                      )
                    ],
                  )
                ),
              ],
            ),
          ),
        );
       },
       controller: bloc.scroll,
    );
  }
}