import 'package:core/models/common.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class Navigation extends StatefulWidget {
  const Navigation({
    super.key,
    this.activedMenu,
    this.menus = const [],
    this.onMenuExpanded,
  });

  final List<Menu> menus;
  final String? activedMenu;
  final void Function(Menu menu)? onMenuExpanded;

  @override
  State<Navigation> createState() => _NavigationState();
}

class _NavigationState extends State<Navigation> {
  @override
  Widget build(BuildContext context) {
    return _renderNavigations(widget.menus);
  }

  Widget _renderNavigations(List<Menu> menus) {
    var mainMenus = menus.where((menu) => menu.children?.isNotEmpty == false);
    var subMenus = menus.where((menu) => menu.children?.isNotEmpty == true);

    return Column(
      children: [
        _renderMenus(subMenus.toList()),
        ...mainMenus.map((menu) => _buildMenuItem(menu)),
      ],
    );
  }

  Widget _renderMenus(List<Menu> menus) {
    return ExpansionPanelList.radio(
      initialOpenPanelValue: widget.activedMenu,
      expandedHeaderPadding: const EdgeInsets.all(0),
      expansionCallback: (panelIndex, isExpanded) {
        if (widget.onMenuExpanded != null) {
          widget.onMenuExpanded!(menus[panelIndex]);
        }
      },
      children: menus.map<ExpansionPanelRadio>((Menu menu) {
        var body = menu.children?.isNotEmpty == true
          ? _renderNavigations(menu.children!)
          : _buildMenuItem(menu);
        return ExpansionPanelRadio(
          canTapOnHeader: true,
          headerBuilder: (BuildContext context, bool isExpanded) {
            return SizedBox(
              height: 30,
              child: ListTile(
                title: Text(
                  (menu.meta?['displayName']?.toString().tr ?? menu.displayName).padLeft(menu.level * 4)),
              ),
            );
          },
          body: body,
          value: menu.name,
        );
      }).toList(),
    );
  }

  Widget _buildMenuItem(Menu menu) {
    return InkWell(
      onTap: () {
        Get.toNamed(menu.path);
      },
      child: FractionallySizedBox(
        widthFactor: 1,
        child: Container(
          height: 30,
          margin: const EdgeInsets.only(top: 10),
          child: Text(
            (menu.meta?['displayName']?.toString().tr ?? menu.displayName).padLeft(menu.level * 8),
          ),
        ),
      ),
    );
  }
}