
import 'package:components/pages/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';
import './widget/notifier_card.dart';

class NotifierManagePage extends BasePage<NotifierManageController> {
  const NotifierManagePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Page:Notification'.tr),
      ),
      body: SingleChildScrollView(
        child: Column(
          children: [
            Obx(() => SwitchListTile(
              title: Text('Label:EnableNotificationTitle'.tr),
              subtitle: Text('Label:EnableNotificationSubTitle'.tr),
              value: bloc.state.isEnabled,
              onChanged: bloc.onNotificationEnabled,
            )),
            ExpansionTile(
              title: Text('Label:NotificationsTitle'.tr),
              subtitle: Text('Label:NotificationsSubTitle'.tr),
              children: [
                Obx(() => NotifierCard(
                  groups: bloc.state.groups,
                  onChange: bloc.onSubscribed,
                )),
              ],
            ),
          ],
        ),
      ),
    );
  }
}