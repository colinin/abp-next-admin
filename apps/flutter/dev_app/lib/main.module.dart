import 'package:dev_app/pages/index.dart';
import 'package:dev_app/utils/loading.dart';
import 'package:account/index.dart';
import 'package:components/index.dart';
import 'package:core/modularity/index.dart';
import 'package:core/index.dart';
import 'package:dio/dio.dart';
import 'package:flex_color_scheme/flex_color_scheme.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:notifications/notifications.module.dart';

import 'interceptors/index.dart';

class MainModule extends Module {
  @override
  List<Module> get dependencies => [
    CoreModule(),
    AccountModule(),
    NotificationsModule(),
  ];

  @override
  List<GetPage> get routes => [
    GetPage(
      name: '/',
      page: () => const MainPage(),
      bindings: [
        MainBinding(),
        HomeBinding(),
        WorkBinding(),
        CenterBinding(),
      ],
    ),
    ...PublicRoute.routes,
    ...CenterRoute.routes,
    ...SystemRoute.routes,
  ];

  @override
  void configureServices() {
    inject<MainModule>(this);

    inject(SignalrService('${Environment.current.baseUrl}/signalr-hubs/notifications'),
      tag: NotificationTokens.producer,
      permanent: true);

    lazyInject(() {
      var dio = Dio(BaseOptions(
        baseUrl: Environment.current.baseUrl,
      ));
      dio.interceptors.add(LoggerInterceptor());
      dio.interceptors.add(OAuthApiInterceptor());
      dio.interceptors.add(AppendHeaderInterceptor());
      dio.interceptors.add(AbpWrapperRemoteServiceErrorInterceptor());
      dio.interceptors.add(WrapperResultInterceptor());
      dio.interceptors.add(ExceptionInterceptor());

      return RestService(dio: dio);
    }, fenix: true);
    
    lazyInject<AuthService>(() => OAuthService(
      clientId: Environment.current.clientId,
      clientSecret: Environment.current.clientSecret
    ), fenix: true);
  }

  @override
  Future<Map<ModuleKey, List<GetPage>>> initAsync() async {
    WidgetsFlutterBinding.ensureInitialized();
    await SystemChrome.setPreferredOrientations([DeviceOrientation.portraitUp]);
    await Environment.initAsync();
    Loading();

    var moduleRoutes = await super.initAsync();

    initComponent();
    initThemeData();

    return moduleRoutes;
  }

  void initComponent() {
    if (!Environment.current.staticFilesUrl.isNullOrWhiteSpace()) {
      AvatarConfig.baseUrl = Environment.current.staticFilesUrl!;
    } else {
      AvatarConfig.baseUrl = '${Environment.current.baseUrl}/api/files/static/users/p/';
    }
  }

  void initThemeData() {
    ThemeUtils.lightTheme = FlexThemeData.light(
      scheme: FlexScheme.amber,
      surfaceMode: FlexSurfaceMode.levelSurfacesLowScaffold,
      blendLevel: 7,
      subThemesData: const FlexSubThemesData(
        blendOnLevel: 10,
        blendOnColors: false,
        useTextTheme: true,
        useM2StyleDividerInM3: true,
        thinBorderWidth: 0.5,
        thickBorderWidth: 0.5,
        defaultRadius: 0.0,
        cardRadius: 6.0,
      ),
      visualDensity: FlexColorScheme.comfortablePlatformDensity,
      useMaterial3: true,
      swapLegacyOnMaterial3: true,
      // To use the Playground font, add GoogleFonts package and uncomment
      // fontFamily: GoogleFonts.notoSans().fontFamily,
    );
    ThemeUtils.darkTheme = FlexThemeData.dark(
      scheme: FlexScheme.amber,
      surfaceMode: FlexSurfaceMode.levelSurfacesLowScaffold,
      blendLevel: 13,
      subThemesData: const FlexSubThemesData(
        blendOnLevel: 20,
        useTextTheme: true,
        useM2StyleDividerInM3: true,
        thinBorderWidth: 0.5,
        thickBorderWidth: 0.5,
        defaultRadius: 0.0,
        cardRadius: 6.0,
      ),
      visualDensity: FlexColorScheme.comfortablePlatformDensity,
      useMaterial3: true,
      swapLegacyOnMaterial3: true,
      // To use the Playground font, add GoogleFonts package and uncomment
      // fontFamily: GoogleFonts.notoSans().fontFamily,
    );
  }
}