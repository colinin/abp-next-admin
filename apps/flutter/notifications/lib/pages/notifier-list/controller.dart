import 'dart:async';

import 'package:core/models/abp.dto.dart';
import 'package:core/dependency/index.dart';
import 'package:core/services/subscription.service.dart';
import 'package:get/get.dart';
import 'package:notifications/models/index.dart';
import 'package:notifications/proxy/index.dart';

import 'state.dart';

class NotificationsController extends GetxController with ScrollMixin {
  final Rx<NotificationsState> _state = Rx<NotificationsState>(NotificationsState());
  NotificationsState get state => _state.value;

  MyNotificationAppService get _myNotificationAppService => injector.get();
  SubscriptionService get _subscriptionService => injector.get();

  Future<void> refreshState() async {
    var dto = await _myNotificationAppService.getList(UserNotificationGetByPagedDto(
      skipCount: state.skipCount,
      maxResultCount: state.maxResultCount,
    ));
    updateState(dto);
  }

  void showPayload(NotificationPaylod payload) {
    if (payload.state == NotificationReadState.unRead) {
      //_myNotificationAppService.
    }
  }

  Future<PagedResultDto<UserNotificationDto>?> fetch() {
    return _myNotificationAppService.getList(
      UserNotificationGetByPagedDto(
        skipCount: state.skipCount,
        maxResultCount: state.maxResultCount,
      ),
    );
  }

  void updateState(PagedResultDto<UserNotificationDto>? dto) {
    _state.update((val) {
      val!.hasMore = dto?.items.length == val.maxResultCount;
      if (val.skipCount == 0) {
        val.notifications = dto?.items ?? [];
      } else {
        val.notifications.addAll(dto?.items ?? []);
      }
    });
  }

  @override
  void onInit() {
    super.onInit();
    refreshState();
  }

  @override
  void onClose() {
    _subscriptionService.closeAll();
    super.onClose();
  }
  
  @override
  Future<void> onEndScroll() async {
    if (state.hasMore) {
      _state.update((val) {
        // 防止多次触发
        val!.hasMore = false;
        val.skipCount = val.skipCount + val.maxResultCount;
      });
      await refreshState();
    }
  }
  
  @override
  Future<void> onTopScroll() async {
    _state.update((val) {
      val!.skipCount = 0;
    });
    await refreshState();
  }
}