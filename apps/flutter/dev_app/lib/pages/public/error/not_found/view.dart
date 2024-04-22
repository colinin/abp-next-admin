import 'package:bruno/bruno.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class PageNotFound extends StatelessWidget {
  const PageNotFound({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("404Message".tr)
      ),
      body: BrnAbnormalStateWidget(
        img: Image.asset(
          'res/images/no_data.png',
          scale: 3.0,
        ),
        content: "404MessageDetail".tr,
      ),
    );
  }
}