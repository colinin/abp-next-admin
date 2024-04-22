import 'package:core/modularity/module.dart';
import 'package:core/services/index.dart';
import 'proxy/index.dart';

class CoreModule extends Module {
  @override
  Future<void> configureServicesAsync() async {
    await injectAsync<StorageService>((injector) async {
      if (await GetxStorageService.init()) {
        return GetxStorageService(injector);
      }
      return StorageService(injector);
    }, permanent: true);
    await super.configureServicesAsync();
  }

  @override
  void configureServices() {
    inject<CoreModule>(this, permanent: true);
    inject(EnvironmentService(injector), permanent: true);
    inject(ConfigStateService(injector), permanent: true);
    inject(ThemeService(injector), permanent: true);
    inject(ErrorReporterService(), permanent: true);

    lazyInject((injector) => SubscriptionService(injector));
    lazyInject((injector) => SessionService(injector), fenix: true);
    lazyInject((injector) => LanguageService(injector), fenix: true);
    lazyInject((injector) => LocalizationService(injector), fenix: true);
    lazyInject((injector) => AbpTenantService(injector), fenix: true);
    lazyInject((injector) => AbpApiDefinitionService(injector), fenix: true);
    lazyInject((injector) => AbpApplicationLocalizationService(injector), fenix: true);
    lazyInject((injector) => AbpApplicationConfigurationService(injector), fenix: true);
  }
}