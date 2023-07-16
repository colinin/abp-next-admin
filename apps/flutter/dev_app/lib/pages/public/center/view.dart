import 'package:core/services/notification.send.service.dart';
import 'package:dev_app/pages/center/route.name.dart';
import 'package:account/pages/route.name.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import './widget/index.dart';
import 'controller.dart';

class CenterPage extends GetView<CenterController> {
  const CenterPage({super.key});

  @override
  Widget build(BuildContext context) {
    return ListView(
      children: <Widget> [
        Stack(
          children: [
            const StatusBar(),
            Container(
              height: 100,
              margin: const EdgeInsets.only(top: 130, left: 20, right: 20),
              child: Obx(() => UserCard(
                avatarUrl: controller.profile?.avatarUrl,
                takeToken: controller.token?.accessToken,
                userName: controller.userName,
                phoneNumber: controller.phoneNumber,
                onTap: () {
                  if (!controller.isAuthenticated) {
                    controller.redirectToRoute(AccountRoutes.login);
                  }
                  else
                  {
                    controller.redirectToRoute(AccountRoutes.profile);
                  }
                },
              )),
            ),
          ],
        ),
        Container(
          margin: const EdgeInsets.only(top: 20, left: 20, right: 20),
          child: ActionCard(
          onHelp: () {
            //controller.redirectToRoute('/center/help');
          },
          onFeedback: () {
            //controller.redirectToRoute('/center/feedback');
          },
          onSettings: () => controller.redirectToRoute(CenterRoutes.settings),
          onInfo: () async {
            var service = Get.find<NotificationSendService>();
            await service.send('测试通知', '测试内容', '测试载体');
          },
        )),
      ],
    );
  }
}