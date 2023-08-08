import 'package:components/index.dart';
import 'package:dev_app/pages/index.dart';
import 'package:dev_app/pages/public/route.name.dart';
import 'package:dev_app/services/index.dart';
import 'package:dev_app/utils/initial.utils.dart';
import 'package:dev_app/utils/loading.dart';
import 'package:account/index.dart';
import 'package:core/modularity/index.dart';
import 'package:core/index.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:notifications/index.dart';
import 'package:oauth/index.dart';
import 'package:platforms/index.dart';

import 'handlers/error.handler.dart';
import 'interceptors/index.dart';

class MainModule extends Module {
  @override
  List<Module> get dependencies => [
    ComponentsModule(),
    OAuthModule(),
    AccountModule(),
    NotificationsModule(),
    PlatformModule(),
  ];

  @override
  List<GetPage> get routes => [
    GetPage(
      name: PublicRoutes.main,
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
    inject<ApplicationInitialService>(FlutterApplicationInitialService());

    lazyInject((injector) => ErrorHandler(injector));
    lazyInject<SignalrService>((injector) {
      var environmentService = get<EnvironmentService>();
      var environment = environmentService.getEnvironment();

      if (environment.notifications?.serverUrl.isNullOrWhiteSpace() == true) {
        return NullSignalrService();
      }

      return AspNetCoreSignalrService(environment.notifications!.serverUrl!);
    }, tag: NotificationTokens.producer);
    lazyInject<RestService>((injector) {
      var environmentService = injector.get<EnvironmentService>();
      var dio = Dio();
      dio.interceptors.add(LoggerInterceptor());
      dio.interceptors.add(ApiAuthorizationInterceptor());
      dio.interceptors.add(AppendHeaderInterceptor());
      //dio.interceptors.add(AbpWrapperRemoteServiceErrorInterceptor());
      dio.interceptors.add(WrapperResultInterceptor());
      dio.interceptors.add(ExceptionInterceptor(get()));

      return RestService(dio, environmentService);
    });
    lazyInject<TranslationService>((ioc) {
      var environmentService = get<EnvironmentService>();
      var environment = environmentService.getEnvironment();
      // 使用资源文件中的本地化文档
      if (environment.localization.useLocalResources == true) {
        return TranslationResService(ioc);
      }
      // 使用本地化服务提供的本地化支持
      return ioc.get<LocalizationService>();
    }, fenix: true);
  }

  @override
  Future<Map<ModuleKey, List<GetPage>>> initAsync() async {
    WidgetsFlutterBinding.ensureInitialized();
    await SystemChrome.setPreferredOrientations([DeviceOrientation.portraitUp]);
    Loading();

    inject(injector, permanent: true);
    var moduleRoutes = await super.initAsync();

    await InitialUtils.initialState(get<Injector>());

    InitialUtils.initComponent(get<Injector>());
    //InitialUtils.initThemeData();

    return moduleRoutes;
  }
}