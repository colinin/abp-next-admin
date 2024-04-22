import 'package:dev_app/pages/public/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';

class MainPage extends GetView<MainController> {
  const MainPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      bottomNavigationBar: Obx(() => BottomNavigationBar(
        onTap: (int index) {
          controller.setCurrentIndex(index);
        },
        currentIndex: controller.currentIndex,
        items: [
          _bottomNavigationBarItem(Icons.home, 'Page:Home'.tr),
          _bottomNavigationBarItem(Icons.layers, 'Page:Work'.tr),
          _bottomNavigationBarItem(Icons.person, 'Page:Center'.tr),
        ],
      )),
      body: Obx(() => <Widget>[
        Container(
          alignment: Alignment.center,
          child: const HomePage(),
        ),
        Container(
          alignment: Alignment.center,
          child: const WorkPage(),
        ),
        Container(
          alignment: Alignment.center,
          child: const CenterPage(),
        ),
      ][controller.currentIndex]),
    );
  }

  BottomNavigationBarItem _bottomNavigationBarItem(IconData activeIcon, String title){
    return BottomNavigationBarItem(
      icon: Icon(activeIcon), // 图标
      activeIcon: Icon(activeIcon), // 高亮图标
      label: title, // 标题 背景色，仅在 BottomNavigatinBar 中生效，在 iOS 风格组件中无效
    );
  }
}
