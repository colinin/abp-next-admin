import 'package:dev_app/main.module.dart';
import 'package:dev_app/utils/localization.dart';
import 'package:core/utils/theme.utils.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:core/utils/logging.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';

Future main() async {
  final module = MainModule();
  await module.initAsync();

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    MainModule module = Get.find();
    return GetMaterialApp(
      theme: ThemeUtils.lightTheme,
      darkTheme: ThemeUtils.darkTheme,
      themeMode: ThemeMode.system,
      initialRoute: '/',
      getPages: module.getRoutes(),
      debugShowMaterialGrid: false,
      enableLog: true,
      builder: EasyLoading.init(),
      translations: AbpTranslations(Get.find()),
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      logWriterCallback: (String text, {bool isError = false}) {
        if (isError) {
          logger.error(text);
        } else {
          logger.info(text);
        }
      },
      //supportedLocales: AbpLocalizations.supportedLocales,
    );
  }
}
