import 'package:flutter/material.dart';
import 'package:get/get.dart';

class QuickNavigation extends StatelessWidget {
  const QuickNavigation({
    super.key,
    this.menus = const [],
  });

  final List<Widget> menus;

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: true,
      title: Text('Label:QuickNavigation'.tr,
        style: Theme.of(context).textTheme.titleMedium,
      ),
      children: [
        SizedBox(
          height: 120,
          child: GridView.count(
            shrinkWrap: true,
            crossAxisCount: 4,
            crossAxisSpacing: 5,
            physics: const NeverScrollableScrollPhysics(),
            children: menus,
          ),
        ),
      ],
    );
  }
}