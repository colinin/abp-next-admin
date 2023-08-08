import 'package:components/index.dart';
import 'package:dev_app/pages/public/home/widget/search.dart';
import 'package:flutter/material.dart';

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
          child: const Row(
            children: <Widget>[
              Icon(Icons.search),
              Expanded(child: Text('搜索功能'))
            ],
          ),
        ),
      ),
      body: Column(
        children: [
          Expanded(
            child: ListView.builder(
              itemCount: bloc.state.favoriteMenus.length,
              itemBuilder: (context, index) {
                var favoriteMenu = bloc.state.favoriteMenus[index];
                return Text(favoriteMenu.displayName ?? favoriteMenu.name);
              },
            ),
          ),
        ],
      ),
    );
  }
}