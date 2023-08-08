import 'package:core/dependency/index.dart';
import 'package:core/services/index.dart';
import 'package:core/tokens/index.dart';
import 'package:core/utils/index.dart';
import 'package:dev_app/services/notification.send.local.service.dart';
import 'package:flex_color_scheme/flex_color_scheme.dart';

class InitialUtils {
  static Future<void> initialState(Injector injector) async {
    /// 初始化应用程序
    var applicationInitialService = injector.get<ApplicationInitialService>();
    var application = await applicationInitialService.initialApp();
    injector.inject(application, tag: AbpTokens.application);
    /// 初始化环境配置
    var environmentService = injector.get<EnvironmentService>();
    environmentService.setState(application.environment);

    await EnvironmentUtils.mergeRemoteEnvironment(injector, application.environment);

    /// 放在初始化环境之后
    await injector.injectAsync<NotificationSendService>((injector) async {
      var service = FlutterLocalNotificationsSendService(injector);
      await service.initAsync();
      return service;
    }, permanent: true);
  }

  static void initThemeData() {
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

  static void initComponent(Injector injector) {
  }
}