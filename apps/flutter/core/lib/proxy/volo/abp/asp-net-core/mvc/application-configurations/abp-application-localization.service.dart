import 'package:core/dependency/injector.dart';

import 'models.dart';
import 'package:core/services/rest.service.dart';

class AbpApplicationLocalizationService {
  AbpApplicationLocalizationService(Injector injector):
    _restService = injector.get<RestService>();
  final RestService _restService;

  Future<ApplicationLocalizationDto> get(ApplicationLocalizationRequestDto input) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/abp/application-localization',
      queryParameters: {
        'cultureName': input.cultureName,
        'onlyDynamics': input.onlyDynamics
      },
      transformer: (res) => ApplicationLocalizationDto.fromJson(res.data)
    );
  }
}