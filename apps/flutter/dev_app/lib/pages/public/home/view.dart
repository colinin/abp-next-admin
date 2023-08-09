import 'package:account/pages/route.name.dart';
import 'package:components/index.dart';
import 'package:core/utils/index.dart';
import 'package:dev_app/pages/system/route.name.dart';
import 'package:flex_color_scheme/flex_color_scheme.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';
import './widget/index.dart';

class HomePage extends BasePage<HomeController> {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.background,
        title: TextButton(
          style: ButtonStyle(
            backgroundColor: MaterialStatePropertyAll(Theme.of(context).cardColor),
            shape: MaterialStatePropertyAll(RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)))
          ),
          onPressed: () {
            showSearch(context: context, delegate: SearchBarDelegate(menus: bloc.state.menus));
          },
          child: Row(
            children: <Widget>[
              const Icon(Icons.search),
              Expanded(child: Text('Label:SearchFeatures'.tr))
            ],
          ),
        ),
      ),
      body: ListView(
        children: [
            Obx(() => NotificationBar(notifications: bloc.state.notifications)),
            QuickNavigation(
              menus: [
                _buildMenu(
                  SystemRoutes.settings,
                  SystemRoutes.settings,
                  icon: 'res/images/setting.png',
                  displayName: "Label:SystemSettings".tr,
                  color: Colors.red.hex),
                _buildMenu(
                  AccountRoutes.profile,
                  AccountRoutes.profile,
                  icon: 'res/images/profile.png',
                  displayName: "Page:UserProfile".tr,
                  color: const Color.fromARGB(255, 68, 160, 206).hex),
                ],
            ),
            Obx(() => MyFavorite(
              favoriteMenus: bloc.state.favoriteMenus,
              favoriteMenuBuilder: (favoriteMenu) {
                return _buildMenu(
                  favoriteMenu.name,
                  favoriteMenu.path,
                  aliasName: favoriteMenu.aliasName,
                  //icon: favoriteMenu.icon,
                  // TODO: 需要各个模块自行提供本地图标
                  icon: 'res/images/setting.png',
                  color: favoriteMenu.color,
                  displayName: favoriteMenu.displayName,
                );
              },
            )),
          ],
      ),
      drawer: SafeArea(
        child: Obx(() => MenuDrawer(
          activedMenu: bloc.state.activedMenu,
          menus: bloc.state.getMenus(),
          onMenuExpanded: bloc.onMenuExpanded,
          onMenuRefresh: bloc.refreshMenus,
        )),
      ),
    );
  }

  Widget _buildMenu(
    String name,
    String path,
    {
      String? aliasName,
      String? icon,
      String? color,
      String? displayName,
    }
  ) {
    return InkWell(
      onTap: () {
        bloc.redirectToRoute(path);
      },
      child: SizedBox(
        height: 20,
        width: 30,
        child: Column(
          children: [
            const SizedBox(height: 10),
            icon != null
            ? Image.asset(
                icon,
                height: 40,
                width: 40,
                color: color.isNullOrWhiteSpace() ? null : ColorUtils.fromHex(color!),
              )
            : Empty.none,
            Text(
              displayName ?? aliasName ?? name,
              textAlign: TextAlign.center,
              style: const TextStyle(
                fontSize: 14
              ),
            )
          ],
        ),
      ),
    );
  }
}