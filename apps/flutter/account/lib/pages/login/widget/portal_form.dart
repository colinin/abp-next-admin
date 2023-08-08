import 'package:core/models/auth.dart';
import 'package:flutter/material.dart';

class PortalForm extends StatelessWidget {
  const PortalForm({
    super.key,
    required this.portalProviders,
    this.onSelected,
  });

  final List<PortalLoginProvider> portalProviders;
  final void Function(PortalLoginProvider provider)? onSelected;
  
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 200,
      child: ListView(
        children: _buildLoginProviders(),
      ),
    );
  }

  List<Widget> _buildLoginProviders() {
    List<Widget> providers = [];

    for (var provider in portalProviders) {
      providers.add(Card(
        child: ListTile(
          title: Text(provider.name),
            leading: const FlutterLogo(),
            onTap: () {
              onSelected?.call(provider);
            },
          ),
        ),
      );
    }
    
    return providers;
  }
}