import 'package:account/pages/route.name.dart';
import 'package:components/index.dart';
import 'package:core/utils/index.dart';
import 'package:dev_app/pages/public/home/widget/search.dart';
import 'package:dev_app/pages/system/route.name.dart';
import 'package:flex_color_scheme/flex_color_scheme.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'controller.dart';

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
            ExpansionTile(
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
                    children: [
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
                ),
              ],
            ),
            ExpansionTile(
              initiallyExpanded: true,
              title: Text('Label:MyFavorite'.tr,
                style: Theme.of(context).textTheme.titleMedium,
              ),
              children: [
                GridView.builder(
                  shrinkWrap: true,
                  physics: const NeverScrollableScrollPhysics(),
                  itemCount: bloc.state.favoriteMenus.length,
                  gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: 4,
                    crossAxisSpacing: 5,
                  ),
                  itemBuilder: (BuildContext context, int index) {
                    if (index >= bloc.state.favoriteMenus.length) {
                      return Empty.none;
                    }
                    var favoriteMenu = bloc.state.favoriteMenus[index];
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
                ),
              ],
            ),
          ],
      ),
      drawer: SafeArea(
        child: Container(
          width: 260,
          color: const Color.fromARGB(255, 44, 115, 141),
          child: Column(
            children: [
              Container(
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
              ),
              Expanded(
                child: Obx(() => Navigation(
                  activedMenu: bloc.state.activedMenu,
                  menus: bloc.state.getMenus(),
                  onMenuExpanded: bloc.onMenuExpanded,
                )),
              ),
            ],
          ),
        )
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