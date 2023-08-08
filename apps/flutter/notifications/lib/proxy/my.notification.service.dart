import 'package:core/models/index.dart';
import 'package:core/services/rest.service.dart';
import 'package:core/services/service.base.dart';
import 'package:notifications/models/index.dart';

class MyNotificationAppService extends ServiceBase {
  MyNotificationAppService(super._injector);
  
  RestService get _restService => resolve<RestService>();

  Future<PagedResultDto<UserNotificationDto>> getList([UserNotificationGetByPagedDto? dto]) {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/api/notifications/my-notifilers',
      transformer: (res) {
        return PagedResultDto<UserNotificationDto>.fromJson(
          res.data!,
          fromJsonT: (json) => UserNotificationDto.fromJson(json as Map<String, dynamic>),);
      },
      queryParameters: dto?.toJson(),
    );
  }
}