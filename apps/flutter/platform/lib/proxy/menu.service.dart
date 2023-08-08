import 'package:core/dependency/index.dart';
import 'package:core/models/abp.dto.dart';
import 'package:core/models/request.dart';
import 'package:core/services/rest.service.dart';
import 'package:platforms/modes/menu.dto.dart';

class MenuService {
  MenuService(this._injector);

  final Injector _injector;

  RestService get _restService => _injector.get<RestService>();

  Future<ListResultDto<MenuDto>> getCurrentUserMenuList(String framework, { RequestOptions? requestOptions }) {
    return _restService.request(
      url: '/api/platform/menus/by-current-user?framework=$framework',
      method: HttpMethod.GET,
      requestOptions: requestOptions,
      transformer: (response) => ListResultDto<MenuDto>.fromJson(response.data,
        fromJsonT: (json) => MenuDto.fromJson(json as Map<String, dynamic>)),
    );
  }
}