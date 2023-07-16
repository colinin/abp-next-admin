import 'package:core/proxy/volo/abp/localization/index.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class LanguageCard extends StatelessWidget {
  const LanguageCard({
    super.key,
    this.defaultLanguage,
    this.languages,
    this.onChange,
  });

  final String? defaultLanguage;
  final List<LanguageInfo>? languages;
  final void Function(String? value)? onChange;

  List<DropdownMenuItem<String>> _languageList() {
    if (languages?.isEmpty == true) return [];
    List<DropdownMenuItem<String>> languageItems = [];
    for (var language in languages!) {
      languageItems.add(
        DropdownMenuItem<String>(
          value: language.cultureName,
          enabled: language.cultureName != defaultLanguage,
          child: Text(language.displayName!),
        )
      );
    }
    return languageItems;
  }

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: true,
      title: Text('Languages'.tr,
        style: Theme.of(context).textTheme.titleMedium,
      ),
      children: <Widget>[
        ListTile(
          title: Text('DisplayName:Abp.Localization.DefaultLanguage'.tr),
          subtitle: Text('Description:Abp.Localization.DefaultLanguage'.tr),
          trailing: DropdownButton(
            onChanged: onChange,
            value: defaultLanguage,
            items: _languageList(),
          ),
        ),
        // _actionButton(
        //       title: '系统语言',
        //       suffix: Text('请选择'),
        //       onTap: () {

        //       },
        //     ),
      ],
    );
  }
}