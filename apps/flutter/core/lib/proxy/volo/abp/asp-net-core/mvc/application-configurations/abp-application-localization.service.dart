import 'package:get/get.dart';

import 'models.dart';
import 'package:core/services/rest.service.dart';

class AbpApplicationLocalizationService {
  RestService get _restService => Get.find();

  Future<ApplicationLocalizationDto> get(ApplicationLocalizationRequestDto input) {
    return _restService.get('/api/abp/application-localization',
      queryParameters: {
        'cultureName': input.cultureName,
        'onlyDynamics': input.onlyDynamics
      }).then((res) => ApplicationLocalizationDto.fromJson(res.data));
  }
}