import 'package:core/dependency/index.dart';
import 'package:get/get.dart';
import 'package:flutter/material.dart';

abstract class BasePage<Bloc> extends GetView<Bloc> {
  const BasePage({Key? key}) : super(key: key);
  
  Bloc get bloc => Injector.instance.get(tag: tag);
}