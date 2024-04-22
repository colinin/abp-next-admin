import 'package:flutter/material.dart';

class StatusBar extends StatelessWidget {
  const StatusBar({
    super.key,
  });
  
  @override
  Widget build(BuildContext context) {
    return Container(
      height: 190,
      decoration: const BoxDecoration(
        color: Color.fromARGB(255, 98, 168, 200),
      ),
    );
  }
}