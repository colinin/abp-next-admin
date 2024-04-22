import 'package:core/dependency/injector.dart';
import 'package:core/proxy/volo/abp/asp-net-core/mvc/application-configurations/index.dart';
import 'package:core/services/rest.service.dart';

class AbpApplicationConfigurationService {
  AbpApplicationConfigurationService(Injector injector):
    _restService = injector.get<RestService>();
  final RestService _restService;

  Future<ApplicationConfigurationDto> get(ApplicationConfigurationRequestOptions options) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/abp/application-configuration',
      queryParameters: {
        'includeLocalizationResources': options.includeLocalizationResources
      },
      transformer: (res) => ApplicationConfigurationDto.fromJson(res.data),
    );
  }
}