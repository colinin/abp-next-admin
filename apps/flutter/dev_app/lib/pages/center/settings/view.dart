import 'package:dev_app/pages/system/route.name.dart';
import 'package:account/pages/route.name.dart';
import 'package:notifications/pages/route.name.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';
import 'widget/index.dart';

class CenterSettingsPage extends GetView<CenterSettingsController> {
  const CenterSettingsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Center:Settings'.tr),
      ),
      body: ListView(
        children: <Widget> [
          ActionCard(
            onSwitchTheme: () {
              controller.showThemeModalPicker(context);
            },
            onNotifierSettings: () {
              controller.navigateTo(NotificationRoutes.manageNotifier);
            },
            onSystemSettings: () {
              controller.navigateTo(SystemRoutes.settings);
            },
            onAccountSettings: () {
              controller.navigateTo(AccountRoutes.profile);
            },
          ),
          Obx(() => ButtonGroup(
            isAuthenticated: controller.state.isAuthenticated,
            onLogout: controller.logout,
          )),
        ],
      ),
    );
  }
}