import 'package:components/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ActionCard extends StatelessWidget {
  const ActionCard({
    super.key,
    this.onHelp,
    this.onFeedback,
    this.onSettings,
    this.onMessages,
    this.onInfo,
  });

  final VoidCallback? onHelp;
  final VoidCallback? onFeedback;
  final VoidCallback? onSettings;
  final VoidCallback? onMessages;
  final VoidCallback? onInfo;
  
  @override
  Widget build(BuildContext context) {
    return Card(
      child: Column(
        children: [
          _actionButton(
            title: 'Center:Help'.tr,
            prefixIcon: Icons.help,
            prefixIconColor: Colors.blue,
            onTap: onHelp,
          ),
          _actionButton(
            title: 'Center:Feedback'.tr,
            prefixIcon: Icons.feedback,
            prefixIconColor: Colors.orange,
            onTap: onFeedback,
          ),
          _actionButton(
            title: 'Center:Messages'.tr,
            prefixIcon: Icons.message,
            prefixIconColor: Colors.cyan,
            onTap: onMessages,
          ),
          _actionButton(
            title: 'Center:Settings'.tr,
            prefixIcon: Icons.settings,
            prefixIconColor: Colors.red,
            onTap: onSettings,
          ),
          _actionButton(
            title: 'Center:Info'.tr,
            prefixIcon: Icons.info,
            prefixIconColor: Colors.greenAccent,
            onTap: onInfo,
          ),
        ],
      ),
    );
  }

  Widget _actionButton(
    {
      required String title,
      required IconData prefixIcon,
      required Color prefixIconColor,
      VoidCallback? onTap,
    }
  ) {
    return Container(
      height: 50,
      alignment: Alignment.center,
      margin: const EdgeInsets.only(left: 10, right: 10, top: 10),
      child: ActionButton(
        title: title,
        prefixIcon: prefixIcon,
        prefixIconColor: prefixIconColor,
        onTap: onTap,
      ),
    );
  }
}