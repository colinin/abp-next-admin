import 'package:components/widgets/empty/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:platforms/modes/menu.dto.dart';

class MyFavorite extends StatelessWidget {
  const MyFavorite({
    super.key,
    required this.favoriteMenus,
    required this.favoriteMenuBuilder,
  });

  final List<UserFavoriteMenuDto> favoriteMenus;
  final Widget Function(UserFavoriteMenuDto favoriteMenu) favoriteMenuBuilder;
  
  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: true,
      title: Text('Label:MyFavorite'.tr,
        style: Theme.of(context).textTheme.titleMedium,
      ),
      children: [
        GridView.builder(
          shrinkWrap: true,
          physics: const NeverScrollableScrollPhysics(),
          itemCount: favoriteMenus.length,
          gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
            crossAxisCount: 4,
            crossAxisSpacing: 5,
          ),
          itemBuilder: (BuildContext context, int index) {
            if (index >= favoriteMenus.length) {
              return Empty.none;
            }
            return favoriteMenuBuilder(favoriteMenus[index]);
          },
        ),
      ],
    );
  }
}