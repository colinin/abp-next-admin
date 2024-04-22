

import 'package:components/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ActionCard extends StatelessWidget {
  const ActionCard({
    super.key,
    this.onSwitchTheme,
    this.onAccountSettings,
    this.onSystemSettings,
    this.onNotifierSettings,
    this.onCleanCache,
  });

  final VoidCallback? onSwitchTheme;
  final VoidCallback? onAccountSettings;
  final VoidCallback? onSystemSettings;
  final VoidCallback? onNotifierSettings;
  final VoidCallback? onCleanCache;
  
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.only(top: 12),
      child: Column(
        children: [
          /// 主题设置
          _actionButton(
            title: 'Label:SwitchTheme'.tr,
            onTap: onSwitchTheme,
          ),
          /// 账号设置
          _actionButton(
            title: 'Label:AccountSettings'.tr,
            onTap: onAccountSettings,
          ),
          _actionButton(
            title: 'Label:SystemSettings'.tr,
            onTap: onSystemSettings,
          ),
          _actionButton(
            title: 'Label:NotifierSettings'.tr,
            onTap: onNotifierSettings,
          ),
          _actionButton(
            title: 'Label:CleanCache'.tr,
            onTap: onCleanCache,
          ),
        ],
      ),
    );
  }

  Widget _actionButton(
    {
      required String title,
      IconData? prefixIcon,
      Color? prefixIconColor,
      VoidCallback? onTap,
    }
  ) {
    return Container(
      height: 50,
      alignment: Alignment.center,
      child: ActionButton(
        title: title,
        onTap: onTap,
        prefixIcon: prefixIcon,
        prefixIconColor: prefixIconColor,
      ),
    );
  }
}