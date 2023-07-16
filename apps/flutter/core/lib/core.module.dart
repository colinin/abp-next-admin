import 'package:core/dependency/injector.dart';
import 'package:core/modularity/module.dart';
import 'package:core/services/index.dart';
import 'proxy/index.dart';

class CoreModule extends Module {

  @override
  Future<void> configureServicesAsync() async {
    await injectAsync<StorageService>(() async {
      if (await GetxStorageService.init()) {
        return GetxStorageService();
      }
      return StorageService();
    }, permanent: true);
    await super.configureServicesAsync();
  }

  @override
  void configureServices() {
    inject(Injector.instance);
    inject<CoreModule>(this);
    inject(ConfigStateService(), permanent: true);
    inject(ThemeService(), permanent: true);

    lazyInject(() => SessionService(), fenix: true);
    lazyInject(() => SubscriptionService(), fenix: true);
    lazyInject(() => LanguageService(), fenix: true);
    lazyInject(() => LocalizationService(), fenix: true);
    lazyInject(() => AbpTenantService(), fenix: true);
    lazyInject(() => AbpApiDefinitionService(), fenix: true);
    lazyInject(() => AbpApplicationLocalizationService(), fenix: true);
    lazyInject(() => AbpApplicationConfigurationService(), fenix: true);
  }
}