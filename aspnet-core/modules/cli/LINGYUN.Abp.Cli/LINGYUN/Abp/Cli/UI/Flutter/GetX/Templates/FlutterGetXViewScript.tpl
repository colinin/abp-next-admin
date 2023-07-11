import 'package:flutter/material.dart';

import 'controller.dart';
import 'package:get/get.dart';

class {{ model.application }}Page extends GetView<{{ model.application }}Controller> {
  const {{ model.application }}Page({super.key});

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Text('{{ model.application }}'),
    );
  }
}