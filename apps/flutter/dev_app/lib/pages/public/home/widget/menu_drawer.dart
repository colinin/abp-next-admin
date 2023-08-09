import 'package:components/widgets/menu/index.dart';
import 'package:core/models/common.dart';
import 'package:flutter/material.dart';

class MenuDrawer extends StatelessWidget {
  const MenuDrawer({
    super.key,
    this.activedMenu,
    this.menus = const [],
    required this.onMenuRefresh,
    this.onMenuExpanded,
  });

  final String? activedMenu;
  final List<Menu> menus;
  final void Function(Menu menu)? onMenuExpanded;
  final Future<void> Function() onMenuRefresh;

  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: onMenuRefresh,
      child: Drawer(
        width: 260,
        child: Column(
            children: [
              _buildLogo(),
              Expanded(
                child: SingleChildScrollView(
                  physics: const AlwaysScrollableScrollPhysics(),
                  child: Column(
                    children: [
                      Navigation(
                        activedMenu: activedMenu,
                        menus: menus,
                        onMenuExpanded: onMenuExpanded,
                      ),
                    ],
                  ),
                )
              ),
            ],
        ),
      ),
    );
  }

  Widget _buildLogo() {
    return Container(
      height: 24,
      margin: const EdgeInsets.all(10),
      child: Row(
        children: [
          Padding(
            padding: const EdgeInsets.only(left: 10),
            child: Image.asset(
              'res/images/logo.png',
              height: 20,
              width: 20,
            ),
          ),
          const Padding(
            padding: EdgeInsets.only(left: 10),
            child: Text(
              'abp flutter',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.w400,
              ),
            ),
          ),
        ],
      ),
    );
  }
}