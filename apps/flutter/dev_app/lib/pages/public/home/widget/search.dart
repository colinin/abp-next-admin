import 'package:core/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:platforms/modes/menu.dto.dart';

class SearchBarDelegate extends SearchDelegate<String> {
  SearchBarDelegate({
    this.menus = const [],
  });
  final List<MenuDto> menus;

  @override
  List<Widget>? buildActions(BuildContext context) {
    Widget button = IconButton(
      onPressed: () {
        close(context, "error");
      },
      icon: const Icon(Icons.clear),
    );

    return [button];
  }

  @override
  Widget? buildLeading(BuildContext context) {
    // return IconButton(
    //   onPressed: () {
    //     query = "";
    //     showSuggestions(context);
    //   },
    //   icon: AnimatedIcon(
    //     icon: AnimatedIcons.menu_arrow,
    //     progress: transitionAnimation,
    //   ),
    // );
    return null;
  }

  @override
  Widget buildResults(BuildContext context) {
    List<Widget> results = [];
    if (!query.isNullOrWhiteSpace()) {
      results = menus.where((menu) {
        if (menu.displayName.contains(query) == true) {
          return true;
        }
        if (menu.name.contains(query)) {
          return true;
        }
        return menu.description?.contains(query) == true;
      }).map((menu) {
        return Padding(
          padding: const EdgeInsets.all(10),
          child: InkWell(
            child: Text(menu.displayName),
            onTap: () {
              Get.toNamed(menu.path);
            },
          ),
        );
      }).toList();
    }
    if (results.isNotEmpty) {
      return Column(
        children: results,
      );
    }
    return const Center(
      child: Text('没有结果'),
    );
  }

  @override
  Widget buildSuggestions(BuildContext context) {
    List<String> suggestions = [];
    if (!query.isNullOrWhiteSpace()) {
      suggestions = menus.where((menu) {
        if (menu.displayName.contains(query) == true) {
          return true;
        }
        if (menu.name.contains(query)) {
          return true;
        }
        return menu.description?.contains(query) == true;
      }).map((menu) => menu.displayName).toList();
    }

    return ListView.builder(
        itemCount: suggestions.length,
        itemBuilder: (BuildContext context, int index) {
          var suggest = suggestions[index];
          return InkWell(
            child: ListTile(
              title: RichText(
                text: TextSpan(
                  children: _buildSuggestions(suggest),
                ),
              ),
            ),
            onTap: () {
              query =  suggest;
              showResults(context);
            },
          );
        },
    );
  }

  List<InlineSpan> _buildSuggestions(String suggest) {
    List<InlineSpan> suggestions = [];
    for (var char in suggest.characters) {
      if (query.contains(char)) {
        suggestions.add(
          TextSpan(
            text: char,
            style: const TextStyle(color: Colors.blue, fontWeight: FontWeight.bold),
          )
        );
      } else {
        suggestions.add(
          TextSpan(
            text: char,
            style: const TextStyle(color: Colors.grey),
          )
        );
      }
    }
    return suggestions;
  }

  @override
  ThemeData appBarTheme(BuildContext context) {
    //
    return super.appBarTheme(context);
  }
}