
import 'package:core/proxy/volo/abp/http/modeling/index.dart';
import 'package:core/services/rest.service.dart';
import 'package:get/get.dart';

class AbpApiDefinitionService {
  final RestService _restService = Get.find();

  Future<ApplicationApiDescriptionModel> getByModel(ApplicationApiDescriptionModelRequestDto model) {
    return _restService.get('/api/abp/api-definition',
      queryParameters: {
        'includeTypes': model.includeTypes
      }).then((res) => ApplicationApiDescriptionModel.fromJson(res.data));
  }
}