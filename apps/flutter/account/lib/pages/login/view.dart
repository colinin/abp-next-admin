import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';
import './widget/index.dart';

class LoginPage extends GetView<LoginController> {
  const LoginPage({super.key});
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: ListView(
          children: <Widget>[
            const Logo(),
            Obx(() => LoginForm(
              state: controller.state,
              onSubmit: controller.login,
              onPwdVisiable: controller.changePwdVisiable,
            )),
            const SizedBox(height: 10),
            Obx(() => LoginExternal(
              state: controller.state,
            )),
          ],
        )),
    );
  }
}