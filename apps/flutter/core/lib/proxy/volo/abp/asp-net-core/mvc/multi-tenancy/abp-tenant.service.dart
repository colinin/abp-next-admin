import 'package:core/services/rest.service.dart';
import 'package:get/get.dart';

import 'models.dart';

class AbpTenantService {
  RestService get _restService => Get.find();

  Future<FindTenantResultDto> findTenantById(String id) {
    return _restService.get('/api/abp/multi-tenancy/tenants/by-id/$id')
    .then((res) => FindTenantResultDto.fromJson(res.data));
  }

  Future<FindTenantResultDto> findTenantByName(String name) {
    return _restService.get('/api/abp/multi-tenancy/tenants/by-name/$name')
    .then((res) => FindTenantResultDto.fromJson(res.data));
  }
}