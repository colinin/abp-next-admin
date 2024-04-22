
import 'package:core/dependency/injector.dart';
import 'package:core/proxy/volo/abp/http/modeling/index.dart';
import 'package:core/services/rest.service.dart';

class AbpApiDefinitionService {
  AbpApiDefinitionService(Injector injector):
    _restService = injector.get<RestService>();
  final RestService _restService;

  Future<ApplicationApiDescriptionModel> getByModel(ApplicationApiDescriptionModelRequestDto model) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/abp/api-definition',
      queryParameters: {
        'includeTypes': model.includeTypes
      },
      transformer: (res) => ApplicationApiDescriptionModel.fromJson(res.data),
    );
  }
}