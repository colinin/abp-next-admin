import 'package:core/dependency/index.dart';
import 'package:core/models/abp.dto.dart';
import 'package:core/models/request.dart';
import 'package:core/services/index.dart';
import 'package:platforms/modes/menu.dto.dart';

class FavoriteMenuService {
  FavoriteMenuService(this._injector);

  final Injector _injector;

  RestService get _restService => _injector.get<RestService>();

  Future<UserFavoriteMenuDto> createMyFavoriteMenu(UserFavoriteMenuCreateDto input, { RequestOptions? requestOptions }) {
    return _restService.request(
      url: '/api/platform/menus/favorites/my-favorite-menu',
      method: HttpMethod.POST,
      data: input,
      requestOptions: requestOptions,
      transformer: (response) => UserFavoriteMenuDto.fromJson(response.data),
    );
  }

  Future<void> deleteMyFavoriteMenu(String menuId, { RequestOptions? requestOptions }) {
    return _restService.request<void>(
      url: '/api/platform/menus/favorites/my-favorite-menu/$menuId',
      method: HttpMethod.DELETE,
      requestOptions: requestOptions,
    );
  }

  Future<ListResultDto<UserFavoriteMenuDto>> getMyFavoriteMenuList(String framework, { RequestOptions? requestOptions }) {
    return _restService.request(
      url: '/api/platform/menus/favorites/my-favorite-menus?framework=$framework',
      method: HttpMethod.GET,
      requestOptions: requestOptions,
      transformer: (response) => ListResultDto<UserFavoriteMenuDto>.fromJson(response.data,
        fromJsonT: (json) => UserFavoriteMenuDto.fromJson(json as Map<String, dynamic>)),
    );
  }

  Future<UserFavoriteMenuDto> updateMyFavoriteMenu(UserFavoriteMenuUpdateDto input, { RequestOptions? requestOptions }) {
    return _restService.request(
      url: '/api/platform/menus/favorites/my-favorite-menu',
      method: HttpMethod.PUT,
      data: input,
      requestOptions: requestOptions,
      transformer: (response) => UserFavoriteMenuDto.fromJson(response.data),
    );
  }
}