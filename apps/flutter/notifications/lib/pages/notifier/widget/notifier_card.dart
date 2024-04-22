import 'package:flutter/material.dart' hide Notification;

import '../../../models/index.dart';

class NotifierCard extends StatelessWidget {
  const NotifierCard({
    super.key,
    required this.groups,
    this.onChange,
  });

  final List<NotificationGroup> groups;
  final void Function(Notification notification, bool checked)? onChange;

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      child: Card(
        margin: const EdgeInsets.all(10),
        child: Column(
          children: _notifierList(),
        ),
      ),
    );
  }

  List<Widget> _notifierList() {
    if (groups.isEmpty == true) return [];
    List<Widget> notifierItems = [];
    for (var group in groups) {
      if (group.notifications.isEmpty == true) continue;
      notifierItems.add(ExpansionTile(
        title: Text(group.displayName),
        children: _buildNotifierItems(group),
      ));
    }
    return notifierItems;
  }

  List<Widget> _buildNotifierItems(NotificationGroup group) {
    List<Widget> children = [];
    for (var item in group.notifications) {
      children.add(SwitchListTile(
        title: Text(item.displayName),
        subtitle: Text(item.description ?? ''),
        value: item.isSubscribed,
        onChanged: item.loading == false ? (checked) {
          if (onChange != null) {
            onChange!(item, checked);
          }
        } : null,
      ));
    }
    return children;
  }
}