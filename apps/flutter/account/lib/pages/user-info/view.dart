import 'package:flutter/material.dart';
import 'package:components/index.dart';
import 'package:get/get.dart';
import 'controller.dart';

class UserInfoPage extends GetView<UserInfoController> {
  const UserInfoPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Page:UserProfile'.tr),
      ),
      body: Container(
        padding: const EdgeInsets.only(top: 12),
        child: Column(
          children: [
            /// 主题设置
            _actionButton(
              title: 'Avatar'.tr,
              suffix: Obx(() => Avatar(
                url: controller.state.profile?.avatarUrl,
                hintText: controller.state.profile?.userName,
                takeToken: controller.token?.accessToken,
              )),
            ),
            /// 账号设置
            _actionButton(
              title: 'DisplayName:UserName'.tr,
              suffix: Obx(() => Text(controller.state.profile?.userName ?? '')),
            ),
            _actionButton(
              title: 'DisplayName:Email'.tr,
              suffix: Obx(() => Text(controller.state.profile?.email ?? '')),
            ),
            _actionButton(
              title: 'DisplayName:PhoneNumber'.tr,
              suffix: Obx(() => Text(controller.state.profile?.phoneNumber ?? '')),
            ),
          ],
        ),
      ),
    );
  }

  Widget _actionButton(
    {
      required String title,
      Widget? suffix,
      VoidCallback? onTap,
    }
  ) {
    return Container(
      height: 50,
      alignment: Alignment.center,
      child: ActionButton(
        title: title,
        onTap: onTap,
        suffix: suffix,
      ),
    );
  }
}