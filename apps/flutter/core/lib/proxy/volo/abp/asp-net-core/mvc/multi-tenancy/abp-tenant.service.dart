import 'package:core/dependency/injector.dart';
import 'package:core/services/rest.service.dart';

import 'models.dart';

class AbpTenantService {
  AbpTenantService(Injector injector):
    _restService = injector.get<RestService>();
  final RestService _restService;

  Future<FindTenantResultDto> findTenantById(String id) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/abp/multi-tenancy/tenants/by-id/$id',
      transformer: (res) => FindTenantResultDto.fromJson(res.data),
    );
  }

  Future<FindTenantResultDto> findTenantByName(String name) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/abp/multi-tenancy/tenants/by-name/$name',
      transformer: (res) => FindTenantResultDto.fromJson(res.data),
    );
  }
}