import 'package:flutter/material.dart';

import 'controller.dart';
import 'package:get/get.dart';

class HomePage extends GetView<HomeController> {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text('Page:Home'.tr),
    );
  }
}