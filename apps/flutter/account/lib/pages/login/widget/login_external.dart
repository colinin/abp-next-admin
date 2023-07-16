import 'package:flutter/material.dart';

import '../state.dart';

class LoginExternal extends StatelessWidget {
  const LoginExternal({
    super.key,
    required this.state,
  });

  final LoginState state;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceAround,
      children: [
        IconButton(
          onPressed: () {

          },
          iconSize: 28.0,
          color: Colors.green,
          icon: const Icon(Icons.wechat),
        ),
        IconButton(
          onPressed: () {

          },
          iconSize: 28.0,
          color: Colors.teal,
          icon: const Icon(Icons.phone_iphone)
        ),
      ],
    );
  }
}