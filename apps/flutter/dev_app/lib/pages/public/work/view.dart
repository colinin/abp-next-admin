import 'package:flutter/material.dart';

import 'controller.dart';
import 'package:get/get.dart';

class WorkPage extends GetView<WorkController> {
  const WorkPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text('Page:Work'.tr),
    );
  }
}