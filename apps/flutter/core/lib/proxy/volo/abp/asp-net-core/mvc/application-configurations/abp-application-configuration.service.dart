import 'package:core/proxy/volo/abp/asp-net-core/mvc/application-configurations/index.dart';
import 'package:core/services/rest.service.dart';
import 'package:get/get.dart';

class AbpApplicationConfigurationService {
  RestService get _restService => Get.find();

  Future<ApplicationConfigurationDto> get(ApplicationConfigurationRequestOptions options) {
    return _restService.get('/api/abp/application-configuration',
      queryParameters: {
        'includeLocalizationResources': options.includeLocalizationResources
      }).then((res) {
        return ApplicationConfigurationDto.fromJson(res.data);
      });
  }
}