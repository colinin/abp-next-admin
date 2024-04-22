import 'package:components/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'controller.dart';
import './widget/index.dart';

class SystemSettingsPage extends GetView<SystemSettingsController> {
  const SystemSettingsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Label:SystemSettings'.tr),
      ),
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 16),
          child: Column(
            children: [
              Expanded(
                child: Form(
                  key: controller.formKey,
                  child: Column(
                    children: <Widget> [
                      Obx(() => LanguageCard(
                        defaultLanguage: controller.state.language,
                        languages: controller.state.languages,
                        onChange: controller.onLanguageChange,
                      )),
                    ],
                  ),
                ),
              ),
              BottomButton(
                title: 'Label:Submit'.tr,
                onPressed: controller.submit,
                titleStyle: const TextStyle(
                  letterSpacing: 2,
                ),
              ),
            ],
          ),
        )),
    );
  }
}