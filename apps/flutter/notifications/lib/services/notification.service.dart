import 'package:core/models/abp.dto.dart';
import 'package:core/services/rest.service.dart';
import 'package:core/services/service.base.dart';

import '../models/notification.dart';

class NotificationService extends ServiceBase {
  RestService get _restService => find();

  Future<void> sendAsyncByInput(NotificationSendDto input) {
    return _restService.post('/api/notifications',
      data: input,
    );
  }

  Future<void> subscribe(String name) {
    return _restService.post<void>('/api/notifications/my-subscribes',
      data: {
        'name': name,
      },
    );
  }

  Future<void> unSubscribe(String name) {
    return _restService.delete<void>('/api/notifications/my-subscribes?name=$name');
  }

  Future<ListResultDto<UserSubscreNotificationDto>> getMySubscribedListAsync() {
    return _restService.get('/api/notifications/my-subscribes/all',
    ).then((res) {
      return ListResultDto<UserSubscreNotificationDto>(
        items: (res.data['items'] as List<dynamic>).map((e) => UserSubscreNotificationDto.fromJson(e)).toList(),
      );
    });
  }

  Future<ListResultDto<NotificationGroupDto>> getAssignableNotifiersAsync() {
    return _restService.get('/api/notifications/assignables',
    ).then((res) {
      return ListResultDto<NotificationGroupDto>(
        items: (res.data['items'] as List<dynamic>).map((e) => NotificationGroupDto.fromJson(e)).toList(),
      );
    });
  }

  Future<ListResultDto<NotificationTemplateDto>> getAssignableTemplatesAsync() {
    return _restService.get('/api/notifications/assignable-templates',
    ).then((res) => ListResultDto<NotificationTemplateDto>(
      items: (res.data['items'] as List<dynamic>)
        .map((e) => NotificationTemplateDto.fromJson(e)).toList()
      ),
    );
  }
}