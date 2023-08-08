import 'package:core/models/abp.dto.dart';
import 'package:core/models/request.dart';
import 'package:core/services/rest.service.dart';
import 'package:core/services/service.base.dart';

import '../models/notification.dart';

class NotificationService extends ServiceBase {
  NotificationService(super._injector);

  RestService get _restService => resolve<RestService>();

  Future<void> sendAsyncByInput(NotificationSendDto input, { RequestOptions? requestOptions }) {
    return _restService.request(
      method: HttpMethod.POST,
      url: '/api/notifications',
      data: input,
      requestOptions: requestOptions,
    );
  }

  Future<void> subscribe(String name, { RequestOptions? requestOptions }) {
    return _restService.request(
      method: HttpMethod.POST,
      url: '/api/notifications/my-subscribes',
      data: {
        'name': name,
      },
      requestOptions: requestOptions,
    );
  }

  Future<void> unSubscribe(String name, { RequestOptions? requestOptions }) {
    return _restService.request<void>(
      method: HttpMethod.DELETE,
      url: '/api/notifications/my-subscribes?name=$name',
      requestOptions: requestOptions,
    );
  }

  Future<ListResultDto<UserSubscreNotificationDto>> getMySubscribedListAsync({ RequestOptions? requestOptions }) {
    return _restService.request(
      method: HttpMethod.GET,
      requestOptions: requestOptions,
      url: '/api/notifications/my-subscribes/all',
      transformer: (res) => ListResultDto<UserSubscreNotificationDto>(
        items: (res.data['items'] as List<dynamic>).map((e) => UserSubscreNotificationDto.fromJson(e)).toList(),
      ),
    );
  }

  Future<ListResultDto<NotificationGroupDto>> getAssignableNotifiersAsync({ RequestOptions? requestOptions }) {
    return _restService.request(
      method: HttpMethod.GET,
      requestOptions: requestOptions,
      url: '/api/notifications/assignables',
      transformer: (res) => ListResultDto<NotificationGroupDto>(
        items: (res.data['items'] as List<dynamic>).map((e) => NotificationGroupDto.fromJson(e)).toList(),
      ),
    );
  }

  Future<ListResultDto<NotificationTemplateDto>> getAssignableTemplatesAsync({ RequestOptions? requestOptions }) {
    return _restService.request(
      method: HttpMethod.GET,
      requestOptions: requestOptions,
      url: '/api/notifications/assignable-templates',
      transformer: (res) => ListResultDto<NotificationTemplateDto>(
      items: (res.data['items'] as List<dynamic>)
        .map((e) => NotificationTemplateDto.fromJson(e)).toList()
      ),
    );
  }
}