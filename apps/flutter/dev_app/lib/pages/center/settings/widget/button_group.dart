import 'package:components/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ButtonGroup extends StatelessWidget {
  const ButtonGroup({
    super.key,
    this.isAuthenticated,
    this.onLogout,
  });

  final bool? isAuthenticated;
  final VoidCallback? onLogout;
  
  @override
  Widget build(BuildContext context) {
    final media = MediaQuery.of(context).size;
    return Container(
      padding: const EdgeInsets.only(top: 16),
      child: Column(
        children: [
          Container(
            height: 44,
            width: media.width / 1.2,
            decoration: const BoxDecoration(
              borderRadius: BorderRadius.all(Radius.circular(22))
            ),
            child: _logoutButton(),
          ),
        ],
      ),
    );
  }

  Widget _logoutButton() {
    if (isAuthenticated == true) {
      return BottomButton(
        title: 'Label:Logout'.tr,
        onPressed: () {
          onLogout?.call();
        },
      );
    }

    return Empty.none;
  }
}