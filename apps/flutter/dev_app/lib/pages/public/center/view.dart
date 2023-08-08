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
                state: controller.state,
                onTap: controller.onClickProfile,
              )),
            ),
          ],
        ),
        Container(
          margin: const EdgeInsets.only(top: 10, left: 20, right: 20),
          child: ActionCard(
          onHelp: controller.onClickHelp,
          onFeedback: controller.onClickFeedback,
          onMessages: controller.onClickMessage,
          onSettings: controller.onSettings,
          onInfo: controller.onClickInfo,
        )),
      ],
    );
  }
}